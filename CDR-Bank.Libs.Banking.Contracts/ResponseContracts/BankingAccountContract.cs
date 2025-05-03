using CDR_Bank.Libs.Banking.Contracts.Enums;

namespace CDR_Bank.Libs.Banking.Contracts.ResponseContracts
{
    public class BankingAccountContract
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string AccountNumber { get; set; }
        public bool IsMain { get; set; }
        public string Name { get; set; }
        public BankAccountType Type { get; set; }
        public AccountState State { get; set; }
        public decimal Balance { get; set; } = 0m;
        public decimal? CreditLimit { get; set; }
    }
}
