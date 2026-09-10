using FluentValidation;
using PruebaTecnica.Application.Users.Commands.CreateUser;

namespace PruebaTecnica.Endpoints;

public static class UserEndpoints{
    public static void MapUsersEndpoints(this IEndpointRouteBuilder endpoints){
        var group = endpoints.MapGroup("/users");
        group.MapPost("", CreateUserAsync);
    }

    private static async Task<IResult> CreateUserAsync(
        CreateUserCommand command,
        IValidator<CreateUserCommand> validator,
        CreateUserHandler handler,
        CancellationToken cancellationToken
    ){
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid){
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await handler.HandleAsync(command, cancellationToken);
        return result switch{
            UserCreated created => Results.Created($"/users/{created.Id}", new { id = created.Id }),
            UserCreationConflict conflict => Results.Conflict(new { error = conflict.Message }),
            _ => throw new InvalidOperationException("Unexpected create-user result.")
        };
    }
}