namespace CDR_Bank.Libs.API.Constants
{
    public static class AuthorizationConstants
    {
        public static class PathsToBypass
        {
            public const string Swagger = "/swagger";
            public const string Favicon = "/favicon.ico";
        }

        public static class HeaderNames
        {
            public const string Authorization = "Authorization";
        }

        public static class Schemes
        {
            public const string Bearer = "Bearer";
        }

        public static class ErrorCodes
        {
            public const string MissingAuthorizationHeader = "MissingAuthorizationHeader";
            public const string InvalidAuthorizationScheme = "InvalidAuthorizationScheme";
            public const string EmptyBearerToken = "EmptyBearerToken";
            public const string InvalidToken = "InvalidToken";
            public const string AuthorizationServiceUnavailable = "AuthorizationServiceUnavailable";
            public const string AuthenticationFailed = "AuthenticationFailed";
        }

        public static class ContentTypes
        {
            public const string ProblemJson = "application/problem+json";
        }

        public static class ConfigKeys
        {
            public const string AuthorizationEndpoint = "ApiSettings:AuthorizationEndpoint";
        }
    }
}
