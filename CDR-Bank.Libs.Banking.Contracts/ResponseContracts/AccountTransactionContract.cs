using CDR_Bank.Libs.Banking.Contracts.Enums;

namespace CDR_Bank.Libs.Banking.Contracts.ResponseContracts
{
    public class AccountTransactionContract
    {
        public Guid Id { get; set; }
        public Guid BankingAccountId { get; set; }
        public Guid? CounterpartyAccountId { get; set; }
        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Description { get; set; }
    }
}
