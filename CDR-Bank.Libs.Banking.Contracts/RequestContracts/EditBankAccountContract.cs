using CDR_Bank.Libs.Banking.Contracts.Enums;
using CDR_Bank.Libs.Banking.Contracts.RequestContracts.Abstractions;

namespace CDR_Bank.Libs.Banking.Contracts.RequestContracts
{
    /// <summary>
    /// Contract for editing an existing bank account.
    /// </summary>
    public class EditBankAccountContract : BaseBankingContract
    {
        /// <summary>
        /// New name for the account (optional).
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// New type for the account (optional).
        /// </summary>
        public BankAccountType? Type { get; set; }

        /// <summary>
        /// New credit limit for credit accounts (optional).
        /// </summary>
        public decimal? CreditLimit { get; set; }
        public bool? IsMain { get; set; }
    }
}
