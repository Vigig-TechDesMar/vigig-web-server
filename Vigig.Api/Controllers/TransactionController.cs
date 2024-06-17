using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Vigig.Api.Controllers.Base;
using Vigig.Service.Constants;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.PayOS;
using Vigig.Service.Models.Response;

namespace Vigig.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TransactionController : BaseApiController
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet("all")]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> GetAllTransactions()
    {
        return await ExecuteServiceLogic(async () =>
            await _transactionService.GetAllAsync().ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetTransactions([FromQuery]BasePaginatedRequest request)
    {
        return await ExecuteServiceLogic(async () 
            => await _transactionService.GetPaginatedResultAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTransactionById(int id)
    {
        return await ExecuteServiceLogic(async () => 
            await _transactionService.GetById(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchUsingGet(SearchUsingGet request)
    {
        return await ExecuteServiceLogic(async () => 
            await _transactionService.SearchTransaction(request).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpPost("/payos/returnUrl")]
    [AllowAnonymous]
    public async Task<Response> ConfirmPayment([FromBody] WebhookType request)
    {
        return await _transactionService.ProcessPayOSReturnResult(request);
    }
}