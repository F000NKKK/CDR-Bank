namespace CDR_Bank.Libs.Hub.Contracts
{
    public class BankingTransferContract : BankingOperationContract
    {
        /// <summary>
        /// Recipient's telephone number.
        /// </summary>
        public string RecipientTelephoneNumber { get; set; }
    }
}
