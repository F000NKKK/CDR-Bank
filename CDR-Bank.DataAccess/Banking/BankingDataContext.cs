using CDR_Bank.DataAccess.Banking.Entities;
using Microsoft.EntityFrameworkCore;

namespace CDR_Bank.DataAccess.Banking
{
    public class BankingDataContext : DbContext
    {
        public BankingDataContext(DbContextOptions<BankingDataContext> options) : base(options)
        {
        }

        public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
        public DbSet<AccountTransaction> Transactions => Set<AccountTransaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>(e =>
            {
                e.HasKey(a => a.Id);

                e.HasIndex(a => a.AccountNumber).IsUnique();
                e.HasIndex(a => a.UserId);
                e.HasIndex(a => a.IsMain );

                e.Property(a => a.Type).HasConversion<string>();
                e.Property(a => a.State).HasConversion<string>();

                e.HasMany(a => a.Transactions)
                    .WithOne(t => t.BankingAccount)
                    .HasForeignKey(t => t.BankingAccountId);
            });

            modelBuilder.Entity<AccountTransaction>(e =>
            {
                e.HasKey(t => t.Id);
                e.HasIndex(t => t.BankingAccountId);

                e.Property(t => t.Type).HasConversion<string>();
                e.Property(t => t.Status).HasConversion<string>();
            });
        }
    }
}
