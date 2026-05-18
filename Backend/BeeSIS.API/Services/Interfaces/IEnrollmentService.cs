using BeeSIS.API.Models;

namespace BeeSIS.API.Services.Interfaces
{
    /// <summary>
    /// Interface for enrollment business logic operations.
    /// SRP: Only enrollment-related methods.
    /// </summary>
    public interface IEnrollmentService
    {
        Task<List<Enrollment>> GetAllEnrollmentsAsync();
        Task<List<Enrollment>> GetEnrollmentsByStudentIdAsync(string studentId);
        Task<Enrollment?> GetEnrollmentByIdAsync(string enrollmentId);
        Task<ApiResponse<Enrollment>> AddEnrollmentAsync(Enrollment enrollment);
        Task<ApiResponse<Enrollment>> UpdateEnrollmentAsync(string enrollmentId, Enrollment enrollment);
        Task<ApiResponse<bool>> DeleteEnrollmentAsync(string enrollmentId);
    }
}
