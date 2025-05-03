using CDR_Bank.IndentityServer.Services.Abstractions;
using CDR_Bank.Libs.API.Abstractions;
using CDR_Bank.Libs.Identity.Contracts.RequestContracts.Abstractions;
using CDR_Bank.Libs.Identity.Contracts.ResponseContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CDR_Bank.Hub.Controllers
{
    /// <summary>
    /// Controller for handling banking operations such as replenishment, withdrawal, and transfer.
    /// </summary>
    [ApiController]
    [Route("account")]
    public class UserController : AController
    {
        private readonly IIndentityService _identityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="identityService">Service for banking operations.</param>
        public UserController(IIndentityService identityService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        /// <summary>
        /// Replenishes the specified banking account with the given amount.
        /// </summary>
        /// <param name="request">The request containing account identifier and amount to replenish.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("registration")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TokenResponse> Registration([FromBody] UserLoginData request)
        {
            if (request == null)
                return BadRequest("Invalid request payload.");

            var token = _identityService.Registration(request);
            if (string.IsNullOrEmpty(token))
                return BadRequest("Registration failed.");

                return Ok(new TokenResponse { Token = token });
        }

        /// <summary>
        /// Withdraws the specified amount from the given banking account.
        /// </summary>
        /// <param name="request">The request containing account identifier and amount to withdraw.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TokenResponse> Login([FromBody] UserLoginData request)
        {
            if (request == null)
                return BadRequest("Invalid request payload.");

            var token = _identityService.Login(request);
            if (string.IsNullOrEmpty(token))
                return BadRequest("Login failed.");

            return Ok(new TokenResponse { Token = token });
        }
    }
}
