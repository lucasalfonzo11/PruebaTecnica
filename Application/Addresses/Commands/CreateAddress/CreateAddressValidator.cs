using System.Data;
using FluentValidation;

namespace PruebaTecnica.Application.Addresses.Commands.CreateAddress;

public sealed class CreateAddressValidator : AbstractValidator<CreateAddressCommand>{
    public CreateAddressValidator(){
        RuleFor(command => command.Street)
            .NotEmpty()
            .WithMessage("Street is required.");

        RuleFor(command => command.City)
            .NotEmpty()
            .WithMessage("City is required.");

        RuleFor(command => command.Country)
            .NotEmpty()
            .WithMessage("Country is required.");
    }
}