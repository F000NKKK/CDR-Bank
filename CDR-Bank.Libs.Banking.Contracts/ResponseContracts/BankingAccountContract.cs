using CDR_Bank.Libs.Banking.Contracts.Enums;
using CDR_Bank.Libs.Banking.Contracts.Enums;

namespace CDR_Bank.Libs.Banking.Contracts.ResponseContracts
{
    public class BankingAccountContract
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; } // from Identity
        public string AccountNumber { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public BankAccountType Type { get; set; }
        public AccountState State { get; set; } = AccountState.Open;
        public decimal Balance { get; set; } = 0m;
        public decimal? CreditLimit { get; set; }
    }
}
