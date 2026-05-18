using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using BeeSIS.API.Middleware;
using BeeSIS.API.Models;
using BeeSIS.API.Services.Interfaces;
using BeeSIS.API.Services.Implementations;
using BeeSIS.API.Validators;

// -----------------------------------------------------------------------
// Configure Serilog early so startup errors are captured
// -----------------------------------------------------------------------
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // --- Serilog ---
    builder.Host.UseSerilog((ctx, services, config) =>
        config.ReadFrom.Configuration(ctx.Configuration)
              .ReadFrom.Services(services)
              .Enrich.FromLogContext());

    // -----------------------------------------------------------------------
    // 1. Services Registration (DIP: register interfaces → implementations)
    // -----------------------------------------------------------------------

    // HttpClient for GitHub CSV reads
    builder.Services.AddHttpClient<ICsvDataService, CsvDataService>();

    // Business services — DIP: controllers depend on interfaces, not concretions
    builder.Services.AddScoped<IStudentService, StudentService>();
    builder.Services.AddScoped<ICourseService, CourseService>();
    builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
    builder.Services.AddScoped<IAuthService, AuthService>();

    // FluentValidation — Strategy Pattern: each entity has its own validator
    builder.Services.AddScoped<IValidator<Student>, StudentValidator>();
    builder.Services.AddScoped<IValidator<Course>, CourseValidator>();
    builder.Services.AddScoped<IValidator<LoginRequest>, LoginValidator>();

    // Controllers
    builder.Services.AddControllers();

    // -----------------------------------------------------------------------
    // 2. JWT Authentication
    // -----------------------------------------------------------------------
    var jwtSecret = builder.Configuration["Jwt:SecretKey"]
        ?? throw new InvalidOperationException("Jwt:SecretKey is not set in configuration.");

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                ClockSkew = TimeSpan.Zero // No clock skew — token expires exactly at ExpiresAt
            };
        });

    builder.Services.AddAuthorization();

    // -----------------------------------------------------------------------
    // 3. CORS
    // -----------------------------------------------------------------------
    var allowedOrigins = builder.Configuration
        .GetSection("Cors:AllowedOrigins")
        .Get<string[]>() ?? new[] { "http://localhost:5173", "http://localhost:3000" };

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("BeeSISCorsPolicy", policy =>
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
    });

    // -----------------------------------------------------------------------
    // 4. OpenAPI / Swagger
    // -----------------------------------------------------------------------
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddOpenApi();

    // -----------------------------------------------------------------------
    // Build the app
    // -----------------------------------------------------------------------
    var app = builder.Build();

    // -----------------------------------------------------------------------
    // 5. Middleware Pipeline (order matters!)
    // -----------------------------------------------------------------------

    // Global exception handler — must be first
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseSerilogRequestLogging(); // Structured HTTP request logging

    app.UseCors("BeeSISCorsPolicy");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // Health check endpoint
    app.MapGet("/health", () => Results.Ok(new
    {
        status = "healthy",
        timestamp = DateTime.UtcNow,
        version = "1.0.0"
    }));

    Log.Information("🐝 BeeSIS API starting on .NET 10...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "BeeSIS API terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}
