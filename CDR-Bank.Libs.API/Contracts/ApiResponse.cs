using CDR_Bank.Libs.API.Contracts.Abstractions;
using Microsoft.AspNetCore.Http;

namespace CDR_Bank.Libs.API.Contracts
{
    /// <summary>
    /// A single HTTP response model.
    /// </summary>
    /// <typeparam name="T">Payload type.</typeparam>
    public class ApiResponse<T> : IResponse
    {
        /// <summary>The response body (data).</summary>
        public T? Data { get; set; }

        /// <summary>A message for the user.</summary>
        public string? Message { get; set; }

        /// <summary>HTTP status code (duplicates the response status code).</summary>
        public int StatusCode { get; set; }

        public ApiResponse()
        {
        }

        public ApiResponse(T data, string? message = null) : this(data, StatusCodes.Status200OK, message)
        {
        }

        public ApiResponse(int statusCode, string? message = null) : this(default, statusCode, message)
        {
        }

        public ApiResponse(T? data, int statusCode, string? message = null)
        {
            Data = data;
            StatusCode = statusCode;
            Message = message;
        }
    }
}
