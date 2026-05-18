using BeeSIS.API.Models;

namespace BeeSIS.API.Services.Interfaces
{
    /// <summary>
    /// Interface for authentication and authorization operations.
    /// SRP: Only authentication methods.
    /// ISP: Auth-specific operations only.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>Validates credentials and returns a JWT token with user info.</summary>
        Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);

        /// <summary>Returns the user matching the given username.</summary>
        Task<User?> GetUserByUsernameAsync(string username);

        /// <summary>Generates a JWT token for the given user.</summary>
        string GenerateJwtToken(User user);
    }

    /// <summary>
    /// Response payload returned on successful login.
    /// </summary>
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
