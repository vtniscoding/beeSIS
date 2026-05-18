using BeeSIS.API.Models;

namespace BeeSIS.API.Services.Interfaces
{
    /// <summary>
    /// Interface for student business logic operations.
    /// SRP: Only student-related methods.
    /// ISP: Clients depend only on student methods they use.
    /// </summary>
    public interface IStudentService
    {
        Task<List<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(string id);
        Task<ApiResponse<Student>> AddStudentAsync(Student student);
        Task<ApiResponse<Student>> UpdateStudentAsync(string id, Student student);
        Task<ApiResponse<bool>> DeleteStudentAsync(string id);
        string ConvertToCsv(List<Student> students);
    }
}
