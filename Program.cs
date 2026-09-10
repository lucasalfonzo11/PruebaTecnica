using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Application.Users.Queries.GetUsers;
using PruebaTecnica.Domain.Entities;
using PruebaTecnica.Infrastructure.Persistence;
using PruebaTecnica.Middleware;
using FluentValidation;
using PruebaTecnica.Application.Users.Commands.CreateUser;
using Microsoft.AspNetCore.Identity;
using PruebaTecnica.Endpoints;
using PruebaTecnica.Application.Users.Queries.GetUserById;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConection");

if (string.IsNullOrWhiteSpace(connectionString)){
    throw new InvalidOperationException("DefaultConection configuration is required.");
}

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<CreateUserHandler>();
builder.Services.AddScoped<GetUserByIdHandler>();
builder.Services.AddScoped<GetUsersHandler>();

var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();
app.MapGet("/", () => "Hello World!");
app.MapUsersEndpoints();
app.Run();
