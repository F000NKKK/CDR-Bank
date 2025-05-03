
using CDR_Bank.Libs.API;

namespace CDR_Bank.Hub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var app = ApiBuilder.Build(builder);

            app.Run();
        }
    }
}
