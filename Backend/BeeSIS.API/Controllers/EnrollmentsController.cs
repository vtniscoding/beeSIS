using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BeeSIS.API.Models;
using BeeSIS.API.Services.Interfaces;

namespace BeeSIS.API.Controllers
{
    /// <summary>
    /// Handles student-course enrollment operations.
    /// SRP: Only enrollment resource endpoints.
    /// DIP: Depends on IEnrollmentService abstraction.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly ILogger<EnrollmentsController> _logger;

        public EnrollmentsController(IEnrollmentService enrollmentService, ILogger<EnrollmentsController> logger)
        {
            _enrollmentService = enrollmentService;
            _logger = logger;
        }

        /// <summary>GET /api/enrollments — Returns all enrollments.</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
            return Ok(ApiResponse<List<Enrollment>>.Success(enrollments, $"{enrollments.Count} enrollments retrieved."));
        }

        /// <summary>GET /api/enrollments/{id} — Returns a single enrollment.</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id);
            if (enrollment == null)
                return NotFound(ApiResponse<Enrollment>.Failure("Enrollment not found", $"No enrollment with ID '{id}'."));

            return Ok(ApiResponse<Enrollment>.Success(enrollment));
        }

        /// <summary>GET /api/enrollments/student/{studentId} — Returns all enrollments for a specific student.</summary>
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudent(string studentId)
        {
            var enrollments = await _enrollmentService.GetEnrollmentsByStudentIdAsync(studentId);
            return Ok(ApiResponse<List<Enrollment>>.Success(enrollments, $"{enrollments.Count} enrollments for student '{studentId}'."));
        }

        /// <summary>POST /api/enrollments — Creates a new enrollment. Admin and Faculty only.</summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Create([FromBody] Enrollment enrollment)
        {
            if (string.IsNullOrWhiteSpace(enrollment.StudentId))
                return BadRequest(ApiResponse<Enrollment>.Failure("Validation failed", "StudentId is required."));

            if (string.IsNullOrWhiteSpace(enrollment.CourseCode))
                return BadRequest(ApiResponse<Enrollment>.Failure("Validation failed", "CourseCode is required."));

            var result = await _enrollmentService.AddEnrollmentAsync(enrollment);
            if (!result.IsSuccess)
                return Conflict(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.EnrollmentId }, result);
        }

        /// <summary>PUT /api/enrollments/{id} — Updates an enrollment (e.g., grade, status). Admin and Faculty only.</summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Update(string id, [FromBody] Enrollment enrollment)
        {
            var result = await _enrollmentService.UpdateEnrollmentAsync(id, enrollment);
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>DELETE /api/enrollments/{id} — Deletes an enrollment. Admin only.</summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _enrollmentService.DeleteEnrollmentAsync(id);
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }
    }
}
