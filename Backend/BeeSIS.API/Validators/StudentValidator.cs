using FluentValidation;
using BeeSIS.API.Models;

namespace BeeSIS.API.Validators
{
    /// <summary>
    /// Validates student input data.
    /// Strategy Pattern: Each entity has its own validation strategy.
    /// OCP: New rules can be added without modifying existing code.
    /// </summary>
    public class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator()
        {
            RuleFor(s => s.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

            RuleFor(s => s.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            RuleFor(s => s.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(s => s.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d{10,15}$").WithMessage("Phone must be 10-15 digits.");

            RuleFor(s => s.Major)
                .NotEmpty().WithMessage("Major is required.");

            RuleFor(s => s.GPA)
                .InclusiveBetween(0.0, 4.0).WithMessage("GPA must be between 0.0 and 4.0.");
        }
    }
}
