namespace CDR_Bank.Libs.Identity.Contracts.RequestContracts.Abstractions;

public class PasswordChange
{ 
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}