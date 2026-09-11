namespace PruebaTecnica.Application.CurrencyConversion;

public sealed class ConvertCurrencyQuery{
    public required string FromCurrencyCode { get; init; }
    public required string ToCurrencyCode { get; init; }
    public required decimal Amount { get; init; }
}