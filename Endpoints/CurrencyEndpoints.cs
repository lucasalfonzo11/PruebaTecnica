using FluentValidation;
using PruebaTecnica.Application.Currencies.Commands.CreateCurrency;
using PruebaTecnica.Application.Currencies.Queries.GetCurrencies;
using PruebaTecnica.Application.Currencies;
namespace PruebaTecnica.Endpoints;


public static class CurrencyEndpoints{
    public static void MapCurrencyEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/currencies");
        group.MapPost("", CreateCurrencyAsync);
        group.MapGet("", GetCurrenciesAsync);
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

        CreateCurrencyResult? result = await handler.HandleAsync(command, cancellationToken);
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

    private static async Task<IResult> GetCurrenciesAsync(
        GetCurrenciesHandler handler,
        CancellationToken cancellationToken
    ){
        List<CurrencyResponse>? currencies = await handler.HandleAsync(new GetCurrenciesQuery(), cancellationToken);
        return Results.Ok(currencies);
    }
}