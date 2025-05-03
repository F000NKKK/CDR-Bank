using Autofac;
using Microsoft.EntityFrameworkCore;
using System;

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
                optionsBuilder.UseMySql(
                    _connectionString,
                    new MySqlServerVersion(new Version(8, 0, 36)),
                    mySqlOptions =>
                    {
                        mySqlOptions.EnableRetryOnFailure();
                    });

                return new IdentityDataContext(optionsBuilder.Options);
            })
            .AsSelf()
            .InstancePerLifetimeScope();
        }
    }
}
