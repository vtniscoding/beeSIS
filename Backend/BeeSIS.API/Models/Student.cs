namespace BeeSIS.API.Models
{
    /// <summary>
    /// Represents a student entity in the system.
    /// </summary>
    public class Student
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Major { get; set; } = string.Empty;
        public string EnrollmentDate { get; set; } = string.Empty;
        public double GPA { get; set; }
        public string Status { get; set; } = "Active";
    }
}
