using CDR_Bank.Libs.Hub.Contracts.Abstractions;

namespace CDR_Bank.Libs.Hub.Contracts
{
    public class BankingOperationContract : BaseBankingContract
    {
        /// <summary>
        /// Amount to replenish.
        /// </summary>
        public decimal Amount { get; set; }
    }
}
