using CDR_Bank.Libs.API.Extensions;
using CDR_Bank.Libs.API.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace CDR_Bank.Libs.API
{
    public static class IdentityApiBuilder
    {
        public static WebApplication Build(WebApplicationBuilder builder)
        {
            builder.Services
               .AddApiCore(builder.Configuration)
               .AddApiSwagger();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseApiCore()
               .UseApiSwagger();

            app.UseHttpsRedirection();
            app.MapControllers();

            return app;
        }
    }
}
