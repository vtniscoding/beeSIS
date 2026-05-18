using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using BeeSIS.API.Models;
using BeeSIS.API.Services.Interfaces;

namespace BeeSIS.API.Controllers
{
    /// <summary>
    /// Handles all student CRUD operations.
    /// SRP: Only student resource endpoints.
    /// DIP: Depends on IStudentService and IValidator abstractions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // All student endpoints require authentication
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IValidator<Student> _studentValidator;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentService studentService, IValidator<Student> studentValidator, ILogger<StudentsController> logger)
        {
            _studentService = studentService;
            _studentValidator = studentValidator;
            _logger = logger;
        }

        /// <summary>GET /api/students — Returns all students. Accessible by all authenticated roles.</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(ApiResponse<List<Student>>.Success(students, $"{students.Count} students retrieved."));
        }

        /// <summary>GET /api/students/{id} — Returns a single student by ID.</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound(ApiResponse<Student>.Failure("Student not found", $"No student with ID '{id}'."));

            return Ok(ApiResponse<Student>.Success(student));
        }

        /// <summary>POST /api/students — Creates a new student. Admin only.</summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] Student student)
        {
            var validation = await _studentValidator.ValidateAsync(student);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<Student>.Failure("Validation failed", errors));
            }

            var result = await _studentService.AddStudentAsync(student);
            if (!result.IsSuccess)
                return Conflict(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
        }

        /// <summary>PUT /api/students/{id} — Updates an existing student. Admin only.</summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string id, [FromBody] Student student)
        {
            var validation = await _studentValidator.ValidateAsync(student);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<Student>.Failure("Validation failed", errors));
            }

            var result = await _studentService.UpdateStudentAsync(id, student);
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>DELETE /api/students/{id} — Deletes a student. Admin only.</summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _studentService.DeleteStudentAsync(id);
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>GET /api/students/download — Downloads all students as a CSV file.</summary>
        [HttpGet("download")]
        public async Task<IActionResult> Download()
        {
            var students = await _studentService.GetAllStudentsAsync();
            var csv = _studentService.ConvertToCsv(students);
            var bytes = System.Text.Encoding.UTF8.GetBytes(csv);

            return File(bytes, "text/csv", $"students_{DateTime.Today:yyyy-MM-dd}.csv");
        }
    }
}
