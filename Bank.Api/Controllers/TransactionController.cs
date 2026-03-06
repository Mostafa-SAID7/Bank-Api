using Bank.Application.Interfaces;
using Bank.Application.Commands;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly IMediator _mediator;

    public TransactionController(ITransactionService transactionService, IMediator mediator)
    {
        _transactionService = transactionService;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> InitiateTransaction([FromBody] InitiateTransactionCommand request)
    {
        try
        {
            var transaction = await _mediator.Send(request);
            return Ok(transaction);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("history/{accountId}")]
    public async Task<IActionResult> GetHistory(Guid accountId)
    {
        var history = await _transactionService.GetTransactionHistoryAsync(accountId);
        return Ok(history);
    }
}
