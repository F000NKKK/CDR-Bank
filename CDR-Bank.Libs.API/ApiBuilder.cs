using CDR_Bank.Libs.API.Extensions;
using Microsoft.AspNetCore.Builder;

namespace CDR_Bank.Libs.API
{
    public static class ApiBuilder
    {
        public static WebApplication Build(WebApplicationBuilder builder)
        {
            builder.Services
                   .AddApiCore(builder.Configuration)
                   .AddApiSwagger();

            var app = builder.Build();
            
            app.UseApiCore()
               .UseApiSwagger();

            app.UseHttpsRedirection();
            app.MapControllers();

            return app;
        }
    }
}
