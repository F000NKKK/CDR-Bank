namespace CDR_Bank.Libs.Banking.Contracts.RequestContracts
{
    public class BankingTransferContract : BankingOperationContract
    {
        /// <summary>
        /// Recipient's telephone number.
        /// </summary>
        public string RecipientTelephoneNumber { get; set; }
    }
}
