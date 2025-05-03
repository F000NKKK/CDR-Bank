using CDR_Bank.Libs.Banking.Contracts.RequestContracts.Abstractions;

namespace CDR_Bank.Libs.Banking.Contracts.RequestContracts
{
    public class BankingOperationContract : BaseBankingContract
    {
        /// <summary>
        /// Amount to replenish.
        /// </summary>
        public decimal Amount { get; set; }
    }
}
