using CDR_Bank.Libs.API.Constants;
using CDR_Bank.Libs.API.Filtres;
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

                options.AddSecurityDefinition(AuthorizationConstants.Schemes.Bearer, new OpenApiSecurityScheme
                {
                    Description = "Введите ваш токен с приставкой 'Bearer' и нажмите Authorize.",
                    Name = AuthorizationConstants.HeaderNames.Authorization,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = AuthorizationConstants.Schemes.Bearer,
                    BearerFormat = "JWT",
                    // Тут мы описываем, что токен должен быть типа Bearer
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = AuthorizationConstants.Schemes.Bearer
                        },
                        Scheme = AuthorizationConstants.Schemes.Bearer,
                        Name = AuthorizationConstants.HeaderNames.Authorization,
                        In = ParameterLocation.Header,
                    }] = new List<string>()
                });

                // Регистрируем фильтр для работы с Bearer токеном
                options.OperationFilter<AddBearerTokenOperationFilter>();
            });

            return services;
        }
    }
}
