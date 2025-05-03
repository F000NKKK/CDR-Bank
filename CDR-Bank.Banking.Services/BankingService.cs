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
                .FirstOrDefault(a => a.AccountNumber == recipientTelephoneNumber && a.State == AccountState.Open);

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
    }
}
