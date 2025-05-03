using Microsoft.EntityFrameworkCore;
using CDR_Bank.DataAccess.Identity.Entities;

namespace CDR_Bank.DataAccess.Identity
{
    public class IdentityDataContext : DbContext
    {
        public IdentityDataContext(DbContextOptions<IdentityDataContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<UserVerificationCode> VerificationCodes => Set<UserVerificationCode>();
        public DbSet<UserSession> Sessions => Set<UserSession>();
        public DbSet<LoginHistory> LoginHistories => Set<LoginHistory>();
        public DbSet<UserSecuritySettings> SecuritySettings => Set<UserSecuritySettings>();
        public DbSet<UserContactInfo> ContactInfos => Set<UserContactInfo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(u => u.Id);
                e.HasIndex(u => u.Email).IsUnique();
                e.HasMany(u => u.Roles).WithOne(r => r.User).HasForeignKey(r => r.UserId);
                e.HasOne(u => u.ContactInfo).WithOne(c => c.User).HasForeignKey<UserContactInfo>(c => c.UserId);
            });

            modelBuilder.Entity<Role>(e =>
            {
                e.HasKey(r => r.Id);
                e.HasIndex(r => r.Name).IsUnique();
            });

            modelBuilder.Entity<UserRole>(e =>
            {
                e.HasKey(ur => new { ur.UserId, ur.RoleId });
            });

            modelBuilder.Entity<UserVerificationCode>(e =>
            {
                e.HasKey(vc => vc.Id);
                e.HasIndex(vc => new { vc.UserId, vc.Code });
            });

            modelBuilder.Entity<UserSession>(e =>
            {
                e.HasKey(s => s.Id);
                e.HasIndex(s => s.RefreshToken).IsUnique();
            });

            modelBuilder.Entity<LoginHistory>(e =>
            {
                e.HasKey(lh => lh.Id);
                e.HasIndex(lh => lh.UserId);
            });

            modelBuilder.Entity<UserSecuritySettings>(e =>
            {
                e.HasKey(s => s.Id);
                e.HasIndex(s => s.UserId).IsUnique();
            });

            modelBuilder.Entity<UserContactInfo>(e =>
            {
                e.HasKey(ci => ci.Id);
            });
        }
    }
}
