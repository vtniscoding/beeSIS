using FluentValidation;
using BeeSIS.API.Models;

namespace BeeSIS.API.Validators
{
    /// <summary>
    /// Validates login request input.
    /// Strategy Pattern: Separate validation strategy for login.
    /// </summary>
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(r => r.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        }
    }
}
