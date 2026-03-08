namespace Bank.Application.DTOs;

// Profile
public record ProfileResponse(Guid Id, string UserName, string Email, string FirstName, string LastName);
public record UpdateProfileRequest(string FirstName, string LastName);
