using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CDR_Bank.DataAccess.Banking
{
    public class BankingDataContextFactory : IDesignTimeDbContextFactory<BankingDataContext>
    {
        public BankingDataContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<BankingDataContext>();

            optionsBuilder.UseMySQL(configuration.GetConnectionString("BankingDb"));

            return new BankingDataContext(optionsBuilder.Options);
        }
    }
}
