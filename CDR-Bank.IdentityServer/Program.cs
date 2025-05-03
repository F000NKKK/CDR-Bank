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

        var configuration = builder.Configuration;

        var identityConnectionString = configuration.GetConnectionString("IdentityDb")
            ?? throw new InvalidOperationException("Connection string 'IdentityDb' is not configured.");

        builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
        {
            containerBuilder.RegisterModule(new IocModule(identityConnectionString));
        });

        var app = IdentityApiBuilder.Build(builder);
        app.Run();
    }
}
