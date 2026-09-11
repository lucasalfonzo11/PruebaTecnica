using FluentValidation;
using System.Data;

namespace PruebaTecnica.Application.Currencies.Commands.CreateCurrency;

public sealed class CreateCurrencyValidator : AbstractValidator<CreateCurrencyCommand>{
    public CreateCurrencyValidator(){
        RuleFor(command => command.Code)
            .NotEmpty()
            .WithMessage("Currency code is required.");

        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("Currency name is required.");

        RuleFor(command => command.RateToBase)
            .GreaterThan(0m)
            .WithMessage("Rate to base must be greater than zero.");
    }
}
