namespace CDR_Bank.Libs.Banking.Contracts.RequestContracts
{
    public class TransactionFilterContract
    {
        public Guid? BankingAccountId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
