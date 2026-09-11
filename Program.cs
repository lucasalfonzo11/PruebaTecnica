using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Application.Addresses.Commands.CreateAddress;
using PruebaTecnica.Application.Addresses.Queries.GetUserAddresses;
using PruebaTecnica.Application.Addresses.Commands.UpdateAddress;
using PruebaTecnica.Application.Users.Commands.CreateUser;
using PruebaTecnica.Application.Users.Commands.DeleteUser;
using PruebaTecnica.Application.Users.Commands.UpdateUser;
using PruebaTecnica.Application.Users.Queries.GetUserById;
using PruebaTecnica.Application.Users.Queries.GetUsers;
using PruebaTecnica.Domain.Entities;
using PruebaTecnica.Endpoints;
using PruebaTecnica.Infrastructure.Persistence;
using PruebaTecnica.Middleware;

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
builder.Services.AddScoped<CreateAddressHandler>();
builder.Services.AddScoped<GetUserAddressesHandler>();
builder.Services.AddScoped<UpdateAddressHandler>();

var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();
app.MapGet("/", () => "Hello World!");
app.MapUsersEndpoints();
app.MapAddressEndpoints();
app.Run();
