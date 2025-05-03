using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CDR_Bank.Libs.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddControllers();
            return services;
        }
        public static IServiceCollection AddApiSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TimeTracking API",
                    Version = "v1",
                    Description = "API для отслеживания времени",
                });

                options.AddSecurityDefinition("ApiKeyAuth", new OpenApiSecurityScheme
                {
                    Description = "Введите ваш токен и нажмите Authorize.\n" +
                                  "Токен будет помещён в заголовок: x-vp-api-key",
                    Name = "x-vp-api-key",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "ApiKeyAuth"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKeyAuth"
                        },
                        Scheme = "ApiKeyAuth",
                        Name = "x-vp-api-key",
                        In = ParameterLocation.Header,
                    }] = new List<string>()
                });
            });

            return services;
        }
    }
}
