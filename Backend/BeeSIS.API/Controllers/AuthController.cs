using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using BeeSIS.API.Models;
using BeeSIS.API.Services.Interfaces;

namespace BeeSIS.API.Controllers
{
    /// <summary>
    /// Handles authentication: login and current user info.
    /// SRP: Only authentication-related endpoints.
    /// DIP: Depends on IAuthService interface.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, IValidator<LoginRequest> loginValidator, ILogger<AuthController> logger)
        {
            _authService = authService;
            _loginValidator = loginValidator;
            _logger = logger;
        }

        /// <summary>POST /api/auth/login — Authenticates a user and returns a JWT token.</summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var validation = await _loginValidator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<object>.Failure("Validation failed", errors));
            }

            var result = await _authService.LoginAsync(request);

            if (!result.IsSuccess)
                return Unauthorized(result);

            return Ok(result);
        }

        /// <summary>POST /api/auth/logout — Client-side logout (token invalidation reminder).</summary>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // JWT is stateless — actual invalidation is done client-side by removing the token.
            return Ok(new { message = "Logged out successfully. Please remove your token." });
        }

        /// <summary>GET /api/auth/me — Returns the current authenticated user's info from the JWT token claims.</summary>
        [HttpGet("me")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new
            {
                id = userId,
                username,
                email,
                role
            });
        }
    }
}
