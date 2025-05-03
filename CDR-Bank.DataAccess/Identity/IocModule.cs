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

                // Обновляем для новой версии MySql.EntityFrameworkCore
                optionsBuilder.UseMySQL(
                    _connectionString,
                    mySqlOptions =>
                    {
                        mySqlOptions.EnableRetryOnFailure();
                    });

                // Регистрация контекста в DI контейнере
                return new IdentityDataContext(optionsBuilder.Options);
            })
            .AsSelf()
            .InstancePerLifetimeScope();
        }
    }
}
