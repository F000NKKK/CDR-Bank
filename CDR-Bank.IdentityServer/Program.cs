using Autofac;
using Autofac.Extensions.DependencyInjection;
using CDR_Bank.DataAccess.Identity;
using CDR_Bank.Libs.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

        builder.Host.ConfigureContainer<ContainerBuilder>((context, containerBuilder) =>
        {
            containerBuilder.RegisterModule(new CDR_Bank.IndentityServer.Services.IocModule(builder.Configuration));
        });

        var app = IdentityApiBuilder.Build(builder);
        app.Run();
    }
}
