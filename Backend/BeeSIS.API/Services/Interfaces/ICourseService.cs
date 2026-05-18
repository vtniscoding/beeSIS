using BeeSIS.API.Models;

namespace BeeSIS.API.Services.Interfaces
{
    /// <summary>
    /// Interface for course business logic operations.
    /// SRP: Only course-related methods.
    /// ISP: Clients depend only on course methods they use.
    /// </summary>
    public interface ICourseService
    {
        Task<List<Course>> GetAllCoursesAsync();
        Task<Course?> GetCourseByCourseCodeAsync(string courseCode);
        Task<ApiResponse<Course>> AddCourseAsync(Course course);
        Task<ApiResponse<Course>> UpdateCourseAsync(string courseCode, Course course);
        Task<ApiResponse<bool>> DeleteCourseAsync(string courseCode);
        string ConvertToCsv(List<Course> courses);
    }
}
