using CDR_Bank.DataAccess.Banking.Entities;
using CDR_Bank.DataAccess.Banking.Enums;

namespace CDR_Bank.Banking.Services.Abstractions
{
    public interface IBankingService
    {
        bool CloseAccount(Guid bankingAccountId);
        Guid OpenDebitAccount(Guid userId, string name, bool isMain = false);
        Guid OpenCreditAccount(Guid userId, string name, decimal limit, bool isMain = false);
        bool EditAccount(Guid bankingAccountId, string? name, BankAccountType? type, decimal? creditLimit, bool? isMain = null);
        BankAccount? GetAccountData(Guid bankingAccountId);
        bool InternalTransfer(Guid sourceAccountId, Guid destinationAccountId, decimal amount);
        void Replenish(Guid bankingAccount, decimal amount);
        bool Transfer(Guid bankingAccount, string recipientTelephoneNumber, decimal amount);
        bool Withdraw(Guid bankingAccount, decimal amount);
    }
}
