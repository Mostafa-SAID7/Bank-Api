using Bank.Domain.Enums;

namespace Bank.Domain.ValueObjects;

/// <summary>
/// Value object representing an authentication method configuration
/// </summary>
public class AuthenticationMethod
{
    public TwoFactorMethod Method { get; private set; }
    public string Destination { get; private set; }
    public bool IsEnabled { get; private set; }
    public DateTime? LastUsed { get; private set; }
    public string? SecretKey { get; private set; } // For authenticator apps
    
    private AuthenticationMethod(TwoFactorMethod method, string destination, bool isEnabled, string? secretKey = null)
    {
        Method = method;
        Destination = destination;
        IsEnabled = isEnabled;
        SecretKey = secretKey;
    }
    
    public static AuthenticationMethod CreateSms(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number is required for SMS authentication", nameof(phoneNumber));
            
        return new AuthenticationMethod(TwoFactorMethod.SMS, phoneNumber, true);
    }
    
    public static AuthenticationMethod CreateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required for email authentication", nameof(email));
            
        return new AuthenticationMethod(TwoFactorMethod.Email, email, true);
    }
    
    public static AuthenticationMethod CreateAuthenticatorApp(string secretKey)
    {
        if (string.IsNullOrWhiteSpace(secretKey))
            throw new ArgumentException("Secret key is required for authenticator app", nameof(secretKey));
            
        return new AuthenticationMethod(TwoFactorMethod.AuthenticatorApp, "Authenticator App", true, secretKey);
    }
    
    public AuthenticationMethod Enable()
    {
        return new AuthenticationMethod(Method, Destination, true, SecretKey);
    }
    
    public AuthenticationMethod Disable()
    {
        return new AuthenticationMethod(Method, Destination, false, SecretKey);
    }
    
    public AuthenticationMethod MarkAsUsed()
    {
        var updated = new AuthenticationMethod(Method, Destination, IsEnabled, SecretKey);
        updated.LastUsed = DateTime.UtcNow;
        return updated;
    }
}