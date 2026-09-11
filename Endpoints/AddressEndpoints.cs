using FluentValidation;
using PruebaTecnica.Application.Addresses.Commands.CreateAddress;
using PruebaTecnica.Application.Addresses.Commands.UpdateAddress;
using PruebaTecnica.Application.Addresses.Queries.GetUserAddresses;

namespace PruebaTecnica.Endpoints;

public static class AddressEndpoints{
    public static void MapAddressEndpoints(this IEndpointRouteBuilder endpoints){
        endpoints.MapPost("/users/{userId:int}/addresses", CreateAddressAsync);
        endpoints.MapGet("/users/{userId:int}/addresses", GetUserAddressesAsync);
        endpoints.MapPut("/addresses/{id:int}", UpdateAddressAsync);
    }

    private static async Task<IResult> CreateAddressAsync(
        int userId,
        CreateAddressCommand command,
        IValidator<CreateAddressCommand> validator,
        CreateAddressHandler handler,
        CancellationToken cancellationToken){
        if (userId <= 0){
            return Results.BadRequest(new{ error = "User ID must be greater than zero."});
        }

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid){
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await handler.HandleAsync(userId, command, cancellationToken);

        return result switch{
            AddressCreated created => Results.Json( new { id = created.Id, userId }, statusCode: StatusCodes.Status201Created),
            AddressUserNotFound => Results.NotFound(new{ error = "User not found."}),
            _ => throw new InvalidOperationException( "Unexpected create-address result.")
        };
    }

    private static async Task<IResult> GetUserAddressesAsync(
        int userId,
        GetUserAddressesHandler handler,
        CancellationToken cancellationToken
    ){
        if (userId <= 0){
            return Results.BadRequest(new{error = "User ID must be greater than zero."});
        }
        var addresses = await handler.HandleAsync(new GetUserAddressesQuery(userId), cancellationToken);
        if (addresses is null){
            return Results.NotFound(new{error = "User not found."});
        }
        return Results.Ok(addresses);
    }

    private static async Task<IResult> UpdateAddressAsync(
        int id,
        UpdateAddressCommand command,
        IValidator<UpdateAddressCommand> validator,
        UpdateAddressHandler handler,
        CancellationToken cancellationToken
    ){
        if (id <= 0){
            return Results.BadRequest(new{error = "Address ID must be greater than zero."});
        }

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid){
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await handler.HandleAsync(id, command, cancellationToken);
        return result switch{
            UpdateAddressResult.Updated => Results.NoContent(),
            UpdateAddressResult.NotFound => Results.NotFound(new{error = "Address not found."}),
            _ => throw new InvalidOperationException("Unexpected update-address result.")
        };
    }
}