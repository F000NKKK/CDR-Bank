namespace CDR_Bank.Libs.Identity.Contracts.RequestContracts.Abstractions;

public class UserData
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}