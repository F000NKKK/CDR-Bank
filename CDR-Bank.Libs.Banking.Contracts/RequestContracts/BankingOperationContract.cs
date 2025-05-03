using CDR_Bank.Libs.Hub.Contracts.Request.Abstractions;

namespace CDR_Bank.Libs.Hub.Contracts.Request
{
    public class BankingOperationContract : BaseBankingContract
    {
        /// <summary>
        /// Amount to replenish.
        /// </summary>
        public decimal Amount { get; set; }
    }
}
