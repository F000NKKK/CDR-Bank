using CDR_Bank.Libs.API.Contracts;
using CDR_Bank.Libs.API.Contracts.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CDR_Bank.Libs.API.Abstractions
{
    [ApiController]
    [Produces("application/json")]  
    public abstract class AController : ControllerBase
    {
        protected ActionResult<IResponse> OkResponse<T>(T data, string? message = null)
        {
            var response = new ApiResponse<T>(data, message)
            {
                StatusCode = StatusCodes.Status200OK
            };
            return base.Ok(response);
        }

        protected ActionResult<IResponse> CreatedResponse<T>(string uri, T data, string? message = null)
        {
            var response = new ApiResponse<T>(data, message)
            {
                StatusCode = StatusCodes.Status201Created
            };
            return base.Created(uri, response);
        }

        protected ActionResult<IResponse> ErrorResponse(
            IEnumerable<string> errors,
            int statusCode = StatusCodes.Status400BadRequest,
            string? message = null)
        {
            var response = new ErrorResponse();
            response.Status = statusCode;
            response.Title = message;
            errors.ToList().ForEach(s => response.AdditionalData.Add("ErrorMessage", s));
            return base.StatusCode(statusCode, response);
        }

        protected ActionResult<IResponse> ErrorResponse(string error, int statusCode = StatusCodes.Status400BadRequest, string? message = null)
            => ErrorResponse(new[] { error }, statusCode, message);
    }
}
