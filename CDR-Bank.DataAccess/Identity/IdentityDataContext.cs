using Microsoft.EntityFrameworkCore;
using CDR_Bank.DataAccess.Identity.Entities;
using System.Data;

namespace CDR_Bank.DataAccess.Identity
{
    public class IdentityDataContext : DbContext
    {
        public IdentityDataContext(DbContextOptions<IdentityDataContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<UserContactInfo> ContactInfos => Set<UserContactInfo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User ←→ ContactInfo (1:1)
            modelBuilder.Entity<User>()
                .HasOne(u => u.ContactInfo)
                .WithOne(ci => ci.User)
                .HasForeignKey<UserContactInfo>(ci => ci.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ограничения и индексы
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
            .IsUnique();
        }
    }
}
