using CDR_Bank.Libs.Banking.Contracts.Enums;

namespace CDR_Bank.Libs.Banking.Contracts.RequestContracts
{
    /// <summary>
    /// Contract for opening a new bank account.
    /// </summary>
    public class OpenBankAccountContract
    {
        /// <summary>
        /// User identifier who opens the account.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Name for the new account (e.g., "My Savings").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Type of the bank account (Debit or Credit).
        /// </summary>
        public BankAccountType Type { get; set; }

        /// <summary>
        /// Credit limit for credit accounts. Optional for debit accounts.
        /// </summary>
        public decimal? CreditLimit { get; set; }
    }
}
