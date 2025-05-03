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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.ContactInfo)
                .WithOne(ci => ci.User)
                .HasForeignKey<UserContactInfo>(ci => ci.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .Navigation(u => u.ContactInfo)
                .AutoInclude();
        }

    }
}
