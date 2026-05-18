using BeeSIS.API.Models;
using BeeSIS.API.Services.Interfaces;

namespace BeeSIS.API.Services.Implementations
{
    /// <summary>
    /// Implements course business logic.
    /// SRP: Only handles course operations.
    /// DIP: Depends on ICsvDataService interface abstraction.
    /// </summary>
    public class CourseService : ICourseService
    {
        private const string CsvFileName = "courses.csv";
        private readonly ICsvDataService _csvDataService;
        private readonly ILogger<CourseService> _logger;

        public CourseService(ICsvDataService csvDataService, ILogger<CourseService> logger)
        {
            _csvDataService = csvDataService;
            _logger = logger;
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            _logger.LogInformation("Fetching all courses");
            return await _csvDataService.ReadCsvFromGitHubAsync<Course>(CsvFileName);
        }

        public async Task<Course?> GetCourseByCourseCodeAsync(string courseCode)
        {
            var courses = await GetAllCoursesAsync();
            return courses.FirstOrDefault(c => c.CourseCode.Equals(courseCode, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<ApiResponse<Course>> AddCourseAsync(Course course)
        {
            var courses = await GetAllCoursesAsync();

            if (courses.Any(c => c.CourseCode.Equals(course.CourseCode, StringComparison.OrdinalIgnoreCase)))
                return ApiResponse<Course>.Failure("Course already exists", $"Course with code '{course.CourseCode}' already exists.");

            courses.Add(course);
            await _csvDataService.WriteCsvToLocalAsync(courses, CsvFileName);

            _logger.LogInformation("Course added: {CourseCode}", course.CourseCode);
            return ApiResponse<Course>.Success(course, "Course created successfully.");
        }

        public async Task<ApiResponse<Course>> UpdateCourseAsync(string courseCode, Course updatedCourse)
        {
            var courses = await GetAllCoursesAsync();
            var index = courses.FindIndex(c => c.CourseCode.Equals(courseCode, StringComparison.OrdinalIgnoreCase));

            if (index < 0)
                return ApiResponse<Course>.Failure("Course not found", $"No course with code '{courseCode}'.");

            updatedCourse.CourseCode = courseCode;
            courses[index] = updatedCourse;

            await _csvDataService.WriteCsvToLocalAsync(courses, CsvFileName);

            _logger.LogInformation("Course updated: {CourseCode}", courseCode);
            return ApiResponse<Course>.Success(updatedCourse, "Course updated successfully.");
        }

        public async Task<ApiResponse<bool>> DeleteCourseAsync(string courseCode)
        {
            var courses = await GetAllCoursesAsync();
            var course = courses.FirstOrDefault(c => c.CourseCode.Equals(courseCode, StringComparison.OrdinalIgnoreCase));

            if (course == null)
                return ApiResponse<bool>.Failure("Course not found", $"No course with code '{courseCode}'.");

            courses.Remove(course);
            await _csvDataService.WriteCsvToLocalAsync(courses, CsvFileName);

            _logger.LogInformation("Course deleted: {CourseCode}", courseCode);
            return ApiResponse<bool>.Success(true, "Course deleted successfully.");
        }

        public string ConvertToCsv(List<Course> courses)
        {
            return _csvDataService.ConvertToCsvString(courses);
        }
    }
}
