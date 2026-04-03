using Bank.Application.DTOs.Payment.Biller;
using Bank.Application.Interfaces;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Bank.Application.Services;

/// <summary>
/// Service for managing biller data and queries
/// </summary>
public class BillerService : IBillerService
{
    private readonly IBillerRepository _billerRepository;
    private readonly ILogger<BillerService> _logger;

    public BillerService(
        IBillerRepository billerRepository,
        ILogger<BillerService> logger)
    {
        _billerRepository = billerRepository;
        _logger = logger;
    }

    public async Task<List<BillerDto>> GetAvailableBillersAsync()
    {
        _logger.LogInformation("Retrieving all available billers");
        var billers = await _billerRepository.GetActiveBillersAsync();
        return billers.Select(MapToBillerDto).ToList();
    }

    public async Task<List<BillerDto>> GetBillersByCategoryAsync(BillerCategory category)
    {
        _logger.LogInformation("Retrieving billers for category: {Category}", category);
        var billers = await _billerRepository.GetBillersByCategoryAsync(category);
        return billers.Select(MapToBillerDto).ToList();
    }

    public async Task<List<BillerDto>> SearchBillersAsync(BillerSearchRequest request)
    {
        _logger.LogInformation("Searching billers with criteria: SearchTerm={SearchTerm}, Category={Category}, ActiveOnly={ActiveOnly}",
            request.SearchTerm, request.Category, request.ActiveOnly);

        List<Biller> billers;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            billers = await _billerRepository.SearchBillersByNameAsync(request.SearchTerm);
        }
        else if (request.Category.HasValue)
        {
            billers = await _billerRepository.GetBillersByCategoryAsync(request.Category.Value);
        }
        else
        {
            billers = request.ActiveOnly 
                ? await _billerRepository.GetActiveBillersAsync()
                : (await _billerRepository.GetAllAsync()).ToList();
        }

        if (request.ActiveOnly)
        {
            billers = billers.Where(b => b.IsActive).ToList();
        }

        return billers.Select(MapToBillerDto).ToList();
    }

    public async Task<BillerDto?> GetBillerByIdAsync(Guid billerId)
    {
        _logger.LogInformation("Retrieving biller with ID: {BillerId}", billerId);
        var biller = await _billerRepository.GetByIdAsync(billerId);
        return biller != null ? MapToBillerDto(biller) : null;
    }

    #region Private Helper Methods

    private static BillerDto MapToBillerDto(Biller biller)
    {
        string[] supportedMethods = Array.Empty<string>();
        
        if (!string.IsNullOrEmpty(biller.SupportedPaymentMethods))
        {
            try
            {
                supportedMethods = JsonSerializer.Deserialize<string[]>(biller.SupportedPaymentMethods) ?? Array.Empty<string>();
            }
            catch
            {
                // If deserialization fails, return empty array
            }
        }

        return new BillerDto
        {
            Id = biller.Id,
            Name = biller.Name,
            Category = biller.Category,
            AccountNumber = biller.AccountNumber,
            RoutingNumber = biller.RoutingNumber,
            Address = biller.Address,
            IsActive = biller.IsActive,
            SupportedPaymentMethods = supportedMethods,
            MinAmount = biller.MinAmount,
            MaxAmount = biller.MaxAmount,
            ProcessingDays = biller.ProcessingDays,
            CreatedAt = biller.CreatedAt
        };
    }

    #endregion
}
