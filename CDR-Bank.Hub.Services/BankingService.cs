using CDR_Bank.Hub.Services.Abstractions;

namespace CDR_Bank.Hub.Services
{
    internal class BankingService : IBankingService
    {
        public void Replenish(Guid bankingAccount, decimal amount)
        {
            throw new NotImplementedException();
        }

        public bool Transfer(Guid bankingAccount, string recipientTelephoneNumber, decimal amount)
        {
            throw new NotImplementedException();
        }

        public bool Withdraw(Guid bankingAccount, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
