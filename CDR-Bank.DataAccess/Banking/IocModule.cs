using Autofac;
using Microsoft.EntityFrameworkCore;

namespace CDR_Bank.DataAccess.Banking
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
            builder.Register<BankingDataContext>(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<BankingDataContext>();

                optionsBuilder.UseMySQL(
                    _connectionString,
                    mySqlOptions =>
                    {
                        mySqlOptions.EnableRetryOnFailure();
                    });

                var context = new BankingDataContext(optionsBuilder.Options);

                context.Database.Migrate();

                return context;
            })
            .AsSelf()
            .As<DbContext>()
            .SingleInstance();
        }

    }
}
