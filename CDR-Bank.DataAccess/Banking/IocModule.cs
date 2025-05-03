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
            builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<BankingDataContext>();

                // Обновляем для новой версии MySql.EntityFrameworkCore
                optionsBuilder.UseMySQL(
                    _connectionString,
                    mySqlOptions =>
                    {
                        mySqlOptions.EnableRetryOnFailure();
                    });

                // Регистрация контекста в DI контейнере
                return new BankingDataContext(optionsBuilder.Options);
            })
            .AsSelf() // Регистрация самого контекста
            .SingleInstance(); // Синглтон, т.к. DataContext обычно используется в скоупе
        }
    }
}
