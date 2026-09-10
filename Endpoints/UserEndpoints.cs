using FluentValidation;
using PruebaTecnica.Application.Users.Commands.CreateUser;
using PruebaTecnica.Application.Users.Queries.GetUserById;
using PruebaTecnica.Application.Users.Queries.GetUsers;
using PruebaTecnica.Application.Users.Commands.UpdateUser;
using PruebaTecnica.Application.Users.Commands.DeleteUser;

namespace PruebaTecnica.Endpoints;

public static class UserEndpoints{
    public static void MapUsersEndpoints(this IEndpointRouteBuilder endpoints){
        var group = endpoints.MapGroup("/users");
        group.MapPost("", CreateUserAsync);
        group.MapGet("/{id:int}", GetUserByIdAsync);
        group.MapGet("", GetUsersAsync);
        group.MapPut("/{id:int}", UpdateUserAsync);
        group.MapDelete("/{id:int}", DeleteUserAsync);
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

    private static async Task<IResult> GetUserByIdAsync(int id, GetUserByIdHandler handler, CancellationToken cancellationToken){
        if (id <= 0){
            return Results.BadRequest(new { error = "User ID must be greater than zero." });
        }

        var query = new GetUserByIdQuery(id);

        var user = await handler.HandleAsync(query, cancellationToken);
        if (user is null){
            return Results.NotFound(new { error = "User not found." });
        }

        return Results.Ok(user);
    }

    private static async Task<IResult> GetUsersAsync(bool? isActive, GetUsersHandler handler, CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery(isActive);
        var users = await handler.HandleAsync(query, cancellationToken);
        return Results.Ok(users);
    }

    private static async Task<IResult> UpdateUserAsync(int id,UpdateUserCommand command, IValidator<UpdateUserCommand> validator, UpdateUserHandler handler, CancellationToken cancellationToken){
        if (id <= 0){
            return Results.BadRequest(new { error = "User ID must be greater than zero." });
        }

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid){
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await handler.HandleAsync(id, command, cancellationToken);
        return result switch{
            UserUpdated => Results.NoContent(),
            UserUpdateNotFound => Results.NotFound(new { error = "User not found." }),
            UserUpdateConflict conflict => Results.Conflict(new { error = conflict.Message }),
            _=> throw new InvalidOperationException("Unexpected update-user result.")
        };
    }

    private static async Task<IResult> DeleteUserAsync(int id, DeleteUserHandler handler, CancellationToken cancellationToken){
        if( id <= 0){
            return Results.BadRequest(new { error = "User ID must be greater than zero." });
        }
        var result = await handler.HandleAsync(new DeleteUserCommand(id), cancellationToken);
        return result switch
        {
            DeleteUserResult.Deleted => Results.NoContent(),
            DeleteUserResult.NotFound => Results.NotFound(new { error = "User not found." }),
            _ => throw new InvalidOperationException("Unexpected delete-user result.")
        };
    }
}