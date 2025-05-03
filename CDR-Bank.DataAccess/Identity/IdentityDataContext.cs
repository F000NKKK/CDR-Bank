using Microsoft.EntityFrameworkCore;
using CDR_Bank.DataAccess.Identity.Entities;

namespace CDR_Bank.DataAccess.Identity
{
    /// <summary>
    /// Represents the database context for identity-related entities.
    /// </summary>
    public class IdentityDataContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityDataContext"/> class.
        /// </summary>
        /// <param name="options">The options to configure the context.</param>
        public IdentityDataContext(DbContextOptions<IdentityDataContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="User"/>.
        /// </summary>
        public DbSet<User> Users => Set<User>();

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="Role"/>.
        /// </summary>
        public DbSet<Role> Roles => Set<Role>();

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="UserRole"/>.
        /// </summary>
        public DbSet<UserRole> UserRoles => Set<UserRole>();

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="UserVerificationCode"/>.
        /// </summary>
        public DbSet<UserVerificationCode> VerificationCodes => Set<UserVerificationCode>();

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="UserSession"/>.
        /// </summary>
        public DbSet<UserSession> Sessions => Set<UserSession>();

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="LoginHistory"/>.
        /// </summary>
        public DbSet<LoginHistory> LoginHistories => Set<LoginHistory>();

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="UserSecuritySettings"/>.
        /// </summary>
        public DbSet<UserSecuritySettings> SecuritySettings => Set<UserSecuritySettings>();

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="UserContactInfo"/>.
        /// </summary>
        public DbSet<UserContactInfo> ContactInfos => Set<UserContactInfo>();

        /// <summary>
        /// Configures the entity mappings for the database context.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/> used to configure the entities.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure User entity
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(u => u.Id);
                e.HasIndex(u => u.Email).IsUnique();
                e.HasMany(u => u.Roles).WithOne(r => r.User).HasForeignKey(r => r.UserId);
                e.HasOne(u => u.ContactInfo).WithOne(c => c.User).HasForeignKey<UserContactInfo>(c => c.UserId);
            });

            // Configure Role entity
            modelBuilder.Entity<Role>(e =>
            {
                e.HasKey(r => r.Id);
                e.HasIndex(r => r.Name).IsUnique();
            });

            // Configure UserRole entity
            modelBuilder.Entity<UserRole>(e =>
            {
                e.HasKey(ur => new { ur.UserId, ur.RoleId });
            });

            // Configure UserVerificationCode entity
            modelBuilder.Entity<UserVerificationCode>(e =>
            {
                e.HasKey(vc => vc.Id);
                e.HasIndex(vc => new { vc.UserId, vc.Code });
            });

            // Configure UserSession entity
            modelBuilder.Entity<UserSession>(e =>
            {
                e.HasKey(s => s.Id);
                e.HasIndex(s => s.RefreshToken).IsUnique();
            });

            // Configure LoginHistory entity
            modelBuilder.Entity<LoginHistory>(e =>
            {
                e.HasKey(lh => lh.Id);
                e.HasIndex(lh => lh.UserId);
            });

            // Configure UserSecuritySettings entity
            modelBuilder.Entity<UserSecuritySettings>(e =>
            {
                e.HasKey(s => s.Id);
                e.HasIndex(s => s.UserId).IsUnique();
            });

            // Configure UserContactInfo entity
            modelBuilder.Entity<UserContactInfo>(e =>
            {
                e.HasKey(ci => ci.Id);
            });
        }
    }
}
