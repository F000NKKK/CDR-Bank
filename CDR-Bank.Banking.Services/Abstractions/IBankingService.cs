using CDR_Bank.DataAccess.Banking.Entities;
using CDR_Bank.DataAccess.Banking.Enums;
using CDR_Bank.DataAccess.Models;
using CDR_Bank.Libs.Hub.Contracts.RequestContracts;

namespace CDR_Bank.Banking.Services.Abstractions
{
    public interface IBankingService
    {
        bool CloseAccount(Guid bankingAccountId);
        Guid CreateAccount(Guid userId, string name, BankAccountType type, decimal? creditLimit = null, bool isMain = false);
        bool EditAccount(Guid bankingAccountId, string? name, BankAccountType? type, decimal? creditLimit, bool? isMain = null);
        BankAccount? GetAccountData(Guid bankingAccountId);
        bool InternalTransfer(Guid sourceAccountId, Guid destinationAccountId, decimal amount);
        void Replenish(Guid bankingAccount, decimal amount);
        bool Transfer(Guid bankingAccount, string recipientTelephoneNumber, decimal amount);
        bool Withdraw(Guid bankingAccount, decimal amount);
        PagedResult<AccountTransaction> GetTransactions(Guid userId, TransactionFilterContract filter);
    }
}
