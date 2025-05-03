using CDR_Bank.Banking.Services.Abstractions;
using CDR_Bank.Libs.API.Abstractions;
using CDR_Bank.Libs.Hub.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CDR_Bank.Banking.Controllers
{
    /// <summary>
    /// Controller for handling banking operations such as replenishment, withdrawal, and transfer.
    /// </summary>
    [ApiController]
    [Route("banking")]
    public class BankingController : AController
    {
        private readonly IBankingService _bankingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BankingController"/> class.
        /// </summary>
        /// <param name="bankingService">Service for banking operations.</param>
        public BankingController(IBankingService bankingService)
        {
            _bankingService = bankingService ?? throw new ArgumentNullException(nameof(bankingService));
        }

        /// <summary>
        /// Replenishes the specified banking account with the given amount.
        /// </summary>
        /// <param name="request">The request containing account identifier and amount to replenish.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("replenish")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Replenish([FromBody] BankingOperationContract request)
        {
            if (request == null || request.Amount <= 0)
            {
                return BadRequest("Invalid request payload.");
            }

            _bankingService.Replenish(request.BankingAccount, request.Amount);
            return Ok();
        }

        /// <summary>
        /// Withdraws the specified amount from the given banking account.
        /// </summary>
        /// <param name="request">The request containing account identifier and amount to withdraw.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("withdraw")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status402PaymentRequired)]
        public IActionResult Withdraw([FromBody] BankingOperationContract request)
        {
            if (request == null || request.Amount <= 0)
            {
                return BadRequest("Invalid request payload.");
            }

            var success = _bankingService.Withdraw(request.BankingAccount, request.Amount);
            if (!success)
            {
                return StatusCode(StatusCodes.Status402PaymentRequired, "Insufficient funds.");
            }

            return Ok();
        }

        /// <summary>
        /// Transfers the specified amount from the sender's account to another client's account by phone number.
        /// </summary>
        /// <param name="request">The request containing sender account, recipient telephone number, and amount.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("transfer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status402PaymentRequired)]
        public IActionResult Transfer([FromBody] BankingTransferContract request)
        {
            if (request == null || request.Amount <= 0 || string.IsNullOrWhiteSpace(request.RecipientTelephoneNumber))
            {
                return BadRequest("Invalid request payload.");
            }

            var success = _bankingService.Transfer(request.BankingAccount, request.RecipientTelephoneNumber, request.Amount);
            if (!success)
            {
                return StatusCode(StatusCodes.Status402PaymentRequired, "Transfer failed due to insufficient funds or invalid recipient.");
            }

            return Ok();
        }
    }
}
