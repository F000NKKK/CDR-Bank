using CDR_Bank.Banking.Services.Abstractions;
using CDR_Bank.Libs.API.Abstractions;
using CDR_Bank.Libs.API.Contracts;
using CDR_Bank.Libs.Banking.Contracts.RequestContracts;
using CDR_Bank.Libs.Banking.Contracts.ResponseContracts;
using Microsoft.AspNetCore.Mvc;

namespace CDR_Bank.Banking.Controllers;

[ApiController]
[Route("banking")]
public class BankingController : AController
{
    private readonly IBankingService _bankingService;

    public BankingController(IBankingService bankingService)
    {
        _bankingService = bankingService ?? throw new ArgumentNullException(nameof(bankingService));
    }

    [HttpGet("balance")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    public ActionResult<decimal> GetBalance()
    {
        var userData = GetUserDataFromContext();

        var balance = _bankingService.GetBalance(userData!.Id);
        return Ok(balance);
    }


    [HttpPost("replenish")]
    public IActionResult Replenish([FromBody] BankingOperationContract request)
    {
        var userData = GetUserDataFromContext();

        if (request == null || request.Amount <= 0)
            return BadRequest("Invalid request payload.");

        _bankingService.Replenish(userData!.Id, request.BankingAccount, request.Amount);
        return Ok();
    }

    [HttpPost("withdraw")]
    public IActionResult Withdraw([FromBody] BankingOperationContract request)
    {
        if (request == null || request.Amount <= 0)
            return BadRequest("Invalid request payload.");

        if (!_bankingService.Withdraw(request.BankingAccount, request.Amount))
            return StatusCode(StatusCodes.Status402PaymentRequired, "Insufficient funds.");

        return Ok();
    }

    [HttpPost("transfer")]
    public IActionResult Transfer([FromBody] BankingTransferContract request)
    {
        if (request == null || request.Amount <= 0 || string.IsNullOrWhiteSpace(request.RecipientTelephoneNumber))
            return BadRequest("Invalid request payload.");

        if (!_bankingService.Transfer(request.BankingAccount, request.RecipientTelephoneNumber, request.Amount))
            return StatusCode(StatusCodes.Status402PaymentRequired, "Transfer failed.");

        return Ok();
    }

    /// <summary>
    /// Internal transfer between two accounts of the same client.
    /// </summary>
    [HttpPost("bank-account/transfer")]
    public IActionResult InternalTransfer([FromBody] InternalTransferContract request)
    {
        if (request == null || request.Amount <= 0)
            return BadRequest("Invalid request payload.");

        var success = _bankingService.InternalTransfer(
            request.SourceAccountId,
            request.DestinationAccountId,
            request.Amount);

        if (!success)
            return BadRequest("Transfer failed (insufficient funds or invalid accounts).");

        return Ok();
    }

    /// <summary>
    /// Open a new debit account.
    /// </summary>
    [HttpPost("bank-account/open")]
    public IActionResult OpenBankAccount([FromBody] OpenBankAccountContract request)
    {
        var userData = GetUserDataFromContext();

        if (!IsValidOpenRequest(request, userData!.Id))
            return BadRequest("Invalid request payload.");

        var accountId = _bankingService.CreateAccount(userData!.Id, request.Name, (DataAccess.Banking.Enums.BankAccountType)(int)request.Type, request.CreditLimit, request.IsMain);
        return Ok(new { accountId });
    }

    /// <summary>
    /// Close an existing account.
    /// </summary>
    [HttpPatch("bank-account/close")]
    public IActionResult Close([FromBody] BankingOperationContract request)
    {
        if (request == null)
            return BadRequest("Invalid request payload.");

        if (!_bankingService.CloseAccount(request.BankingAccount))
            return BadRequest("Account not found or already closed.");

        return Ok();
    }

    /// <summary>
    /// Edit account parameters (name, type, credit limit, isMain).
    /// </summary>
    [HttpPost("bank-account/edit")]
    public IActionResult Edit([FromBody] EditBankAccountContract request)
    {
        if (request == null)
            return BadRequest("Invalid request payload.");

        var success = _bankingService.EditAccount(
            request.BankingAccount,
            request.Name,
            request.Type.HasValue ? (DataAccess.Banking.Enums.BankAccountType)(int)request.Type.Value : null,
            request.CreditLimit,
            request.IsMain);

        if (!success)
            return BadRequest("Account not found or invalid parameters.");

        return Ok();
    }

    /// <summary>
    /// Get data for a specific account.
    /// </summary>
    [HttpGet("bank-account/get-data")]
    [ProducesResponseType(typeof(BankingAccountContract), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<BankingAccountContract> GetData([FromQuery] string bankingAccountRequest)
    {
        if (bankingAccountRequest == null)
        {
            return BadRequest("Invalid request.");
        }

        Guid bankingAccount;

        if (Guid.TryParse(bankingAccountRequest, out bankingAccount))
        {
            var account = _bankingService.GetAccountData(bankingAccount);

            if (account == null)
                return NotFound("Account not found.");

            return Ok(account);
        }
        else
        {
            return BadRequest("Invalid request.");
        }
    }

    [HttpGet("transactions")]
    [ProducesResponseType(typeof(PagedResult<AccountTransactionContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<PagedResult<AccountTransactionContract>> GetTransactions([FromQuery] TransactionFilterContract filter)
    {
        var userData = GetUserDataFromContext();

        if (userData.Id == Guid.Empty)
            return BadRequest("Invalid user ID.");

        if (filter.Page <= 0 || filter.PageSize <= 0)
            return BadRequest("Invalid pagination parameters.");

        var result = _bankingService.GetTransactions(userData.Id, filter);

        return Ok(result);
    }

    [HttpGet("bank-accounts")]
    [ProducesResponseType(typeof(PagedResult<BankingAccountContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<PagedResult<BankingAccountContract>> GetAccounts([FromQuery] int page, [FromQuery] int pageSize)
    {
        var userData = GetUserDataFromContext();

        if (userData.Id == Guid.Empty)
            return BadRequest("Invalid user ID.");

        if (page <= 0 || pageSize <= 0)
            return BadRequest("Invalid pagination parameters.");

        var result = _bankingService.GetAccounts(userData.Id, page, pageSize);

        return Ok(result);
    }

    private static bool IsValidOpenRequest(OpenBankAccountContract request, Guid userId) =>
        request != null && userId != Guid.Empty && !string.IsNullOrWhiteSpace(request.Name);

    private UserDataContract? GetUserDataFromContext()
    {
        if (HttpContext.Items.TryGetValue("UserData", out var userData))
        {
            return userData as UserDataContract;
        }
        return null;
    }
}
