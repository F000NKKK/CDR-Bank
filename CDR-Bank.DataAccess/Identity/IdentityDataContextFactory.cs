using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CDR_Bank.DataAccess.Identity
{
    public class IdentityDataContextFactory : IDesignTimeDbContextFactory<IdentityDataContext>
    {
        public IdentityDataContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<IdentityDataContext>();

            optionsBuilder.UseMySQL(configuration.GetConnectionString("IdentityDb"));

            return new IdentityDataContext(optionsBuilder.Options);
        }
    }
}
