using CDR_Bank.Libs.API.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace CDR_Bank.Libs.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static WebApplication UseApiCore(this WebApplication app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<AuthorizationMiddleware>();
            return app;
        }
        public static WebApplication UseApiSwagger(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "TimeTracking API v1");
                    options.RoutePrefix = "swagger";
                });
            }
            return app;
        }
    }
}
