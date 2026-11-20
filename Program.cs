using StudentManagementApi.Middleware;
using StudentManagementApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Register services (dependency injection)
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IStudentService, StudentService>();

var app = builder.Build();

// Middleware pipeline - the order matters:
// request  ->  logging  ->  exception handling  ->  controllers
// response <-  logging  <-  exception handling  <-  controllers
app.UseRequestLogging();
app.UseExceptionHandling();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();   // OpenAPI document at /openapi/v1.json
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
