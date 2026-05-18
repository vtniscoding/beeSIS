using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using BeeSIS.API.Models;
using BeeSIS.API.Services.Interfaces;

namespace BeeSIS.API.Controllers
{
    /// <summary>
    /// Handles all course CRUD operations.
    /// SRP: Only course resource endpoints.
    /// DIP: Depends on ICourseService and IValidator abstractions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IValidator<Course> _courseValidator;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(ICourseService courseService, IValidator<Course> courseValidator, ILogger<CoursesController> logger)
        {
            _courseService = courseService;
            _courseValidator = courseValidator;
            _logger = logger;
        }

        /// <summary>GET /api/courses — Returns all courses.</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(ApiResponse<List<Course>>.Success(courses, $"{courses.Count} courses retrieved."));
        }

        /// <summary>GET /api/courses/{code} — Returns a single course by code.</summary>
        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var course = await _courseService.GetCourseByCourseCodeAsync(code);
            if (course == null)
                return NotFound(ApiResponse<Course>.Failure("Course not found", $"No course with code '{code}'."));

            return Ok(ApiResponse<Course>.Success(course));
        }

        /// <summary>POST /api/courses — Creates a new course. Admin and Faculty only.</summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Create([FromBody] Course course)
        {
            var validation = await _courseValidator.ValidateAsync(course);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<Course>.Failure("Validation failed", errors));
            }

            var result = await _courseService.AddCourseAsync(course);
            if (!result.IsSuccess)
                return Conflict(result);

            return CreatedAtAction(nameof(GetByCode), new { code = result.Data!.CourseCode }, result);
        }

        /// <summary>PUT /api/courses/{code} — Updates a course. Admin and Faculty only.</summary>
        [HttpPut("{code}")]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Update(string code, [FromBody] Course course)
        {
            var validation = await _courseValidator.ValidateAsync(course);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<Course>.Failure("Validation failed", errors));
            }

            var result = await _courseService.UpdateCourseAsync(code, course);
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>DELETE /api/courses/{code} — Deletes a course. Admin only.</summary>
        [HttpDelete("{code}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string code)
        {
            var result = await _courseService.DeleteCourseAsync(code);
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>GET /api/courses/download — Downloads all courses as CSV.</summary>
        [HttpGet("download")]
        public async Task<IActionResult> Download()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            var csv = _courseService.ConvertToCsv(courses);
            var bytes = System.Text.Encoding.UTF8.GetBytes(csv);

            return File(bytes, "text/csv", $"courses_{DateTime.Today:yyyy-MM-dd}.csv");
        }
    }
}
