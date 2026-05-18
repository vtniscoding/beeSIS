using BeeSIS.API.Models;
using BeeSIS.API.Services.Interfaces;

namespace BeeSIS.API.Services.Implementations
{
    /// <summary>
    /// Implements student business logic.
    /// SRP: Only handles student operations.
    /// DIP: Depends on ICsvDataService interface, not a concrete class.
    /// </summary>
    public class StudentService : IStudentService
    {
        private const string CsvFileName = "students.csv";
        private readonly ICsvDataService _csvDataService;
        private readonly ILogger<StudentService> _logger;

        // DIP: Constructor injection of abstraction
        public StudentService(ICsvDataService csvDataService, ILogger<StudentService> logger)
        {
            _csvDataService = csvDataService;
            _logger = logger;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            _logger.LogInformation("Fetching all students");
            return await _csvDataService.ReadCsvFromGitHubAsync<Student>(CsvFileName);
        }

        public async Task<Student?> GetStudentByIdAsync(string id)
        {
            var students = await GetAllStudentsAsync();
            return students.FirstOrDefault(s => s.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<ApiResponse<Student>> AddStudentAsync(Student student)
        {
            var students = await GetAllStudentsAsync();

            // Check for duplicate ID or email
            if (students.Any(s => s.Id.Equals(student.Id, StringComparison.OrdinalIgnoreCase)))
                return ApiResponse<Student>.Failure("Student already exists", $"Student with ID '{student.Id}' already exists.");

            if (students.Any(s => s.Email.Equals(student.Email, StringComparison.OrdinalIgnoreCase)))
                return ApiResponse<Student>.Failure("Duplicate email", $"Email '{student.Email}' is already registered.");

            // Auto-generate ID if not provided
            if (string.IsNullOrWhiteSpace(student.Id))
                student.Id = GenerateStudentId(students);

            students.Add(student);
            await _csvDataService.WriteCsvToLocalAsync(students, CsvFileName);

            _logger.LogInformation("Student added: {StudentId}", student.Id);
            return ApiResponse<Student>.Success(student, "Student created successfully.");
        }

        public async Task<ApiResponse<Student>> UpdateStudentAsync(string id, Student updatedStudent)
        {
            var students = await GetAllStudentsAsync();
            var index = students.FindIndex(s => s.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (index < 0)
                return ApiResponse<Student>.Failure("Student not found", $"No student with ID '{id}'.");

            // Preserve the original ID
            updatedStudent.Id = id;
            students[index] = updatedStudent;

            await _csvDataService.WriteCsvToLocalAsync(students, CsvFileName);

            _logger.LogInformation("Student updated: {StudentId}", id);
            return ApiResponse<Student>.Success(updatedStudent, "Student updated successfully.");
        }

        public async Task<ApiResponse<bool>> DeleteStudentAsync(string id)
        {
            var students = await GetAllStudentsAsync();
            var student = students.FirstOrDefault(s => s.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (student == null)
                return ApiResponse<bool>.Failure("Student not found", $"No student with ID '{id}'.");

            students.Remove(student);
            await _csvDataService.WriteCsvToLocalAsync(students, CsvFileName);

            _logger.LogInformation("Student deleted: {StudentId}", id);
            return ApiResponse<bool>.Success(true, "Student deleted successfully.");
        }

        public string ConvertToCsv(List<Student> students)
        {
            return _csvDataService.ConvertToCsvString(students);
        }

        // --- Private Helpers ---

        private static string GenerateStudentId(List<Student> existing)
        {
            int max = existing
                .Select(s => int.TryParse(s.Id.Replace("S", ""), out int n) ? n : 0)
                .DefaultIfEmpty(0)
                .Max();
            return $"S{(max + 1):D3}";
        }
    }
}
