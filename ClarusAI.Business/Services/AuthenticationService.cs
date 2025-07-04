using ClarusAI.Core.Entities;
using ClarusAI.Core.Interfaces;
using ClarusAI.Core.Constants;
using Microsoft.Extensions.Logging;

namespace ClarusAI.Business.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IRepository<User> _userRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly JwtTokenService _jwtTokenService;
    private readonly IAuditService _auditService;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly Dictionary<string, string> _activeTokens = new();

    public AuthenticationService(
        IRepository<User> userRepository,
        IEncryptionService encryptionService,
        JwtTokenService jwtTokenService,
        IAuditService auditService,
        ILogger<AuthenticationService> logger)
    {
        _userRepository = userRepository;
        _encryptionService = encryptionService;
        _jwtTokenService = jwtTokenService;
        _auditService = auditService;
        _logger = logger;
    }

    public async Task<string> AuthenticateAsync(string username, string password)
    {
        try
        {
            var users = await _userRepository.FindAsync(u => u.Username == username || u.Email == username);
            var user = users.FirstOrDefault();

            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("Authentication failed for username: {Username}", username);
                return string.Empty;
            }

            if (!_encryptionService.VerifyPassword(password, user.PasswordHash))
            {
                _logger.LogWarning("Password verification failed for user: {UserId}", user.Id);
                return string.Empty;
            }

            // Get user roles and permissions
            var roles = GetUserRoles(user.Role);
            var permissions = GetRolePermissions(user.Role);

            var token = _jwtTokenService.GenerateToken(user, roles, permissions);
            _activeTokens[token] = user.Id.ToString();

            // Update last login
            user.LastLoginAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            await _auditService.LogActionAsync(
                user.Id, 
                "Login", 
                "User", 
                user.Id.ToString(), 
                "Successful authentication", 
                "", 
                ""
            );

            _logger.LogInformation("User {UserId} authenticated successfully", user.Id);
            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during authentication for username: {Username}", username);
            return string.Empty;
        }
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            if (!_activeTokens.ContainsKey(token))
                return false;

            var principal = _jwtTokenService.ValidateToken(token);
            if (principal == null)
            {
                _activeTokens.Remove(token);
                return false;
            }

            return !_jwtTokenService.IsTokenExpired(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating token");
            return false;
        }
    }

    public async Task<User?> GetUserFromTokenAsync(string token)
    {
        try
        {
            var userId = _jwtTokenService.GetUserIdFromToken(token);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                return null;

            return await _userRepository.GetByIdAsync(userGuid);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user from token");
            return null;
        }
    }

    public async Task<bool> LogoutAsync(string token)
    {
        try
        {
            var user = await GetUserFromTokenAsync(token);
            if (user != null)
            {
                await _auditService.LogActionAsync(
                    user.Id, 
                    "Logout", 
                    "User", 
                    user.Id.ToString(), 
                    "User logged out", 
                    "", 
                    ""
                );
            }

            _activeTokens.Remove(token);
            _logger.LogInformation("User logged out successfully");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return false;
        }
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            // Verify current password
            if (!_encryptionService.VerifyPassword(currentPassword, user.PasswordHash))
                return false;

            // Hash new password and update user
            user.PasswordHash = _encryptionService.HashPassword(newPassword);
            
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            await _auditService.LogActionAsync(
                userId, 
                "PasswordChange", 
                "User", 
                userId.ToString(), 
                "Password changed successfully", 
                "", 
                ""
            );

            _logger.LogInformation("Password changed for user {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for user {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> ValidateUserPermissionsAsync(Guid userId, string resource, string action)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || !user.IsActive)
                return false;

            var permissions = GetRolePermissions(user.Role);
            var requiredPermission = $"{action}{resource}";

            return permissions.Contains(requiredPermission) || permissions.Contains($"*");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating permissions for user {UserId}", userId);
            return false;
        }
    }

    private static List<string> GetUserRoles(string userRole)
    {
        return new List<string> { userRole };
    }

    private static List<string> GetRolePermissions(string role)
    {
        return role switch
        {
            Roles.Administrator => new List<string>
            {
                Permissions.ViewPatients, Permissions.EditPatients, Permissions.DeletePatients,
                Permissions.ViewMedicalImages, Permissions.UploadMedicalImages, Permissions.DeleteMedicalImages,
                Permissions.RequestAnalysis, Permissions.ViewAnalysisResults,
                Permissions.ManageUsers, Permissions.ViewAuditLogs
            },
            Roles.Radiologist => new List<string>
            {
                Permissions.ViewPatients, Permissions.EditPatients,
                Permissions.ViewMedicalImages, Permissions.UploadMedicalImages,
                Permissions.RequestAnalysis, Permissions.ViewAnalysisResults
            },
            Roles.Clinician => new List<string>
            {
                Permissions.ViewPatients, Permissions.EditPatients,
                Permissions.ViewMedicalImages, Permissions.UploadMedicalImages,
                Permissions.RequestAnalysis, Permissions.ViewAnalysisResults
            },
            Roles.Technician => new List<string>
            {
                Permissions.ViewPatients,
                Permissions.ViewMedicalImages, Permissions.UploadMedicalImages
            },
            Roles.Viewer => new List<string>
            {
                Permissions.ViewPatients,
                Permissions.ViewMedicalImages, Permissions.ViewAnalysisResults
            },
            _ => new List<string>()
        };
    }
}