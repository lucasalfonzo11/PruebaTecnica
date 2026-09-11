using PruebaTecnica.Domain.Entities;
namespace PruebaTecnica.Application.CurrencyConversion;

public abstract record ConvertCurrencyResult;

public sealed record CurrencyConvertionComplete(CurrencyConversionResponse Response) : ConvertCurrencyResult;

public sealed record CurrencyNotFound(string Code) : ConvertCurrencyResult;

public sealed record CurrencyConversionOverflow : ConvertCurrencyResult;