using Bank.Application.Interfaces;
using Bank.Domain.Enums;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BatchController : ControllerBase
{
    private readonly IBatchService _batchService;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public BatchController(IBatchService batchService, IBackgroundJobClient backgroundJobClient)
    {
        _batchService = batchService;
        _backgroundJobClient = backgroundJobClient;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadBatch([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        // Simulate file parsing (In a real app, parse CSV/JSON)
        var totalRecords = new Random().Next(10, 100);
        var job = await _batchService.CreateBatchJobAsync(file.FileName, totalRecords);

        // Mock some transactions for the batch
        var mockTransactions = Enumerable.Range(0, totalRecords).Select(_ => new TransactionRequest(
            Guid.NewGuid(), Guid.NewGuid(), new Random().Next(10, 1000), TransactionType.ACH, "Batch processing"
        ));

        // Enqueue background job
        _backgroundJobClient.Enqueue<IBatchService>(x => x.ProcessBatchAsync(job.Id, mockTransactions));

        return Ok(new { Message = "Batch file uploaded and processing initiated.", JobId = job.Id });
    }

    [HttpGet("status/{jobId}")]
    public async Task<IActionResult> GetStatus(Guid jobId)
    {
        var job = await _batchService.GetBatchJobStatusAsync(jobId);
        if (job == null) return NotFound();
        return Ok(job);
    }
}
