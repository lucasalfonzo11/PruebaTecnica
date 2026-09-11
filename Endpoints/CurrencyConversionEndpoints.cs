using System.ComponentModel.DataAnnotations;
using FluentValidation;
using PruebaTecnica.Application.CurrencyConversion;
using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Endpoints;

public static class CurrencyConversionEndpoints{
    public static void MapCurrencyConversionEndpoints(this IEndpointRouteBuilder endpoints){
        endpoints.MapPost("/currency/convert", ConvertCurrencyAsync);
    }

    private static async Task<IResult> ConvertCurrencyAsync(
        ConvertCurrencyQuery query,
        IValidator<ConvertCurrencyQuery> validator,
        ConvertCurrencyHandler handler,
        CancellationToken cancellationToken
    ){
        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if(!validationResult.IsValid){
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await handler.HandleAsync(query, cancellationToken);
        return result switch
        {
            CurrencyConvertionComplete completed => Results.Ok(completed.Response),
            CurrencyNotFound notFound => Results.NotFound(new { error = $"Currency '{notFound.Code}' not found" }),
            CurrencyConversionOverflow overflow => Results.BadRequest(new { error = "Convertion amount is too large." }),
            _ => throw new InvalidOperationException("Unexpected currency-conversion result.")
        };
    }
}