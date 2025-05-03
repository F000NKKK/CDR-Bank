using CDR_Bank.Libs.API;
using Microsoft.AspNetCore.Builder;

namespace CDR_Bank.Banking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var app = IdentityApiBuilder.Build(builder);

            app.Run();
        }
    }
}
