namespace BeeSIS.API.Models
{
    /// <summary>
    /// Represents a course entity in the system.
    /// </summary>
    public class Course
    {
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string Instructor { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Semester { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
    }
}
