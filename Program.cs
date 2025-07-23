using Microsoft.EntityFrameworkCore;
using PhotoApi.Data;  // Change "PhotoApi" to your actual project name

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=photos.db"));

// Add CORS for frontend requests
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:4200", "http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// Add Swagger/OpenAPI
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Create appsettings.Development.json for database connection (development only)
if (app.Environment.IsDevelopment())
{
    var appSettingsDevPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json");
    if (!File.Exists(appSettingsDevPath))
    {
        var devSettings = """
        {
          "ConnectionStrings": {
            "DefaultConnection": "Data Source=photos.db"
          },
          "Logging": {
            "LogLevel": {
              "Default": "Information",
              "Microsoft.AspNetCore": "Warning"
            }
          }
        }
        """;
        File.WriteAllText(appSettingsDevPath, devSettings);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

// Create database on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

// Only seed data in development environment
    if (app.Environment.IsDevelopment())
    {
        DbSeeder.SeedData(context);
    }
}

app.Run();


// Make Program class public for testing
public partial class Program { }
