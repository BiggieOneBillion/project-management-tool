using Project.INFRASTRUCTURE;
using Project.APPLICATION;
using Project.INFRASTRUCTURE.Data;

var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// SERVICE CONFIGURATION
// ============================================================================

// Add Controllers
builder.Services.AddControllers();

// Add API Documentation (Swagger)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Project Management API",
        Version = "v1",
        Description = "RESTful API for Project Management Platform with CQRS architecture",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Project Management Team"
        }
    });
});

// ============================================================================
// LAYER DEPENDENCIES
// ============================================================================

// APPLICATION Layer - Registers:
// - MediatR (CQRS Commands & Queries)
// - FluentValidation (Input validation)
// - AutoMapper (Entity-to-DTO mapping)
// - Authentication Services (JWT, Password)
builder.Services.AddApplication();

// INFRASTRUCTURE Layer - Registers:
// - DbContext (Entity Framework Core)
// - Repositories (Data access)
// - Database connection
builder.Services.AddInfrastructure(builder.Configuration);

// ============================================================================
// AUTHENTICATION & AUTHORIZATION
// ============================================================================

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"] ?? 
                    throw new InvalidOperationException("JWT Secret not configured"))
            ),
            ClockSkew = TimeSpan.Zero // Remove default 5 minute tolerance
        };
    });

builder.Services.AddAuthorization();

// ============================================================================
// CORS CONFIGURATION
// ============================================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // React/Vite frontend
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Allow cookies if needed
    });
});

// ============================================================================
// BUILD APPLICATION
// ============================================================================

var app = builder.Build();

// ============================================================================
// DATABASE INITIALIZATION
// ============================================================================

// Seed database with initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        
        logger.LogInformation("Seeding database...");
        DbInitializer.Seed(context);
        logger.LogInformation("Database seeded successfully.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
        throw; // Re-throw to prevent app from starting with bad data
    }
}

// ============================================================================
// HTTP REQUEST PIPELINE
// ============================================================================

// Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Project Management API v1");
        options.RoutePrefix = string.Empty; // Serve Swagger UI at root (http://localhost:5000/)
    });
}

// Enable CORS
app.UseCors("AllowFrontend");

// Enable HTTPS Redirection (Production)
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Enable Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map Controllers
app.MapControllers();

// ============================================================================
// RUN APPLICATION
// ============================================================================

app.Logger.LogInformation("Starting Project Management API...");
app.Logger.LogInformation("CQRS Architecture: MediatR + FluentValidation + AutoMapper");
app.Logger.LogInformation("Swagger UI available at: http://localhost:5000/");
app.Logger.LogInformation("API Base URL: http://localhost:5000/api/v1");

app.Run();

