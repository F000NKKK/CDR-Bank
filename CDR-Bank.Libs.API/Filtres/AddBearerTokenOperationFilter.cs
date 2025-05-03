using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CDR_Bank.Libs.API.Filtres
{
    public class AddBearerTokenOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authorizationParam = operation.Parameters
                .FirstOrDefault(p => p.Name.Equals("Authorization", StringComparison.InvariantCultureIgnoreCase));

            if (authorizationParam != null)
            {
                authorizationParam.Description = "Введите ваш токен с приставкой 'Bearer' и нажмите Authorize.";
                authorizationParam.Example = new OpenApiString("Bearer <your-token>");
            }
        }
    }

}
