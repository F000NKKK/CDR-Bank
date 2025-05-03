namespace CDR_Bank.Libs.Banking.Contracts.RequestContracts
{
    /// <summary>
    /// Contract for internal transfer between two accounts of the same client.
    /// </summary>
    public class InternalTransferContract
    {
        /// <summary>
        /// Source account identifier.
        /// </summary>
        public Guid SourceAccountId { get; set; }

        /// <summary>
        /// Destination account identifier.
        /// </summary>
        public Guid DestinationAccountId { get; set; }

        /// <summary>
        /// Amount to transfer.
        /// </summary>
        public decimal Amount { get; set; }
    }

}
