using CDR_Bank.Hub.Services.Abstractions;
using CDR_Bank.Libs.API.Abstractions;
using CDR_Bank.Libs.Hub.Contracts.Banking;
using Microsoft.AspNetCore.Mvc;

namespace CDR_Bank.Hub.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : AController
    {
        private readonly IAccountService _bankingService;

        public AccountController(IAccountService bankingService)
        {
            _bankingService = bankingService ?? throw new ArgumentNullException(nameof(bankingService));
        }
    }
}
