using FluentValidation;
using PruebaTecnica.Application.Currencies.Commands.CreateCurrency;

namespace PruebaTecnica.Endpoints;

public static class CurrencyEndpoints{
    public static void MapCurrencyEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/currencies");
        group.MapPost("", CreateCurrencyAsync);
    }

    private static async Task<IResult> CreateCurrencyAsync(
        CreateCurrencyCommand command,
        IValidator<CreateCurrencyCommand> validator,
        CreateCurrencyHandler handler,
        CancellationToken cancellationToken
    ){
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid){
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await handler.HandleAsync(command, cancellationToken);
        return result switch
        {
            CurrencyCreated created => Results.Json(
                new { id = created.Id },
                statusCode: StatusCodes.Status201Created
            ),
            CurrencyCreationConflict conflict => Results.Conflict(new { error = conflict.Message }),
            _ => throw new InvalidOperationException("Unexpected create-currency result.")
        };
    }
}