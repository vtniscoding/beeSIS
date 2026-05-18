namespace BeeSIS.API.Models
{
    /// <summary>
    /// Represents a student-course enrollment relationship.
    /// </summary>
    public class Enrollment
    {
        public string EnrollmentId { get; set; } = string.Empty;
        public string StudentId { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public string EnrollmentDate { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
    }
}
