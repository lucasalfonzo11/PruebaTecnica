using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Infrastructure.Persistence;
using PruebaTecnica.Middleware;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConection");

if (string.IsNullOrWhiteSpace(connectionString)){
    throw new InvalidOperationException("DefaultConection configuration is required.");
}
builder.Services.AddDbContext<AppDbContext>( options => options.UseSqlite(connectionString));

var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();
app.MapGet("/", () => "Hello World!");
app.Run();
