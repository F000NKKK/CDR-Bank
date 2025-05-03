using Autofac;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient; // для MySqlException

namespace CDR_Bank.DataAccess.Identity
{
    public class IocModule : Module
    {
        private readonly string _connectionString;

        public IocModule(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<IdentityDataContext>();
                optionsBuilder.UseMySQL(
                    _connectionString,
                    mySqlOptions =>
                    {
                        mySqlOptions.EnableRetryOnFailure();
                    });

                var context = new IdentityDataContext(optionsBuilder.Options);

                try
                {
                    context.Database.Migrate();
                }
                catch (MySqlException ex) when (ex.Message.Contains("Parameter '@result'"))
                {
                    Console.WriteLine("Warning: migrations could not capture LOCK, we skip: " + ex.Message);
                }

                return context;
            })
            .AsSelf()
            .SingleInstance();
        }
    }
}
