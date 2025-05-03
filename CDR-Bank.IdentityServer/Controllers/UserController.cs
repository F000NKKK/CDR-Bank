using CDR_Bank.IndentityServer.Services.Abstractions;
using CDR_Bank.Libs.API.Abstractions;
using CDR_Bank.Libs.Identity.Contracts.RequestContracts;
using CDR_Bank.Libs.Identity.Contracts.RequestContracts.Abstractions;
using CDR_Bank.Libs.Identity.Contracts.ResponseContracts;
using Microsoft.AspNetCore.Mvc;

namespace CDR_Bank.Hub.Controllers
{

    [ApiController]
    [Route("account")]
    public class UserController : AController
    {
        private readonly IIndentityService _identityService;


        public UserController(IIndentityService identityService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        [HttpPost("registration")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TokenResponse> Registration([FromBody] UserRegistrationContract request)
        {
            if (request == null)
                return BadRequest("Invalid request payload.");

            var token = _identityService.Registration(request);
            if (string.IsNullOrEmpty(token))
                return BadRequest("Registration failed.");

            return Ok(new TokenResponse { Token = token });
        }


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


        [HttpGet("get-user")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserData> GetUser()
        {
            var token = ControllerContext.HttpContext.Request.Headers.Authorization.ToString();
            if (token.StartsWith("Bearer"))
            {
                token = token.Split(' ')[1];
            }
            else
            {
                return BadRequest("Bad token");
            }
            UserData result = _identityService.GetUserData(token);
            return Ok(result);
        }

        [HttpGet("get-user-contact-info")]
        [ProducesResponseType(typeof(UserContactInfoContract), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserContactInfoContract> GetUserContactInfo()
        {
            var token = ControllerContext.HttpContext.Request.Headers.Authorization.ToString();
            if (token.StartsWith("Bearer"))
            {
                token = token.Split(' ')[1];
            }
            else
            {
                return BadRequest("Bad token");
            }
            UserContactInfoContract result = _identityService.GetUserContactsData(token);
            return Ok(result);
        }


        [HttpPost("change-password")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult ChangePassword(PasswordChange passwordChange)
        {
            var token = ControllerContext.HttpContext.Request.Headers.Authorization.ToString();
            if (token.StartsWith("Bearer"))
            {
                token = token.Split(' ')[1];
            }
            else
            {
                return BadRequest("Bad token");
            }
            bool result = _identityService.ChangePassword(token, passwordChange);
            if (!result)
            {
                return BadRequest("Change failed.");
            }
            return Ok();
        }


        [HttpPost("edit")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Edit(UserContactInfoContract userContactInfo)
        {
            var token = ControllerContext.HttpContext.Request.Headers.Authorization.ToString();
            if (token.StartsWith("Bearer"))
            {
                token = token.Split(' ')[1];
            }
            else
            {
                return BadRequest("Bad token");
            }
            bool result = _identityService.Edit(token, userContactInfo);
            if (!result)
            {
                return BadRequest("Change failed.");
            }
            return Ok();
        }



        [HttpPost("Test")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Test(TokenResponse token)
        {
            _identityService.GetUserContactsData(token.Token);
            return Ok();
        }


    }
}
