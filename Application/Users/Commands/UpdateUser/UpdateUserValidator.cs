using FluentValidation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Data;

namespace PruebaTecnica.Application.Users.Commands.UpdateUser;

public sealed class UpdateUserValidator : AbstractValidator<UpdateUserCommand>{
    public UpdateUserValidator(){
        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(command => command.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email is not valid.");
    }
}