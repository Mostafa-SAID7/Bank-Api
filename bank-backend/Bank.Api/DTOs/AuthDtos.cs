namespace Bank.Api.DTOs;

// Auth
public record LoginRequest(string Email, string Password);
public record RegisterRequest(string Username, string Email, string Password);
public record AuthResponse(string Token);
