using ClarusAI.Core.Entities;

namespace ClarusAI.Core.Interfaces;

public interface IAuthenticationService
{
    Task<string> AuthenticateAsync(string username, string password);
    Task<bool> ValidateTokenAsync(string token);
    Task<User?> GetUserFromTokenAsync(string token);
    Task<bool> LogoutAsync(string token);
    Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
    Task<bool> ValidateUserPermissionsAsync(Guid userId, string resource, string action);
}