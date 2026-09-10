using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PruebaTecnica.Endpoints;
using PruebaTecnica.Middleware;
using PruebaTecnica.Domain.Entities;
using PruebaTecnica.Infrastructure.Persistence;
using PruebaTecnica.Application.Users.Queries.GetUsers;
using PruebaTecnica.Application.Users.Commands.UpdateUser;
using PruebaTecnica.Application.Users.Commands.CreateUser;
using PruebaTecnica.Application.Users.Queries.GetUserById;
using PruebaTecnica.Application.Users.Commands.DeleteUser;

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
builder.Services.AddScoped<UpdateUserHandler>();
builder.Services.AddScoped<DeleteUserHandler>();

var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();
app.MapGet("/", () => "Hello World!");
app.MapUsersEndpoints();
app.Run();
