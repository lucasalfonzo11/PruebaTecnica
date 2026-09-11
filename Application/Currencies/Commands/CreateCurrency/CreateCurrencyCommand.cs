namespace PruebaTecnica.Application.Currencies.Commands.CreateCurrency;

public sealed class CreateCurrencyCommand{
    public required string Code { get; init; }
    public required string Name { get; init; }
    public required decimal RateToBase { get; init; }
}