using CDR_Bank.Banking.Services.Abstractions;
using CDR_Bank.DataAccess.Banking;

namespace CDR_Bank.Banking.Services
{
    internal class BankingService : IBankingService
    {
        public BankingService(BankingDataContext bankingDataContext)
        {
        }

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
