namespace PruebaTecnica.Application.CurrencyConversion;

public sealed record CurrencyConversionResponse(
    string FromCurrency,
    string ToCurrency,
    decimal OriginalAmount,
    decimal ConvertedAmount
);