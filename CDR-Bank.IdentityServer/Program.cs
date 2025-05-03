using CDR_Bank.Libs.API;
using Microsoft.AspNetCore.Builder;

namespace CDR_Bank.Hub
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
