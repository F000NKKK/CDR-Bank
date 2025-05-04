using CDR_Bank.DataAccess.Banking.Entities;

namespace CDR_Bank.Hub.Services.Abstractions
{
    public interface IAccountValidationService
    {
        /// <summary>
        /// Verifies that the account exists and is open.
        /// </summary>
        BankAccount? GetAccountIfOpen(Guid bankingAccountId);

        /// <summary>
        /// Is it possible to make a transfer from this account to the specified amount?
        /// </summary>
        bool CanTransfer(BankAccount senderAccount, decimal amount);

        /// <summary>
        /// Is it possible to withdraw funds from this account for the specified amount?
        /// </summary>
        bool CanWithdraw(BankAccount account, decimal amount);

        /// <summary>
        /// Whether the user has a credit account with a balance below 20,000.
        /// </summary>
        bool HasNegativeCreditBalance(Guid userId);
        bool CanReplenish(BankAccount account, decimal amount);
    }
}
