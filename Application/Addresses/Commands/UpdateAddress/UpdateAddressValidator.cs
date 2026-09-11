using System.Data;
using FluentValidation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PruebaTecnica.Application.Addresses.Commands.UpdateAddress;

public sealed class UpdateAddressValidator : AbstractValidator<UpdateAddressCommand>{
    public UpdateAddressValidator(){
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