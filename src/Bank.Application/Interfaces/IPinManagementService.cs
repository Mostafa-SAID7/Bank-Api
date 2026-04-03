using Bank.Application.DTOs;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service for managing card PIN operations
/// </summary>
public interface IPinManagementService
{
    /// <summary>
    /// Set PIN for a card (initial PIN setup)
    /// </summary>
    Task<PinOperationResponse> SetPinAsync(SetPinRequest request, string userId);
    
    /// <summary>
    /// Change existing PIN for a card
    /// </summary>
    Task<PinOperationResponse> ChangePinAsync(SetPinRequest request, string userId);
    
    /// <summary>
    /// Reset PIN for a card (requires verification)
    /// </summary>
    Task<PinOperationResponse> ResetPinAsync(ResetPinRequest request, string userId);
    
    /// <summary>
    /// Verify PIN for a card
    /// </summary>
    Task<PinVerificationResult> VerifyPinAsync(VerifyPinRequest request, string userId);
    
    /// <summary>
    /// Generate verification code for PIN reset
    /// </summary>
    Task<string> GenerateVerificationCodeAsync(string cardId, string verificationMethod, string userId);
    
    /// <summary>
    /// Validate verification code for PIN reset
    /// </summary>
    Task<bool> ValidateVerificationCodeAsync(string cardId, string verificationCode, string userId);
    
    /// <summary>
    /// Unblock card after PIN reset
    /// </summary>
    Task<PinOperationResponse> UnblockCardAsync(string cardId, string userId);
    
    /// <summary>
    /// Get PIN status for a card
    /// </summary>
    Task<bool> HasPinSetAsync(string cardId, string userId);
}