namespace CDR_Bank.DataAccess.Identity.Entities
{
    public class UserVerificationCode
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        public string Code { get; set; } = default!;
        public DateTime Expiration { get; set; }
        public VerificationCodeType CodeType { get; set; } // e.g. EmailConfirmation, PasswordReset
        public bool IsUsed { get; set; }
    }

    public enum VerificationCodeType
    {
        EmailConfirmation,
        PasswordReset,
        PhoneConfirmation,
        TwoFactorAuth
    }
}
