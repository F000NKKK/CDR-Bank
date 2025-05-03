using Autofac;
using CDR_Bank.Banking.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CDR_Bank.Banking.Services
{
    public class IocModule : Module
    {
        private readonly IConfiguration _configuration;

        public IocModule(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterConfiguration(builder);
            RegisterServices(builder);
            RegisterModules(builder);
        }

        private void RegisterConfiguration(ContainerBuilder builder)
        {
            // Здесь можно регистрировать IOptions, IConfigurationSection и пр.
            builder.RegisterInstance(_configuration).As<IConfiguration>().SingleInstance();
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<BankingService>()
                   .As<IBankingService>()
                   .InstancePerLifetimeScope();
        }

        private void RegisterModules(ContainerBuilder builder)
        {
            var connectionString = _configuration.GetConnectionString("BankingDb");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string 'Banking' not found.");

            builder.RegisterModule(new CDR_Bank.DataAccess.Banking.IocModule(connectionString));
        }
    }
}
