using Autofac;
using Microsoft.Extensions.Configuration;

namespace CDR_Bank.IndentityServer.Services
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
            builder.RegisterType<UserService>()
                   .As<IUserService>()
                   .InstancePerLifetimeScope();
        }

        private void RegisterModules(ContainerBuilder builder)
        {
            var connectionString = _configuration.GetConnectionString("BankingDb");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string 'Banking' not found.");

            builder.RegisterModule(new CDR_Bank.DataAccess.Identity.IocModule(connectionString));
        }
    }
}
