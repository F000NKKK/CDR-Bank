using CDR_Bank.Banking.Services.Abstractions;
using CDR_Bank.DataAccess.Banking;
using CDR_Bank.DataAccess.Banking.Entities;
using CDR_Bank.DataAccess.Banking.Enums;
using CDR_Bank.DataAccess.Models;
using CDR_Bank.Libs.Banking.Contracts.RequestContracts;
using CDR_Bank.Libs.Banking.Contracts.ResponseContracts;

namespace CDR_Bank.Banking.Services
{
    internal class BankingService : IBankingService
    {
        private readonly BankingDataContext _bankingDataContext;

        public BankingService(BankingDataContext bankingDataContext)
        {
            _bankingDataContext = bankingDataContext;
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
                    TelephoneNumber = s.TelephoneNumber,
                    UserId = s.UserId
                }).ToList(),
                TotalCount = totalCount
            };
        }

        public void Replenish(Guid bankingAccountId, decimal amount)
        {
            var account = _bankingDataContext.BankAccounts
                .FirstOrDefault(a => a.Id == bankingAccountId && a.State == AccountState.Open);

            if (account == null)
                throw new InvalidOperationException("Account not found or is closed.");

            account.Balance += amount;

            var transaction = new AccountTransaction
            {
                BankingAccountId = bankingAccountId,
                Type = TransactionType.Replenish,
                Status = TransactionStatus.Completed,
                Amount = amount,
                CreatedAt = DateTime.UtcNow,
                Description = "Account replenishment"
            };

            _bankingDataContext.Transactions.Add(transaction);

            ApplyDataBaseTransaction();
        }

        private BankingAccountContract? GetAccountIfOpen(Guid bankingAccountId)
        {
            var account = _bankingDataContext.BankAccounts
                .FirstOrDefault(a => a.Id == bankingAccountId && a.State == AccountState.Open);

            if (account != null)
            {
                return new BankingAccountContract()
                {
                    AccountNumber = account.AccountNumber,
                    Balance = account.Balance,
                    CreditLimit = account.CreditLimit,
                    Name = account.Name,
                    State = (CDR_Bank.Libs.Banking.Contracts.Enums.AccountState)(int)account.State,
                    Type = (CDR_Bank.Libs.Banking.Contracts.Enums.BankAccountType)(int)account.Type,
                    IsMain = account.IsMain,
                    TelephoneNumber = account.TelephoneNumber,
                    UserId = account.UserId,
                    Id = account.Id
                };
            }

            return null;
        }

        public bool Transfer(Guid bankingAccountId, string recipientTelephoneNumber, decimal amount)
        {
            var senderAccount = GetAccountIfOpen(bankingAccountId);
            if (senderAccount == null || senderAccount.Balance < amount)
                return false;

            if (senderAccount == null)
                return false;

            var maxAvailable = senderAccount.Type == Libs.Banking.Contracts.Enums.BankAccountType.Credit
            ? senderAccount.Balance + (senderAccount.CreditLimit ?? 0)
                : senderAccount.Balance;

            if (maxAvailable < amount)
                return false;

            var recipientAccount = _bankingDataContext.BankAccounts
                .FirstOrDefault(a => a.TelephoneNumber == recipientTelephoneNumber && a.State == AccountState.Open && a.IsMain);

            if (recipientAccount == null)
                return false;

            senderAccount.Balance -= amount;
            recipientAccount.Balance += amount;

            var recipientTransaction = new AccountTransaction
            {
                BankingAccountId = recipientAccount.Id,
                CounterpartyAccountId = senderAccount.Id,
                Type = TransactionType.TransferToUser,
                Status = TransactionStatus.Completed,
                Amount = amount,
                CreatedAt = DateTime.UtcNow,
                Description = "Received transfer from another user"
            };

            var senderTransaction = new AccountTransaction
            {
                BankingAccountId = bankingAccountId,
                CounterpartyAccountId = recipientAccount.Id,
                Type = TransactionType.TransferToUser,
                Status = TransactionStatus.Completed,
                Amount = amount,
                CreatedAt = DateTime.UtcNow,
                Description = "Transfer to another user"
            };

            _bankingDataContext.Transactions.Add(recipientTransaction);
            _bankingDataContext.Transactions.Add(senderTransaction);

            ApplyDataBaseTransaction();

            return true;
        }

        public bool Withdraw(Guid bankingAccountId, decimal amount)
        {
            var account = GetAccountIfOpen(bankingAccountId);

            if (account == null)
                return false;

            var maxAvailable = account.Type == Libs.Banking.Contracts.Enums.BankAccountType.Credit
                ? account.Balance + (account.CreditLimit ?? 0)
                : account.Balance;

            if (maxAvailable < amount)
                return false;

            account.Balance -= amount;

            var transaction = new AccountTransaction
            {
                BankingAccountId = bankingAccountId,
                Type = TransactionType.Withdraw,
                Status = TransactionStatus.Completed,
                Amount = amount,
                CreatedAt = DateTime.UtcNow,
                Description = "Withdrawal from account"
            };

            _bankingDataContext.Transactions.Add(transaction);

            ApplyDataBaseTransaction();

            return true;
        }

        public bool InternalTransfer(Guid sourceAccountId, Guid destinationAccountId, decimal amount)
        {
            var source = GetAccountIfOpen(sourceAccountId);
            var dest = GetAccountIfOpen(destinationAccountId);

            if (source == null || dest == null || source.Balance < amount)
                return false;

            source.Balance -= amount;
            dest.Balance += amount;

            var internalTransaction = new AccountTransaction
            {
                BankingAccountId = source.Id,
                CounterpartyAccountId = dest.Id,
                Type = TransactionType.TransferBetweenAccounts,
                Status = TransactionStatus.Completed,
                Amount = amount,
                CreatedAt = DateTime.UtcNow,
                Description = "Internal account transfer"
            };

            _bankingDataContext.Transactions.Add(internalTransaction);

            ApplyDataBaseTransaction();

            return true;
        }
        public bool CloseAccount(Guid bankingAccountId)
        {
            var account = GetAccountIfOpen(bankingAccountId);

            if (account == null)
                return false;

            account.State = Libs.Banking.Contracts.Enums.AccountState.Closed;

            ApplyDataBaseTransaction();

            return true;
        }

        public BankingAccountContract? GetAccountData(Guid bankingAccountId)
        {
            return GetAccountIfOpen(bankingAccountId);
        }

        private void ResetOtherMainAccounts(Guid userId, Guid? excludeAccountId = null)
        {
            var mainAccounts = _bankingDataContext.BankAccounts
                .Where(a => a.UserId == userId && a.IsMain && (!excludeAccountId.HasValue || a.Id != excludeAccountId.Value))
                .ToList();

            foreach (var acc in mainAccounts)
                acc.IsMain = false;
        }

        public Guid CreateAccount(Guid userId, string name, BankAccountType type, decimal? creditLimit = null, bool isMain = false)
        {
            if (isMain)
                ResetOtherMainAccounts(userId);

            var account = new BankAccount
            {
                UserId = userId,
                Name = name,
                Type = type,
                State = AccountState.Open,
                Balance = 0m,
                CreditLimit = type == BankAccountType.Credit ? creditLimit ?? 0 : null,
                IsMain = isMain
            };

            _bankingDataContext.BankAccounts.Add(account);

            ApplyDataBaseTransaction();

            return account.Id;
        }

        public bool EditAccount(Guid bankingAccountId, string? name, BankAccountType? type, decimal? creditLimit, bool? isMain = null)
        {
            var account = GetAccountIfOpen(bankingAccountId);

            if (account == null)
                return false;

            if (!string.IsNullOrWhiteSpace(name))
                account.Name = name;

            if (type.HasValue)
                account.Type = (Libs.Banking.Contracts.Enums.BankAccountType)(int)type.Value;

            if (type == BankAccountType.Credit && creditLimit.HasValue)
                account.CreditLimit = creditLimit;

            ApplyMainAccountFlag(account, isMain);


            ApplyDataBaseTransaction();

            return true;
        }

        private void ApplyDataBaseTransaction()
        {
            using var transaction = _bankingDataContext.Database.BeginTransaction();
            try
            {
                _bankingDataContext.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private void ApplyMainAccountFlag(BankingAccountContract account, bool? isMain)
        {
            if (isMain.HasValue)
            {
                if (isMain.Value && !account.IsMain)
                    ResetOtherMainAccounts(account.UserId, account.Id);

                account.IsMain = isMain.Value;
            }
        }
    }
}
