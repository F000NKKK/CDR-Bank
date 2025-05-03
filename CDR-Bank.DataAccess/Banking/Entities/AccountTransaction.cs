using CDR_Bank.DataAccess.Banking.Enums;

namespace CDR_Bank.DataAccess.Banking.Entities
{
    public class AccountTransaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid BankingAccountId { get; set; }
        public Guid? CounterpartyAccountId { get; set; } // if this is a translation
        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }

        public virtual BankAccount BankingAccount { get; set; } = null!;
    }
}
