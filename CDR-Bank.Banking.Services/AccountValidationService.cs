using CDR_Bank.DataAccess.Banking;
using CDR_Bank.DataAccess.Banking.Entities;
using CDR_Bank.DataAccess.Banking.Enums;
using CDR_Bank.Hub.Services.Abstractions;

namespace CDR_Bank.Banking.Services
{
    internal class AccountValidationService : IAccountValidationService
    {
        private const decimal NEGATIVE_CREDIT_THRESHOLD = -20_000m;
        private const decimal MAX_WITHDRAW_AMOUNT = 30_000m;

        private readonly BankingDataContext _bankingDataContext;

        public AccountValidationService(BankingDataContext bankingDataContext)
        {
            _bankingDataContext = bankingDataContext;
        }

        public BankAccount? GetAccountIfOpen(Guid bankingAccountId)
        {
            return _bankingDataContext.BankAccounts
                .FirstOrDefault(a => a.Id == bankingAccountId && a.State == AccountState.Open);
        }

        public bool CanTransfer(BankAccount senderAccount, decimal amount)
        {
            if (senderAccount == null)
                return false;

            decimal maxAvailable = senderAccount.Type == BankAccountType.Credit
                ? senderAccount.Balance + (senderAccount.CreditLimit ?? 0)
                : senderAccount.Balance;

            if (maxAvailable < amount)
                return false;

            if (senderAccount.Type == BankAccountType.Debit && HasNegativeCreditBalance(senderAccount.UserId))
                return false;

            return true;
        }

        public bool CanWithdraw(BankAccount account, decimal amount)
        {
            if (account == null)
                return false;

            if (amount > MAX_WITHDRAW_AMOUNT)
                return false;

            decimal maxAvailable = account.Type == BankAccountType.Credit
                ? account.Balance + (account.CreditLimit ?? 0)
                : account.Balance;

            if (maxAvailable < amount)
                return false;

            if (account.Type == BankAccountType.Debit && HasNegativeCreditBalance(account.UserId))
                return false;

            return true;
        }

        public bool HasNegativeCreditBalance(Guid userId)
        {
            return _bankingDataContext.BankAccounts
                .Any(a => a.UserId == userId
                          && a.Type == BankAccountType.Credit
                          && a.Balance <= NEGATIVE_CREDIT_THRESHOLD);
        }
    }
}
