using Bank.Application.DTOs;
using Bank.Application.Interfaces;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Bank.Application.Services;

/// <summary>
/// Service implementation for batch payment processing
/// </summary>
public class BatchPaymentService : IBatchPaymentService
{
    private readonly IBillPaymentRepository _billPaymentRepository;
    private readonly IBillerIntegrationService _billerIntegrationService;
    private readonly IPaymentReceiptService _paymen