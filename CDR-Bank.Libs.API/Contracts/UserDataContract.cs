namespace CDR_Bank.Libs.API.Contracts
{
    public class UserDataContract
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
