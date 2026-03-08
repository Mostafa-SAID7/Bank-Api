using Bank.Application.DTOs;
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransaction(Guid id)
    {
        var transaction = await _transactionService.GetTransactionByIdAsync(id);
        if (transaction == null)
            return NotFound();
        
        return Ok(transaction);
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchTransactions(
        [FromBody] TransactionSearchRequest request)
    {
        var criteria = new TransactionSearchCriteria
        {
            AccountId = request.AccountId,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            Type = request.Type,
            Status = request.Status,
            MinAmount = request.MinAmount,
            MaxAmount = request.MaxAmount,
            Description = request.Description,
            Reference = request.Reference,
            FromAccountId = request.FromAccountId,
            ToAccountId = request.ToAccountId
        };

        var (transactions, totalCount) = await _transactionService.SearchTransactionsAsync(
            criteria, request.PageNumber, request.PageSize);

        return Ok(new
        {
            Transactions = transactions,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
        });
    }

    [HttpGet("by-date-range/{accountId}")]
    public async Task<IActionResult> GetTransactionsByDateRange(
        Guid accountId,
        [FromQuery] DateTime fromDate,
        [FromQuery] DateTime toDate)
    {
        var transactions = await _transactionService.GetTransactionsByDateRangeAsync(
            accountId, fromDate, toDate);
        return Ok(transactions);
    }

    [HttpGet("by-type/{accountId}")]
    public async Task<IActionResult> GetTransactionsByType(
        Guid accountId,
        [FromQuery] TransactionType type)
    {
        var transactions = await _transactionService.GetTransactionsByTypeAsync(accountId, type);
        return Ok(transactions);
    }

    [HttpGet("by-amount-range/{accountId}")]
    public async Task<IActionResult> GetTransactionsByAmountRange(
        Guid accountId,
        [FromQuery] decimal minAmount,
        [FromQuery] decimal maxAmount)
    {
        var transactions = await _transactionService.GetTransactionsByAmountRangeAsync(
            accountId, minAmount, maxAmount);
        return Ok(transactions);
    }

    [HttpGet("by-status/{accountId}")]
    public async Task<IActionResult> GetTransactionsByStatus(
        Guid accountId,
        [FromQuery] TransactionStatus status)
    {
        var transactions = await _transactionService.GetTransactionsByStatusAsync(accountId, status);
        return Ok(transactions);
    }

    [HttpPost("export/csv")]
    public async Task<IActionResult> ExportToCsv([FromBody] TransactionSearchRequest request)
    {
        var criteria = new TransactionSearchCriteria
        {
            AccountId = request.AccountId,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            Type = request.Type,
            Status = request.Status,
            MinAmount = request.MinAmount,
            MaxAmount = request.MaxAmount,
            Description = request.Description,
            Reference = request.Reference,
            FromAccountId = request.FromAccountId,
            ToAccountId = request.ToAccountId
        };

        var csvData = await _transactionService.ExportTransactionsToCsvAsync(criteria);
        
        return File(csvData, "text/csv", $"transactions_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv");
    }

    [HttpPost("export/excel")]
    public async Task<IActionResult> ExportToExcel([FromBody] TransactionSearchRequest request)
    {
        var criteria = new TransactionSearchCriteria
        {
            AccountId = request.AccountId,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            Type = request.Type,
            Status = request.Status,
            MinAmount = request.MinAmount,
            MaxAmount = request.MaxAmount,
            Description = request.Description,
            Reference = request.Reference,
            FromAccountId = request.FromAccountId,
            ToAccountId = request.ToAccountId
        };

        var excelData = await _transactionService.ExportTransactionsToExcelAsync(criteria);
        
        return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                   $"transactions_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx");
    }

    [HttpGet("statistics/{accountId}")]
    public async Task<IActionResult> GetStatistics(
        Guid accountId,
        [FromQuery] DateTime fromDate,
        [FromQuery] DateTime toDate)
    {
        var statistics = await _transactionService.GetTransactionStatisticsAsync(
            accountId, fromDate, toDate);
        return Ok(statistics);
    }
}
