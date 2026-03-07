using Bank.Domain.Entities;

namespace Bank.Application.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(string email, string password);
    Task<User> RegisterAsync(string username, string email, string password);
    Task<User?> GetUserByEmailAsync(string email);
}
