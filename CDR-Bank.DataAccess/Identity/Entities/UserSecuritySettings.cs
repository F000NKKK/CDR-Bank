namespace CDR_Bank.DataAccess.Identity.Entities
{
    public class UserSecuritySettings
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        public bool TwoFactorEnabled { get; set; }
        public bool EmailNotificationsEnabled { get; set; }
        public bool SmsNotificationsEnabled { get; set; }

        public string? BackupEmail { get; set; }
        public string? BackupPhoneNumber { get; set; }
    }
}
