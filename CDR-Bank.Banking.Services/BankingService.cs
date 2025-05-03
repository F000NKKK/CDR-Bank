using CDR_Bank.Banking.Services.Abstractions;
using CDR_Bank.DataAccess.Banking;
using CDR_Bank.DataAccess.Banking.Entities;
using CDR_Bank.DataAccess.Banking.Enums;
using CDR_Bank.DataAccess.Identity;
using CDR_Bank.DataAccess.Models;
using CDR_Bank.Hub.Services.Abstractions;
using CDR_Bank.Libs.Banking.Contracts.RequestContracts;
using CDR_Bank.Libs.Banking.Contracts.ResponseContracts;

namespace CDR_Bank.Banking.Services
{
    internal class BankingService : IBankingService
    {
        private readonly BankingDataContext _bankingDataContext;
        private readonly IAccountValidationService _accountValidationService;
        private readonly IdentityDataContext _identityDataContext;

        public BankingService(BankingDataContext bankingDataContext, IdentityDataContext identityDataContext, IAccountValidationService accountValidationService)
        {
            _identityDataContext = identityDataContext;
            _bankingDataContext = bankingDataContext;
            _accountValidationService = accountValidationService;
        }

        public PagedResult<AccountTransactionContract> GetTransactions(Guid userId, TransactionFilterContract filter)
        {
            var query = _bankingDataContext.Transactions
                .Where(t => _bankingDataContext.BankAccounts
                    .Any(a => a.Id == t.BankingAccountId && a.UserId == userId));

            if (filter.BankingAccountId.HasValue)
            {
                query = query.Where(t => t.BankingAccountId == filter.BankingAccountId.Value);
            }

            var totalCount = query.Count();

            var transactions = query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return new PagedResult<AccountTransactionContract>
            {
                Items = transactions.Select(s => new AccountTransactionContract()
                {
                    Amount = s.Amount,
                    BankingAccountId = s.BankingAccountId,
                    CounterpartyAccountId = s.CounterpartyAccountId,
                    CreatedAt = s.CreatedAt,
                    Description = s.Description,
                    Id = s.Id,
                    Status = (CDR_Bank.Libs.Banking.Contracts.Enums.TransactionStatus)(int)s.Status,
                    Type = (CDR_Bank.Libs.Banking.Contracts.Enums.TransactionType)(int)s.Type,
                }).ToList(),
                TotalCount = totalCount
            };
        }

        public PagedResult<BankingAccountContract> GetAccounts(Guid userId, int page, int pageSize)
        {
            var query = _bankingDataContext.BankAccounts
                .Where(a => a.UserId == userId && a.State == AccountState.Open)
                .OrderBy(a => a.Name);

            var totalCount = query.Count();

            var accounts = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<BankingAccountContract>
            {
                Items = accounts.Select(s => new BankingAccountContract()
                {
                    AccountNumber = s.AccountNumber,
                    Balance = s.Balance,
                    CreditLimit = s.CreditLimit,
                    Name = s.Name,
                    State = (CDR_Bank.Libs.Banking.Contracts.Enums.AccountState)(int)s.State,
                    Type = (CDR_Bank.Libs.Banking.Contracts.Enums.BankAccountType)(int)s.Type,
                    Id = s.Id,
                    IsMain = s.IsMain,
                    UserId = s.UserId
                }).ToList(),
                TotalCount = totalCount
            };
        }

        public void Replenish(Guid userId, Guid bankingAccountId, decimal amount)
        {
            var account = _accountValidationService.GetAccountIfOpen(bankingAccountId)
                          ?? throw new InvalidOperationException("Account not found or is closed.");

            CheckAndAddBonusToDebit(userId, amount);

            account.Balance += amount;

            _bankingDataContext.Transactions.Add(new AccountTransaction
            {
                BankingAccountId = bankingAccountId,
                Type = TransactionType.Replenish,
                Status = TransactionStatus.Completed,
                Amount = amount,
                CreatedAt = DateTime.UtcNow,
                Description = "Account replenishment"
            });

            _bankingDataContext.SaveChanges();
        }

        private void CheckAndAddBonusToDebit(Guid userId, decimal amount)
        {
            if (amount > AMOUNT_FOR_THE_BONUS)
            {
                var account = _bankingDataContext.BankAccounts.Where(a => a.UserId == userId).FirstOrDefault(a => a.Type == BankAccountType.Debit && a.State == AccountState.Open);

                if (account is not null)
                {
                    account.Balance += BONUS_AMOUNT;

                    _bankingDataContext.Transactions.Add(new AccountTransaction
                    {
                        BankingAccountId = account.Id,
                        Type = TransactionType.Replenish,
                        Status = TransactionStatus.Completed,
                        Amount = amount,
                        CreatedAt = DateTime.UtcNow,
                        Description = "The deposit bonus is more than one million"
                    });
                }
            }
        }

        private BankAccount? GetAccountIfOpen(Guid bankingAccountId)
        {
            var account = _bankingDataContext.BankAccounts
                .FirstOrDefault(a => a.Id == bankingAccountId && a.State == AccountState.Open);

            if (account != null)
            {
                return account;
            }

            return null;
        }

        public bool Transfer(Guid bankingAccountId, string recipientTelephone, decimal amount)
        {
            var sender = _accountValidationService.GetAccountIfOpen(bankingAccountId);
            if (!_accountValidationService.CanTransfer(sender, amount))
                return false;

            var recipientUserId = _identityDataContext.ContactInfos.Where(f => f.PhoneNumber == recipientTelephone).Select(s => s.UserId).FirstOrDefault();

            var recipient = _bankingDataContext.BankAccounts
                .FirstOrDefault(a => a.UserId == recipientUserId
                                     && a.State == AccountState.Open
                                     && a.IsMain);
            if (recipient == null)
                return false;

            sender.Balance -= amount;
            recipient.Balance += amount;

            _bankingDataContext.Transactions.AddRange(new[]
            {
                new AccountTransaction
                {
                    BankingAccountId = recipient.Id,
                    CounterpartyAccountId = sender.Id,
                    Type = TransactionType.Replenish,
                    Status = TransactionStatus.Completed,
                    Amount = amount,
                    CreatedAt = DateTime.UtcNow,
                    Description = "Received transfer from another user"
                },
                new AccountTransaction
                {
                    BankingAccountId = sender.Id,
                    CounterpartyAccountId = recipient.Id,
                    Type = TransactionType.TransferToUser,
                    Status = TransactionStatus.Completed,
                    Amount = amount,
                    CreatedAt = DateTime.UtcNow,
                    Description = "Transfer to another user"
                }
            });

            _bankingDataContext.SaveChanges();
            return true;
        }

        public bool Withdraw(Guid bankingAccountId, decimal amount)
        {
            var account = _accountValidationService.GetAccountIfOpen(bankingAccountId);
            if (!_accountValidationService.CanWithdraw(account, amount))
                return false;

            account.Balance -= amount;

            _bankingDataContext.Transactions.Add(new AccountTransaction
            {
                BankingAccountId = bankingAccountId,
                Type = TransactionType.Withdraw,
                Status = TransactionStatus.Completed,
                Amount = amount,
                CreatedAt = DateTime.UtcNow,
                Description = "Withdrawal from account"
            });

            _bankingDataContext.SaveChanges();
            return true;
        }


        public bool InternalTransfer(Guid sourceId, Guid destId, decimal amount)
        {
            var source = _accountValidationService.GetAccountIfOpen(sourceId);
            var dest = _accountValidationService.GetAccountIfOpen(destId);

            if (source == null || dest == null)
                return false;

            if (!_accountValidationService.CanTransfer(source, amount))
                return false;

            source.Balance -= amount;
            dest.Balance += amount;

            _bankingDataContext.Transactions.Add(new AccountTransaction
            {
                BankingAccountId = source.Id,
                CounterpartyAccountId = dest.Id,
                Type = TransactionType.TransferBetweenAccounts,
                Status = TransactionStatus.Completed,
                Amount = amount,
                CreatedAt = DateTime.UtcNow,
                Description = "Internal account transfer"
            });

            _bankingDataContext.SaveChanges();
            return true;
        }

        public decimal GetBalance(Guid userId)
            => _bankingDataContext.BankAccounts.Where(a => a.UserId == userId).Sum(a => a.Balance);

        public bool CloseAccount(Guid bankingAccountId)
        {
            var account = _accountValidationService.GetAccountIfOpen(bankingAccountId);
            if (account == null) return false;

            account.State = AccountState.Closed;
            _bankingDataContext.SaveChanges();
            return true;
        }

        public BankingAccountContract? GetAccountData(Guid bankingAccountId)
        {
            var account = GetAccountIfOpen(bankingAccountId);

            return new BankingAccountContract()
            {
                Balance = account?.Balance ?? 0m,
                CreditLimit = account?.CreditLimit,
                Id = account?.Id ?? Guid.Empty,
                IsMain = account?.IsMain ?? false,
                Name = account?.Name ?? string.Empty,
                State = (CDR_Bank.Libs.Banking.Contracts.Enums.AccountState)(int)(account?.State ?? AccountState.Closed),
                Type = (CDR_Bank.Libs.Banking.Contracts.Enums.BankAccountType)(int)(account?.Type ?? BankAccountType.Savings),
                UserId = account?.UserId ?? Guid.Empty,
                AccountNumber = account?.AccountNumber ?? string.Empty
            };
        }

        public Guid CreateAccount(Guid userId, string name, BankAccountType type, decimal? creditLimit = null, bool isMain = false)
        {
            if (isMain)
            {
                var others = _bankingDataContext.BankAccounts
                    .Where(a => a.UserId == userId && a.IsMain).ToList();
                foreach (var o in others)
                {
                    o.IsMain = false;
                }
            }

            var account = new BankAccount
            {
                UserId = userId,
                Name = name,
                Type = type,
                State = AccountState.Open,
                Balance = 0m,
                CreditLimit = type == BankAccountType.Credit ? creditLimit : null,
                IsMain = isMain
            };

            _bankingDataContext.BankAccounts.Add(account);
            _bankingDataContext.SaveChanges();
            return account.Id;
        }

        public bool EditAccount(Guid bankingAccountId, string? name, BankAccountType? type, decimal? creditLimit, bool? isMain = null)
        {
            var account = _accountValidationService.GetAccountIfOpen(bankingAccountId);
            if (account == null) return false;

            if (!string.IsNullOrWhiteSpace(name)) account.Name = name;
            if (type.HasValue) account.Type = type.Value;
            if (type == BankAccountType.Credit && creditLimit.HasValue)
                account.CreditLimit = creditLimit.Value;

            if (isMain.HasValue && isMain.Value && !account.IsMain)
            {
                var others = _bankingDataContext.BankAccounts
                    .Where(a => a.UserId == account.UserId && a.IsMain && a.Id != account.Id).ToList();
                foreach (var o in others) o.IsMain = false;
                account.IsMain = true;
            }

            _bankingDataContext.SaveChanges();
            return true;
        }

        public const decimal AMOUNT_FOR_THE_BONUS = 1_000_000m;
        public const decimal BONUS_AMOUNT = 2_000m;
    }
}
