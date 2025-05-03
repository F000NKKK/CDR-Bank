using CDR_Bank.Banking.Services.Abstractions;
using CDR_Bank.DataAccess.Banking;
using CDR_Bank.DataAccess.Banking.Entities;
using CDR_Bank.DataAccess.Banking.Enums;

namespace CDR_Bank.Banking.Services
{
    internal class BankingService : IBankingService
    {
        private readonly BankingDataContext _bankingDataContext;

        public BankingService(BankingDataContext bankingDataContext)
        {
            _bankingDataContext = bankingDataContext;
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
            _bankingDataContext.SaveChanges();
        }

        public bool Transfer(Guid bankingAccountId, string recipientTelephoneNumber, decimal amount)
        {
            var senderAccount = _bankingDataContext.BankAccounts
                .FirstOrDefault(a => a.Id == bankingAccountId && a.State == AccountState.Open);

            if (senderAccount == null || senderAccount.Balance < amount)
                return false;

            var recipientAccount = _bankingDataContext.BankAccounts
                .FirstOrDefault(a => a.TelephoneNumber == recipientTelephoneNumber && a.State == AccountState.Open && a.IsMain);

            if (recipientAccount == null)
                return false;

            senderAccount.Balance -= amount;
            recipientAccount.Balance += amount;

            var transaction = new AccountTransaction
            {
                BankingAccountId = bankingAccountId,
                CounterpartyAccountId = recipientAccount.Id,
                Type = TransactionType.TransferToUser,
                Status = TransactionStatus.Completed,
                Amount = amount,
                CreatedAt = DateTime.UtcNow,
                Description = "Transfer to another user"
            };

            _bankingDataContext.Transactions.Add(transaction);
            _bankingDataContext.SaveChanges();

            return true;
        }

        public bool Withdraw(Guid bankingAccountId, decimal amount)
        {
            var account = _bankingDataContext.BankAccounts
                .FirstOrDefault(a => a.Id == bankingAccountId && a.State == AccountState.Open);

            if (account == null || account.Balance < amount)
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
            _bankingDataContext.SaveChanges();

            return true;
        }

        public bool InternalTransfer(Guid sourceAccountId, Guid destinationAccountId, decimal amount)
        {
            var source = _bankingDataContext.BankAccounts
                .FirstOrDefault(a => a.Id == sourceAccountId && a.State == AccountState.Open);
            var dest = _bankingDataContext.BankAccounts
                .FirstOrDefault(a => a.Id == destinationAccountId && a.State == AccountState.Open);

            if (source == null || dest == null || source.Balance < amount)
                return false;

            source.Balance -= amount;
            dest.Balance += amount;

            var transaction = new AccountTransaction
            {
                BankingAccountId = source.Id,
                CounterpartyAccountId = dest.Id,
                Type = TransactionType.TransferBetweenAccounts,
                Status = TransactionStatus.Completed,
                Amount = amount,
                CreatedAt = DateTime.UtcNow,
                Description = "Internal account transfer"
            };

            _bankingDataContext.Transactions.Add(transaction);
            _bankingDataContext.SaveChanges();

            return true;
        }
        public bool CloseAccount(Guid bankingAccountId)
        {
            var account = _bankingDataContext.BankAccounts
                .FirstOrDefault(a => a.Id == bankingAccountId && a.State == AccountState.Open);

            if (account == null)
                return false;

            account.State = AccountState.Closed;
            _bankingDataContext.SaveChanges();
            return true;
        }

        public BankAccount? GetAccountData(Guid bankingAccountId)
        {
            return _bankingDataContext.BankAccounts
                .FirstOrDefault(a => a.Id == bankingAccountId);
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
            _bankingDataContext.SaveChanges();

            return account.Id;
        }

        private void ApplyMainAccountFlag(BankAccount account, bool? isMain)
        {
            if (isMain.HasValue)
            {
                if (isMain.Value && !account.IsMain)
                    ResetOtherMainAccounts(account.UserId, account.Id);

                account.IsMain = isMain.Value;
            }
        }

        public bool EditAccount(Guid bankingAccountId, string? name, BankAccountType? type, decimal? creditLimit, bool? isMain = null)
        {
            var account = _bankingDataContext.BankAccounts
                .FirstOrDefault(a => a.Id == bankingAccountId);

            if (account == null)
                return false;

            if (!string.IsNullOrWhiteSpace(name))
                account.Name = name;

            if (type.HasValue)
                account.Type = type.Value;

            if (type == BankAccountType.Credit && creditLimit.HasValue)
                account.CreditLimit = creditLimit;

            ApplyMainAccountFlag(account, isMain);

            _bankingDataContext.SaveChanges();
            return true;
        }
    }
}
