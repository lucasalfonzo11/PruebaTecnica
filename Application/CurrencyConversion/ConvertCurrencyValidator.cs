using FluentValidation;
using System.Data;

namespace PruebaTecnica.Application.CurrencyConversion;

public sealed class ConvertCurrencyValidator : AbstractValidator<ConvertCurrencyQuery>{
    public ConvertCurrencyValidator(){
        RuleFor(query => query.FromCurrencyCode)
            .NotEmpty()
            .WithMessage("Source currency code is required.");

        RuleFor(query => query.ToCurrencyCode)
            .NotEmpty()
            .WithMessage("Target currency code is required.");

        RuleFor(query => query.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero.");
    }
}