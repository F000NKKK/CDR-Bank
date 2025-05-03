using Autofac;
using Microsoft.EntityFrameworkCore;

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

                context.Database.Migrate();

                return context;
            })
            .AsSelf()
            .As<DbContext>()
            .SingleInstance();
        }
    }
}
