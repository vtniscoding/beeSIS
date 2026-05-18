namespace BeeSIS.API.Models
{
    /// <summary>
    /// Represents a user account for authentication and authorization.
    /// </summary>
    public class User
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Student"; // Admin, Faculty, Student
        public string CreatedDate { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
    }
}
