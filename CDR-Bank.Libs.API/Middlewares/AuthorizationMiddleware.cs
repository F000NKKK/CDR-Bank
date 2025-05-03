using CDR_Bank.Libs.API.Constants;
using CDR_Bank.Libs.API.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace CDR_Bank.Libs.API.Middlewares
{
    /// <summary>
    /// Middleware for global external API Bearer token authorization.
    /// </summary>
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthorizationMiddleware> _logger;
        private readonly string _authorizationEndpoint;
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthorizationMiddleware(
            RequestDelegate next,
            IConfiguration config,
            ILogger<AuthorizationMiddleware> logger,
            IHttpClientFactory httpClientFactory)
        {
            _next = next;
            _logger = logger;
            _httpClientFactory = httpClientFactory;

            _authorizationEndpoint = config[AuthorizationConstants.ConfigKeys.AuthorizationEndpoint]
                ?? throw new InvalidOperationException($"{AuthorizationConstants.ConfigKeys.AuthorizationEndpoint} is not configured");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;

            if (string.IsNullOrWhiteSpace(path)
                || path.StartsWith(AuthorizationConstants.PathsToBypass.Swagger, StringComparison.OrdinalIgnoreCase)
                || path.StartsWith(AuthorizationConstants.PathsToBypass.Favicon, StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(AuthorizationConstants.HeaderNames.Authorization, out var authHeader)
                || string.IsNullOrWhiteSpace(authHeader))
            {
                await RespondUnauthorizedAsync(context, AuthorizationConstants.ErrorCodes.MissingAuthorizationHeader);
                return;
            }

            var headerValue = authHeader.ToString();
            if (!headerValue.StartsWith($"{AuthorizationConstants.Schemes.Bearer} ", StringComparison.OrdinalIgnoreCase))
            {
                await RespondUnauthorizedAsync(context, AuthorizationConstants.ErrorCodes.InvalidAuthorizationScheme);
                return;
            }

            var token = headerValue.Substring(AuthorizationConstants.Schemes.Bearer.Length).Trim();
            if (string.IsNullOrEmpty(token))
            {
                await RespondUnauthorizedAsync(context, AuthorizationConstants.ErrorCodes.EmptyBearerToken);
                return;
            }

            var client = _httpClientFactory.CreateClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, _authorizationEndpoint);
            requestMessage.Headers.Add(AuthorizationConstants.HeaderNames.Authorization, $"{AuthorizationConstants.Schemes.Bearer} {token}");

            try
            {
                var response = await client.SendAsync(requestMessage);

                if (response.StatusCode == HttpStatusCode.Unauthorized ||
                    response.StatusCode == HttpStatusCode.Forbidden)
                {
                    _logger.LogWarning("External auth returned {StatusCode}", response.StatusCode);
                    await RespondUnauthorizedAsync(context, AuthorizationConstants.ErrorCodes.InvalidToken);
                    return;
                }

                if ((int)response.StatusCode >= 500)
                {
                    _logger.LogError("Auth service error {StatusCode}", response.StatusCode);
                    await RespondUnauthorizedAsync(context, AuthorizationConstants.ErrorCodes.AuthorizationServiceUnavailable, StatusCodes.Status503ServiceUnavailable);
                    return;
                }

                if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 300)
                {
                    _logger.LogWarning("External auth failure.");
                    await RespondUnauthorizedAsync(context, AuthorizationConstants.ErrorCodes.AuthenticationFailed);
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Authorization request to external endpoint failed.");
                await RespondUnauthorizedAsync(context, AuthorizationConstants.ErrorCodes.AuthorizationServiceUnavailable, StatusCodes.Status503ServiceUnavailable);
                return;
            }

            await _next(context);
        }

        private static async Task RespondUnauthorizedAsync(HttpContext context, string errorCode, int statusCode = StatusCodes.Status401Unauthorized)
        {
            var error = new ErrorResponse
            {
                Type = $"https://httpstatuses.com/{statusCode}",
                Title = "Unauthorized",
                Status = statusCode,
                Instance = context.Request.Path,
                RequestId = context.TraceIdentifier,
                ErrorCode = errorCode,
                AdditionalData = new Dictionary<string, object>()
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = AuthorizationConstants.ContentTypes.ProblemJson;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(error));
        }
    }
}
