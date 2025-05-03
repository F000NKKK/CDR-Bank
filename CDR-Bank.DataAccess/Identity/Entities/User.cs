namespace CDR_Bank.DataAccess.Identity.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // «один-к-одному»
        public UserContactInfo? ContactInfo { get; set; }
    }
}
