using FluentValidation;
using BeeSIS.API.Models;

namespace BeeSIS.API.Validators
{
    /// <summary>
    /// Validates course input data.
    /// Strategy Pattern: Separate validation strategy for course entities.
    /// </summary>
    public class CourseValidator : AbstractValidator<Course>
    {
        public CourseValidator()
        {
            RuleFor(c => c.CourseCode)
                .NotEmpty().WithMessage("Course code is required.")
                .MaximumLength(20).WithMessage("Course code cannot exceed 20 characters.")
                .Matches(@"^[A-Za-z]{2,4}\d{3,4}$").WithMessage("Course code format must be like CS101 or MATH2001.");

            RuleFor(c => c.CourseName)
                .NotEmpty().WithMessage("Course name is required.")
                .MaximumLength(100).WithMessage("Course name cannot exceed 100 characters.");

            RuleFor(c => c.Credits)
                .InclusiveBetween(1, 6).WithMessage("Credits must be between 1 and 6.");

            RuleFor(c => c.Instructor)
                .NotEmpty().WithMessage("Instructor name is required.");

            RuleFor(c => c.Department)
                .NotEmpty().WithMessage("Department is required.");
        }
    }
}
