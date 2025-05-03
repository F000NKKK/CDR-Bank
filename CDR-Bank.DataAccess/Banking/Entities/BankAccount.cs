using CDR_Bank.DataAccess.Banking.Enums;

namespace CDR_Bank.DataAccess.Banking.Entities
{
    public class BankAccount
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; } // from Identity
        public string AccountNumber { get; set; } = Guid.NewGuid().ToString();
        public bool IsMain { get; set; }
        public string Name { get; set; } = string.Empty;
        public BankAccountType Type { get; set; }
        public AccountState State { get; set; } = AccountState.Open;
        public decimal Balance { get; set; } = 0m;
        public decimal? CreditLimit { get; set; }

        public ICollection<AccountTransaction> Transactions { get; set; } = new List<AccountTransaction>();
    }
}
