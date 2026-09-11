namespace PruebaTecnica.Application.Currencies.Commands.CreateCurrency;

public abstract record CreateCurrencyResult;

public sealed record CurrencyCreated(int Id) : CreateCurrencyResult;
public sealed record CurrencyCreationConflict(string Message) : CreateCurrencyResult;