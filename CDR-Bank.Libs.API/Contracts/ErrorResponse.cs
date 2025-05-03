using CDR_Bank.Libs.API.Contracts.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CDR_Bank.Libs.API.Contracts
{
    /// <summary>
    /// The basic model of the error response.
    /// </summary>
    public class ErrorResponse : ProblemDetails, IResponse
    {
        public ErrorResponse()
        {
        }

        public ErrorResponse(string requestId, string errorCode, Dictionary<string, object> additionalData) : base()
        {
            RequestId = requestId;
            ErrorCode = errorCode;
            AdditionalData = additionalData;
        }

        /// <summary>
        /// Request ID (may help with tracing).
        /// </summary>
        public string RequestId { get; set; } = string.Empty;

        /// <summary>
        /// Error code for internal processing.
        /// </summary>
        public string ErrorCode { get; set; } = string.Empty;

        /// <summary>
        /// Additional error information, if necessary.
        /// </summary>
        public Dictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();
    }
}
