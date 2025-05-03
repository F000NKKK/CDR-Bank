namespace CDR_Bank.DataAccess.Identity.Entities
{
    public class LoginHistory
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string IpAddress { get; set; } = string.Empty;
        public string Device { get; set; } = string.Empty;
        public bool IsSuccessful { get; set; }
    }
}
