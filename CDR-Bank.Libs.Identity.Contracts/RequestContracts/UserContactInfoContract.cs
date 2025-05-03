namespace CDR_Bank.Libs.Identity.Contracts.RequestContracts.Abstractions;

public class UserContactInfoContract
{
    public string PhoneNumber { get; set; }
    public bool PhoneConfirmed { get; set; }

    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string PostalCode { get; set; }
}