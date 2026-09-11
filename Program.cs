using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using PruebaTecnica.Application.Addresses.Commands.CreateAddress;
using PruebaTecnica.Application.Addresses.Queries.GetUserAddresses;
using PruebaTecnica.Application.Addresses.Commands.UpdateAddress;
using PruebaTecnica.Application.Currencies.Commands.CreateCurrency;
using PruebaTecnica.Application.Currencies.Queries.GetCurrencies;
using PruebaTecnica.Application.CurrencyConversion;
using PruebaTecnica.Application.Users.Commands.CreateUser;
using PruebaTecnica.Application.Users.Commands.DeleteUser;
using PruebaTecnica.Application.Users.Commands.UpdateUser;
using PruebaTecnica.Application.Users.Queries.GetUserById;
using PruebaTecnica.Application.Users.Queries.GetUsers;
using PruebaTecnica.Domain.Entities;
using PruebaTecnica.Endpoints;
using PruebaTecnica.Infrastructure.Persistence;
using PruebaTecnica.Middleware;
using PruebaTecnica.Application.Addresses.Commands.DeleteAddress;

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
builder.Services.AddScoped<DeleteAddressHandler>();
builder.Services.AddScoped<CreateCurrencyHandler>();
builder.Services.AddScoped<GetCurrenciesHandler>();
builder.Services.AddScoped<ConvertCurrencyHandler>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>{
    options.SwaggerDoc("v1", new OpenApiInfo{
        Title = "Prueba Tecnica API",
        Version = "v1"
    });

    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme{
        Name = "X-API-KEY",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Enter the API key."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement{
        [new OpenApiSecuritySchemeReference("ApiKey", document)] = []
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.RoutePrefix = "swagger");
}

app.UseMiddleware<ApiKeyMiddleware>();
app.MapGet("/", () => "Hello World!");
app.MapUsersEndpoints();
app.MapAddressEndpoints();
app.MapCurrencyEndpoints();
app.MapCurrencyConversionEndpoints();
app.Run();
