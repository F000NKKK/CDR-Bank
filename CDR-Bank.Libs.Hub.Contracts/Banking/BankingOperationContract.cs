using CDR_Bank.Libs.Hub.Contracts.Banking.Abstractions;

namespace CDR_Bank.Libs.Hub.Contracts.Banking
{
    public class BankingOperationContract : BaseBankingContract
    {
        /// <summary>
        /// Amount to replenish.
        /// </summary>
        public decimal Amount { get; set; }
    }
}
