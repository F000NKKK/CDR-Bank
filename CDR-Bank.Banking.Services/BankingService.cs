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
                .FirstOrDefault(a => a.Name == recipientTelephoneNumber && a.State == AccountState.Open);

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

        public Guid OpenDebitAccount(Guid userId, string name)
        {
            var account = new BankAccount
            {
                UserId = userId,
                Name = name,
                Type = BankAccountType.Debit,
                State = AccountState.Open,
                Balance = 0m
            };

            _bankingDataContext.BankAccounts.Add(account);
            _bankingDataContext.SaveChanges();

            return account.Id;
        }

        public Guid OpenCreditAccount(Guid userId, string name, decimal limit)
        {
            var account = new BankAccount
            {
                UserId = userId,
                Name = name,
                Type = BankAccountType.Credit,
                State = AccountState.Open,
                Balance = 0m,
                CreditLimit = limit
            };

            _bankingDataContext.BankAccounts.Add(account);
            _bankingDataContext.SaveChanges();

            return account.Id;
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

        public bool EditAccount(Guid bankingAccountId, string? name, BankAccountType? type, decimal? creditLimit)
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

            _bankingDataContext.SaveChanges();
            return true;
        }

        public BankAccount? GetAccountData(Guid bankingAccountId)
        {
            return _bankingDataContext.BankAccounts
                .FirstOrDefault(a => a.Id == bankingAccountId);
        }
    }
}
