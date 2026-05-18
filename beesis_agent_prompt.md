# 🎯 BeeSIS Project - Complete Development Prompt for AI Agent

## PROJECT OVERVIEW

Build a **Student Information Management System (BeeSIS)** with:

- **Backend**: ASP.NET Core API (self-contained executable)
- **Frontend**: React Single Page Application
- **Data Storage**: CSV files on GitHub (Cloud-based)
- **Architecture**: SOLID Principles + Design Patterns
- **Deployment**: No server hosting required - executable runs anywhere

---

## 📋 REQUIREMENTS

### Functional Requirements

1. **Student Registration**
   - Add new student with personal details (ID, name, email, date of birth,
     phone, address)
   - Store academic records (major, enrollment date, GPA)
   - Edit/Delete student information
   - View all students

2. **Course Management**
   - Admin adds/edits courses (course code, name, credits, instructor)
   - Assign students to courses
   - View course enrollment
   - Update course status (active/inactive)

3. **User Authentication & Authorization**
   - Login system for Student, Faculty, Admin roles
   - Role-Based Access Control (RBAC)
   - JWT token authentication
   - Logout functionality

### Non-Functional Requirements

- **Performance**: Response time < 1 second for all operations
- **Security**: Password hashing, secure token handling
- **Usability**: Clean, responsive UI (mobile-friendly)
- **Reliability**: Error handling and validation
- **Accessibility**: Follow basic accessibility standards

---

## 🏗️ PROJECT STRUCTURE

```
BeeSIS-App/
├── Backend/
│   ├── BeeSIS.API/
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs
│   │   │   ├── StudentController.cs
│   │   │   ├── CourseController.cs
│   │   │   └── EnrollmentController.cs
│   │   ├── Models/
│   │   │   ├── Student.cs
│   │   │   ├── Course.cs
│   │   │   ├── User.cs
│   │   │   ├── Enrollment.cs
│   │   │   ├── LoginRequest.cs
│   │   │   └── ApiResponse.cs
│   │   ├── Services/
│   │   │   ├── Interfaces/
│   │   │   │   ├── IStudentService.cs
│   │   │   │   ├── ICourseService.cs
│   │   │   │   ├── IAuthService.cs
│   │   │   │   ├── IEnrollmentService.cs
│   │   │   │   └── ICsvDataService.cs
│   │   │   ├── Implementations/
│   │   │   │   ├── StudentService.cs
│   │   │   │   ├── CourseService.cs
│   │   │   │   ├── AuthService.cs
│   │   │   │   ├── EnrollmentService.cs
│   │   │   │   └── CsvDataService.cs
│   │   ├── Validators/
│   │   │   ├── StudentValidator.cs
│   │   │   ├── CourseValidator.cs
│   │   │   └── LoginValidator.cs
│   │   ├── Middleware/
│   │   │   ├── ExceptionHandlingMiddleware.cs
│   │   │   └── AuthenticationMiddleware.cs
│   │   ├── appsettings.json
│   │   ├── appsettings.Development.json
│   │   ├── Program.cs
│   │   ├── Startup.cs
│   │   └── BeeSIS.API.csproj
│   └── BeeSIS.API.sln
│
├── Frontend/
│   ├── beesis-react-app/
│   │   ├── public/
│   │   │   ├── index.html
│   │   │   └── favicon.ico
│   │   ├── src/
│   │   │   ├── components/
│   │   │   │   ├── Auth/
│   │   │   │   │   ├── LoginPage.jsx
│   │   │   │   │   ├── LogoutButton.jsx
│   │   │   │   │   └── ProtectedRoute.jsx
│   │   │   │   ├── Students/
│   │   │   │   │   ├── StudentList.jsx
│   │   │   │   │   ├── StudentForm.jsx
│   │   │   │   │   ├── StudentDetail.jsx
│   │   │   │   │   └── DownloadStudentsCsv.jsx
│   │   │   │   ├── Courses/
│   │   │   │   │   ├── CourseList.jsx
│   │   │   │   │   ├── CourseForm.jsx
│   │   │   │   │   └── CourseDetail.jsx
│   │   │   │   ├── Enrollment/
│   │   │   │   │   ├── EnrollmentList.jsx
│   │   │   │   │   └── EnrollmentForm.jsx
│   │   │   │   ├── Dashboard/
│   │   │   │   │   └── Dashboard.jsx
│   │   │   │   ├── Navigation/
│   │   │   │   │   ├── Navbar.jsx
│   │   │   │   │   └── Sidebar.jsx
│   │   │   │   └── Common/
│   │   │   │       ├── Loading.jsx
│   │   │   │       └── ErrorMessage.jsx
│   │   │   ├── services/
│   │   │   │   ├── api.js
│   │   │   │   ├── authService.js
│   │   │   │   ├── studentService.js
│   │   │   │   ├── courseService.js
│   │   │   │   └── enrollmentService.js
│   │   │   ├── context/
│   │   │   │   └── AuthContext.jsx
│   │   │   ├── pages/
│   │   │   │   ├── HomePage.jsx
│   │   │   │   ├── NotFoundPage.jsx
│   │   │   │   └── UnauthorizedPage.jsx
│   │   │   ├── styles/
│   │   │   │   ├── App.css
│   │   │   │   ├── index.css
│   │   │   │   └── variables.css
│   │   │   ├── App.jsx
│   │   │   ├── App.css
│   │   │   └── index.jsx
│   │   ├── .env.example
│   │   ├── package.json
│   │   ├── vite.config.js
│   │   └── .gitignore
│
├── Data/
│   ├── students.csv
│   ├── courses.csv
│   ├── users.csv
│   └── enrollments.csv
│
├── Documentation/
│   ├── ARCHITECTURE.md
│   ├── API_DOCUMENTATION.md
│   ├── SETUP_GUIDE.md
│   ├── SOLID_PRINCIPLES.md
│   ├── DEPLOYMENT.md
│   └── DIAGRAMS/
│       ├── UseCase_Diagram.png
│       ├── ClassDiagram.png
│       └── PackageDiagram.png
│
├── .gitignore
├── README.md
└── CONTRIBUTING.md
```

---

## 🔧 TECHNOLOGY STACK

### Backend

- **Framework**: ASP.NET Core 8.0
- **Language**: C#
- **CSV Processing**: CsvHelper NuGet package
- **HTTP Client**: Built-in HttpClient
- **Authentication**: JWT (System.IdentityModel.Tokens.Jwt)
- **Validation**: FluentValidation NuGet
- **Logging**: Serilog
- **CORS**: Built-in CORS middleware

### Frontend

- **Framework**: React 18+
- **Build Tool**: Vite
- **HTTP Client**: Axios
- **Styling**: CSS3 + Tailwind CSS (optional)
- **State Management**: React Context API
- **Routing**: React Router v6

### Cloud

- **CSV Storage**: GitHub Repository (Raw Content URL)
- **Version Control**: Git

---

## 📝 CSV FILE SCHEMAS

### students.csv

```
Id,FirstName,LastName,Email,Phone,DateOfBirth,Address,Major,EnrollmentDate,GPA,Status
S001,John,Doe,john.doe@university.edu,0123456789,1999-05-15,123 Main St,Computer Science,2023-09-01,3.5,Active
S002,Jane,Smith,jane.smith@university.edu,0987654321,2000-03-20,456 Oak Ave,Business Administration,2023-09-01,3.8,Active
```

### courses.csv

```
CourseCode,CourseName,Credits,Instructor,Department,Semester,Status
CS101,Introduction to Programming,3,Dr. Smith,Computer Science,Fall2024,Active
CS201,Data Structures,3,Dr. Johnson,Computer Science,Fall2024,Active
BA101,Business Fundamentals,3,Prof. Williams,Business,Fall2024,Active
```

### users.csv

```
Id,Username,Email,PasswordHash,Role,CreatedDate,Status
U001,admin,admin@university.edu,hashed_password,Admin,2024-01-01,Active
U002,student1,john.doe@university.edu,hashed_password,Student,2024-01-01,Active
U003,faculty1,dr.smith@university.edu,hashed_password,Faculty,2024-01-01,Active
```

### enrollments.csv

```
EnrollmentId,StudentId,CourseCode,EnrollmentDate,Grade,Status
E001,S001,CS101,2024-01-15,,Active
E002,S001,CS201,2024-01-15,,Active
E003,S002,BA101,2024-01-15,,Active
```

---

## 🏛️ SOLID PRINCIPLES IMPLEMENTATION

### Single Responsibility Principle (SRP)

```
✓ StudentService - Only handles student business logic
✓ CourseService - Only handles course business logic
✓ CsvDataService - Only handles CSV read/write operations
✓ AuthService - Only handles authentication logic
✓ Each controller handles one resource type
```

### Open/Closed Principle (OCP)

```
✓ Services use interfaces (IStudentService, ICourseService, etc.)
✓ Validators are extensible
✓ Middleware chain is extensible
✓ Can add new features without modifying existing code
```

### Liskov Substitution Principle (LSP)

```
✓ All services implement their interfaces correctly
✓ Controllers can substitute service implementations
✓ Derived validators can replace base validators
```

### Interface Segregation Principle (ISP)

```
✓ IStudentService - Only student-related methods
✓ ICsvDataService - Only CSV operations
✓ IAuthService - Only authentication methods
✓ Clients depend only on methods they use
```

### Dependency Inversion Principle (DIP)

```
✓ Constructor Dependency Injection
✓ Controllers depend on service interfaces, not implementations
✓ Services depend on abstraction (interfaces)
✓ Use ConfigureServices in Program.cs for DI setup
```

---

## 🏗️ ARCHITECTURE DESIGN

### Design Patterns to Implement

1. **Factory Pattern** - User creation based on role
2. **Repository Pattern** - CSV data access
3. **Service Pattern** - Business logic layer
4. **Strategy Pattern** - Different validation strategies
5. **Adapter Pattern** - CSV to object conversion

### Layered Architecture

```
┌─────────────────────────────────┐
│   Presentation Layer (React)    │
│   - UI Components               │
│   - User Interactions           │
└────────────────┬────────────────┘
                 │ HTTP/REST
┌────────────────▼────────────────┐
│   API Layer (.NET Controllers)  │
│   - Request handling            │
│   - Response formatting         │
└────────────────┬────────────────┘
                 │
┌────────────────▼────────────────┐
│   Business Logic Layer          │
│   - Services                    │
│   - Validation                  │
│   - Authentication              │
└────────────────┬────────────────┘
                 │
┌────────────────▼────────────────┐
│   Data Access Layer             │
│   - CsvDataService              │
│   - GitHub API integration      │
└────────────────┬────────────────┘
                 │
┌────────────────▼────────────────┐
│   Data Source                   │
│   - GitHub CSV Files            │
└─────────────────────────────────┘
```

---

## 🔐 SECURITY REQUIREMENTS

- [ ] Password hashing with BCrypt
- [ ] JWT token with expiration (15 minutes)
- [ ] CORS properly configured
- [ ] Input validation on all endpoints
- [ ] SQL injection protection (N/A for CSV but prevent path traversal)
- [ ] Error messages don't leak sensitive info
- [ ] Sensitive data not logged
- [ ] HTTPS recommended for production

---

## 📋 IMPLEMENTATION CHECKLIST

### Backend (.NET Core)

- [ ] Create ASP.NET Core Web API project
- [ ] Define Models (Student, Course, User, Enrollment)
- [ ] Create Service Interfaces (DIP)
- [ ] Implement Services with business logic
- [ ] Create Validators (FluentValidation)
- [ ] Create Controllers with proper HTTP methods
- [ ] Setup Dependency Injection in Program.cs
- [ ] Implement JWT Authentication
- [ ] Create CsvDataService for GitHub integration
- [ ] Add error handling middleware
- [ ] Add CORS middleware
- [ ] Setup logging (Serilog)
- [ ] Write unit tests for services
- [ ] Create appsettings.json with GitHub config
- [ ] Build self-contained executable

### Frontend (React)

- [ ] Initialize React project with Vite
- [ ] Create authentication context
- [ ] Create login/logout pages
- [ ] Create student CRUD components
- [ ] Create course CRUD components
- [ ] Create enrollment components
- [ ] Setup API client with axios
- [ ] Implement protected routes
- [ ] Create navigation/sidebar
- [ ] Add responsive styling (Tailwind CSS)
- [ ] Add loading states and error handling
- [ ] Create CSV download functionality
- [ ] Setup environment variables (.env)
- [ ] Build and test production bundle

### GitHub Repository

- [ ] Create repository "SIMS-App"
- [ ] Setup .gitignore properly
- [ ] Upload CSV template files to /Data
- [ ] Create README.md with setup instructions
- [ ] Create CONTRIBUTING.md
- [ ] Setup GitHub Pages (optional)

### Documentation

- [ ] Write ARCHITECTURE.md
- [ ] Write API_DOCUMENTATION.md with all endpoints
- [ ] Write SETUP_GUIDE.md with step-by-step instructions
- [ ] Write SOLID_PRINCIPLES.md explaining implementation
- [ ] Create Use Case diagram
- [ ] Create Class diagram
- [ ] Create Package diagram
- [ ] Document design patterns used

---

## 🚀 BUILD & DEPLOYMENT COMMANDS

### Backend

```bash
# Restore dependencies
dotnet restore

# Build project
dotnet build -c Release

# Run locally
dotnet run

# Build self-contained executable (Windows 64-bit)
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
# Output: bin/Release/net8.0/win-x64/publish/SIMS.API.exe

# Build for Linux
dotnet publish -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true

# Build for macOS
dotnet publish -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true
```

### Frontend

```bash
# Install dependencies
npm install

# Run development server
npm run dev

# Build for production
npm run build

# Preview production build
npm run preview
```

---

## 🔗 API ENDPOINTS REQUIRED

### Authentication

- `POST /api/auth/login` - Login with username/password
- `POST /api/auth/logout` - Logout user
- `GET /api/auth/me` - Get current user info

### Students

- `GET /api/students` - Get all students
- `GET /api/students/{id}` - Get student by ID
- `POST /api/students` - Create new student
- `PUT /api/students/{id}` - Update student
- `DELETE /api/students/{id}` - Delete student
- `GET /api/students/download` - Download students CSV

### Courses

- `GET /api/courses` - Get all courses
- `GET /api/courses/{code}` - Get course by code
- `POST /api/courses` - Create new course
- `PUT /api/courses/{code}` - Update course
- `DELETE /api/courses/{code}` - Delete course
- `GET /api/courses/download` - Download courses CSV

### Enrollments

- `GET /api/enrollments` - Get all enrollments
- `GET /api/enrollments/student/{studentId}` - Get student enrollments
- `POST /api/enrollments` - Create enrollment
- `PUT /api/enrollments/{id}` - Update enrollment
- `DELETE /api/enrollments/{id}` - Delete enrollment

---

## 📊 EXAMPLE CODE SNIPPETS

### CsvDataService.cs (Key Service)

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CsvHelper;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace SIMS.API.Services.Implementations
{
    public class CsvDataService : ICsvDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CsvDataService> _logger;
        private readonly string _gitHubRawUrl = 
            "https://raw.githubusercontent.com/{owner}/{repo}/main/Data";

        public CsvDataService(HttpClient httpClient, ILogger<CsvDataService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // Read CSV from GitHub
        public async Task<List<T>> ReadCsvFromGitHubAsync<T>(string fileName) where T : class
        {
            try
            {
                var url = $"{_gitHubRawUrl}/{fileName}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                
                var content = await response.Content.ReadAsStringAsync();
                using (var reader = new StringReader(content))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<T>();
                    return new List<T>(records);
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"GitHub API error: {ex.Message}");
                throw;
            }
        }

        // Write CSV locally
        public async Task WriteCsvToLocalAsync<T>(List<T> records, string fileName) where T : class
        {
            try
            {
                var filePath = Path.Combine("Data", fileName);
                Directory.CreateDirectory("Data");
                
                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    await csv.WriteRecordsAsync(records);
                }
                _logger.LogInformation($"CSV saved to {filePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error writing CSV: {ex.Message}");
                throw;
            }
        }
    }
}
```

### StudentController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using SIMS.API.Services;
using SIMS.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ICsvDataService _csvDataService;

        public StudentsController(
            IStudentService studentService,
            ICsvDataService csvDataService)
        {
            _studentService = studentService;
            _csvDataService = csvDataService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(string id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound(new { message = "Student not found" });
            return Ok(student);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Student>> CreateStudent([FromBody] Student student)
        {
            var result = await _studentService.AddStudentAsync(student);
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Errors });

            return CreatedAtAction(nameof(GetStudentById), new { id = result.Data.Id }, result.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStudent(string id, [FromBody] Student student)
        {
            var result = await _studentService.UpdateStudentAsync(id, student);
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Errors });

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            var result = await _studentService.DeleteStudentAsync(id);
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Errors });

            return NoContent();
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadStudentsCsv()
        {
            var students = await _studentService.GetAllStudentsAsync();
            var csv = _studentService.ConvertToCsv(students);
            
            return File(
                System.Text.Encoding.UTF8.GetBytes(csv),
                "text/csv",
                "students.csv"
            );
        }
    }
}
```

### React Component Example - LoginPage.jsx

```jsx
import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

const LoginPage = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();
    const { login } = useAuth();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError("");

        try {
            const response = await axios.post(
                "http://localhost:5000/api/auth/login",
                {
                    username,
                    password,
                },
            );

            const { token, user } = response.data;
            login(token, user);
            navigate("/dashboard");
        } catch (err) {
            setError(err.response?.data?.message || "Login failed");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="login-container">
            <form onSubmit={handleSubmit}>
                <h2>SIMS Login</h2>
                {error && <div className="error-message">{error}</div>}
                <input
                    type="text"
                    placeholder="Username"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                    required
                />
                <input
                    type="password"
                    placeholder="Password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />
                <button type="submit" disabled={loading}>
                    {loading ? "Logging in..." : "Login"}
                </button>
            </form>
        </div>
    );
};

export default LoginPage;
```

---

## 🎯 GITHUB SETUP

1. Create repository: `https://github.com/YOUR_USERNAME/SIMS-App`
2. Clone locally: `git clone https://github.com/YOUR_USERNAME/SIMS-App.git`
3. Add files to `/Data`:
   - students.csv
   - courses.csv
   - users.csv
   - enrollments.csv
4. Push initial commit
5. Get raw content URLs:
   - `https://raw.githubusercontent.com/YOUR_USERNAME/SIMS-App/main/Data/students.csv`

---

## 📝 CONFIGURATION FILES

### appsettings.json

```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information"
        }
    },
    "AllowedHosts": "*",
    "GitHub": {
        "Owner": "YOUR_USERNAME",
        "Repository": "SIMS-App",
        "RawContentUrl": "https://raw.githubusercontent.com/YOUR_USERNAME/SIMS-App/main/Data"
    },
    "Jwt": {
        "SecretKey": "your-super-secret-key-min-32-characters-long",
        "Issuer": "SIMS-App",
        "Audience": "SIMS-Users",
        "ExpirationMinutes": 15
    },
    "Cors": {
        "AllowedOrigins": [
            "http://localhost:5173",
            "http://localhost:3000"
        ]
    }
}
```

### .env (React)

```
VITE_API_BASE_URL=http://localhost:5000/api
VITE_APP_NAME=SIMS
```

---

## ✅ SUCCESS CRITERIA

After agent completion:

- [ ] All 4 CSV files exist with sample data in GitHub
- [ ] Backend API runs successfully on localhost:5000
- [ ] All endpoints return correct responses
- [ ] Frontend React app runs on localhost:5173/3000
- [ ] Login functionality works with JWT
- [ ] Student CRUD operations work end-to-end
- [ ] Course CRUD operations work end-to-end
- [ ] Enrollment functionality works
- [ ] CSV download feature works
- [ ] Self-contained .exe builds successfully
- [ ] Executable runs on different machine
- [ ] All SOLID principles applied
- [ ] Code is well-documented
- [ ] Diagrams are created (UseCase, Class, Package)
- [ ] README with setup instructions exists

---

## 📚 DELIVERABLES

1. **Complete Backend Code** with all services, controllers, models
2. **Complete Frontend Code** with all components and pages
3. **CSV Template Files** with sample data
4. **GitHub Repository** with all source code
5. **Self-Contained Executable** (SIMS.API.exe)
6. **Documentation**:
   - ARCHITECTURE.md
   - API_DOCUMENTATION.md
   - SETUP_GUIDE.md
   - SOLID_PRINCIPLES.md
7. **Diagrams**:
   - Use Case Diagram
   - Class Diagram
   - Package Diagram
8. **README.md** with quick start guide

---

## 🎓 NOTES FOR ACADEMIC ASSIGNMENT

This implementation demonstrates:

- ✓ SOLID Principles in action (all 5)
- ✓ Clean Coding Techniques
- ✓ Design Patterns (Factory, Strategy, Repository)
- ✓ Proper layered architecture
- ✓ Security best practices
- ✓ Data structure & algorithm efficiency
- ✓ Professional code organization
- ✓ Proper error handling & validation

---

## 🔗 REFERENCES

### Documentation Links

- ASP.NET Core: https://docs.microsoft.com/aspnet/core
- React: https://react.dev
- CsvHelper: https://joshclose.github.io/CsvHelper/
- JWT: https://tools.ietf.org/html/rfc7519
- GitHub API: https://docs.github.com/en/rest

### SOLID Principles

- https://en.wikipedia.org/wiki/SOLID
- https://www.baeldung.com/solid-principles
- https://refactoring.guru/design-patterns

---

**Generate this complete SIMS application following all specifications above.**
