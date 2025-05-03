namespace CDR_Bank.DataAccess.Identity.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;

        public ICollection<UserRole> Users { get; set; } = new List<UserRole>();
    }
}
