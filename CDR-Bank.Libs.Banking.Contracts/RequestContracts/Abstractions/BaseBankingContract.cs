namespace CDR_Bank.Libs.Banking.Contracts.RequestContracts.Abstractions
{
    public abstract class BaseBankingContract
    {  
        /// <summary>
       /// Unique identifier of the banking account.
       /// </summary>
        public Guid BankingAccount { get; set; }
    }
}
