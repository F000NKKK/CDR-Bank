
namespace CDR_Bank.Hub.Services.Abstractions
{
    public interface IBankingService
    {
        void Replenish(Guid bankingAccount, decimal amount);
        bool Transfer(Guid bankingAccount, string recipientTelephoneNumber, decimal amount);
        bool Withdraw(Guid bankingAccount, decimal amount);
    }
}
