using BeeSIS.API.Models;
using BeeSIS.API.Services.Interfaces;

namespace BeeSIS.API.Services.Implementations
{
    /// <summary>
    /// Implements enrollment business logic.
    /// SRP: Only handles enrollment operations.
    /// DIP: Depends on ICsvDataService abstraction.
    /// </summary>
    public class EnrollmentService : IEnrollmentService
    {
        private const string CsvFileName = "enrollments.csv";
        private readonly ICsvDataService _csvDataService;
        private readonly ILogger<EnrollmentService> _logger;

        public EnrollmentService(ICsvDataService csvDataService, ILogger<EnrollmentService> logger)
        {
            _csvDataService = csvDataService;
            _logger = logger;
        }

        public async Task<List<Enrollment>> GetAllEnrollmentsAsync()
        {
            _logger.LogInformation("Fetching all enrollments");
            return await _csvDataService.ReadCsvFromGitHubAsync<Enrollment>(CsvFileName);
        }

        public async Task<List<Enrollment>> GetEnrollmentsByStudentIdAsync(string studentId)
        {
            var enrollments = await GetAllEnrollmentsAsync();
            return enrollments.Where(e => e.StudentId.Equals(studentId, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public async Task<Enrollment?> GetEnrollmentByIdAsync(string enrollmentId)
        {
            var enrollments = await GetAllEnrollmentsAsync();
            return enrollments.FirstOrDefault(e => e.EnrollmentId.Equals(enrollmentId, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<ApiResponse<Enrollment>> AddEnrollmentAsync(Enrollment enrollment)
        {
            var enrollments = await GetAllEnrollmentsAsync();

            // Check for duplicate enrollment (same student + course)
            if (enrollments.Any(e =>
                e.StudentId.Equals(enrollment.StudentId, StringComparison.OrdinalIgnoreCase) &&
                e.CourseCode.Equals(enrollment.CourseCode, StringComparison.OrdinalIgnoreCase) &&
                e.Status.Equals("Active", StringComparison.OrdinalIgnoreCase)))
            {
                return ApiResponse<Enrollment>.Failure("Duplicate enrollment",
                    $"Student '{enrollment.StudentId}' is already enrolled in course '{enrollment.CourseCode}'.");
            }

            // Auto-generate ID if not provided
            if (string.IsNullOrWhiteSpace(enrollment.EnrollmentId))
                enrollment.EnrollmentId = GenerateEnrollmentId(enrollments);

            if (string.IsNullOrWhiteSpace(enrollment.EnrollmentDate))
                enrollment.EnrollmentDate = DateTime.Today.ToString("yyyy-MM-dd");

            enrollments.Add(enrollment);
            await _csvDataService.WriteCsvToLocalAsync(enrollments, CsvFileName);

            _logger.LogInformation("Enrollment added: {EnrollmentId}", enrollment.EnrollmentId);
            return ApiResponse<Enrollment>.Success(enrollment, "Enrollment created successfully.");
        }

        public async Task<ApiResponse<Enrollment>> UpdateEnrollmentAsync(string enrollmentId, Enrollment updatedEnrollment)
        {
            var enrollments = await GetAllEnrollmentsAsync();
            var index = enrollments.FindIndex(e => e.EnrollmentId.Equals(enrollmentId, StringComparison.OrdinalIgnoreCase));

            if (index < 0)
                return ApiResponse<Enrollment>.Failure("Enrollment not found", $"No enrollment with ID '{enrollmentId}'.");

            updatedEnrollment.EnrollmentId = enrollmentId;
            enrollments[index] = updatedEnrollment;

            await _csvDataService.WriteCsvToLocalAsync(enrollments, CsvFileName);

            _logger.LogInformation("Enrollment updated: {EnrollmentId}", enrollmentId);
            return ApiResponse<Enrollment>.Success(updatedEnrollment, "Enrollment updated successfully.");
        }

        public async Task<ApiResponse<bool>> DeleteEnrollmentAsync(string enrollmentId)
        {
            var enrollments = await GetAllEnrollmentsAsync();
            var enrollment = enrollments.FirstOrDefault(e => e.EnrollmentId.Equals(enrollmentId, StringComparison.OrdinalIgnoreCase));

            if (enrollment == null)
                return ApiResponse<bool>.Failure("Enrollment not found", $"No enrollment with ID '{enrollmentId}'.");

            enrollments.Remove(enrollment);
            await _csvDataService.WriteCsvToLocalAsync(enrollments, CsvFileName);

            _logger.LogInformation("Enrollment deleted: {EnrollmentId}", enrollmentId);
            return ApiResponse<bool>.Success(true, "Enrollment deleted successfully.");
        }

        // --- Private Helpers ---

        private static string GenerateEnrollmentId(List<Enrollment> existing)
        {
            int max = existing
                .Select(e => int.TryParse(e.EnrollmentId.Replace("E", ""), out int n) ? n : 0)
                .DefaultIfEmpty(0)
                .Max();
            return $"E{(max + 1):D3}";
        }
    }
}
