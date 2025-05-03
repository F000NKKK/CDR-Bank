using CDR_Bank.DataAccess.Banking.Entities;
using CDR_Bank.DataAccess.Banking.Enums;

namespace CDR_Bank.Banking.Services.Abstractions
{
    public interface IBankingService
    {
        bool CloseAccount(Guid bankingAccountId);
        bool EditAccount(Guid bankingAccountId, string? name, BankAccountType? type, decimal? creditLimit);
        BankAccount? GetAccountData(Guid bankingAccountId);
        bool InternalTransfer(Guid sourceAccountId, Guid destinationAccountId, decimal amount);
        Guid OpenCreditAccount(Guid userId, string name, decimal limit);
        Guid OpenDebitAccount(Guid userId, string name);
        void Replenish(Guid bankingAccount, decimal amount);
        bool Transfer(Guid bankingAccount, string recipientTelephoneNumber, decimal amount);
        bool Withdraw(Guid bankingAccount, decimal amount);
    }
}
