using Todolist.Middleware;
using Todolist.Repository;
using Todolist.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add CORS policy
builder.Services.AddCors(options =>
{
    // Define a CORS policy
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

// Register services and repositories
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<ITodoRepository, TodoRepo>();
var app = builder.Build();

// Global exception handling (must be first in pipeline)
app.UseExceptionHandling();

// using CORS 
app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// health check 
// app.UseHttpsRedirection();
app.MapGet("/ping", () =>
{
    Dictionary<String, String> res = new Dictionary<string, string>();
    res.Add("message", "pong");
    return res;
});

app.MapControllers();
await app.RunAsync();

