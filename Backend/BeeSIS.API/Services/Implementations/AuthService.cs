using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BeeSIS.API.Models;
using BeeSIS.API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BeeSIS.API.Services.Implementations
{
    /// <summary>
    /// Implements authentication and JWT token generation.
    /// SRP: Only handles authentication logic.
    /// Factory Pattern: Creates different token claims based on user role.
    /// </summary>
    public class AuthService : IAuthService
    {
        private const string CsvFileName = "users.csv";
        private readonly ICsvDataService _csvDataService;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ICsvDataService csvDataService, IConfiguration config, ILogger<AuthService> logger)
        {
            _csvDataService = csvDataService;
            _config = config;
            _logger = logger;
        }

        public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return ApiResponse<LoginResponse>.Failure("Invalid credentials", "Username and password are required.");

            var user = await GetUserByUsernameAsync(request.Username);

            if (user == null)
            {
                _logger.LogWarning("Login failed: user '{Username}' not found", request.Username);
                return ApiResponse<LoginResponse>.Failure("Invalid credentials", "Username or password is incorrect.");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed: wrong password for '{Username}'", request.Username);
                return ApiResponse<LoginResponse>.Failure("Invalid credentials", "Username or password is incorrect.");
            }

            if (!user.Status.Equals("Active", StringComparison.OrdinalIgnoreCase))
                return ApiResponse<LoginResponse>.Failure("Account inactive", "Your account has been deactivated.");

            var expirationMinutes = int.Parse(_config["Jwt:ExpirationMinutes"] ?? "60");
            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);
            var token = GenerateJwtToken(user);

            _logger.LogInformation("User '{Username}' logged in successfully with role '{Role}'", user.Username, user.Role);

            return ApiResponse<LoginResponse>.Success(new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Role = user.Role,
                Email = user.Email,
                ExpiresAt = expiresAt
            }, "Login successful.");
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var users = await _csvDataService.ReadCsvFromGitHubAsync<User>(CsvFileName);
            return users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Generates a signed JWT token for the given user.
        /// Factory Pattern: Claims set differs by role.
        /// </summary>
        public string GenerateJwtToken(User user)
        {
            var secretKey = _config["Jwt:SecretKey"]
                ?? throw new InvalidOperationException("JWT SecretKey is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expirationMinutes = int.Parse(_config["Jwt:ExpirationMinutes"] ?? "60");

            // Factory Pattern: build claims based on user role
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
