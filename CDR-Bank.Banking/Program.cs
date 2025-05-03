using Autofac;
using Autofac.Extensions.DependencyInjection;
using CDR_Bank.Libs.API;

namespace CDR_Bank.Banking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            builder.Host.ConfigureContainer<ContainerBuilder>((context, containerBuilder) =>
            {
                containerBuilder.RegisterModule(new CDR_Bank.Banking.Services.IocModule(builder.Configuration));
            });

            var app = ApiBuilder.Build(builder);

            app.Run();
        }
    }
}
