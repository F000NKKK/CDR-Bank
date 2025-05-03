using CDR_Bank.Libs.API.Constants;
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
                    Title = "API",
                    Version = "v1",
                    Description = "API",
                });

                options.AddSecurityDefinition("ApiKeyAuth", new OpenApiSecurityScheme
                {
                    Description = "Введите ваш токен и нажмите Authorize.",
                    Name = AuthorizationConstants.HeaderNames.Authorization,
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
                        Name = AuthorizationConstants.HeaderNames.Authorization,
                        In = ParameterLocation.Header,
                    }] = new List<string>()
                });
            });

            return services;
        }
    }
}
