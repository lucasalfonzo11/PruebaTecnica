using FluentValidation;
using System.Data;

namespace PruebaTecnica.Application.Users.Commands.CreateUser;

public sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>{
    public CreateUserValidator(){
        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(command => command.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email format is invalid.");

        RuleFor(command => command.CI)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("CI is required.")
            .Matches(@"\A[1-9][0-9]*[A-Z]?\z")
            .WithMessage("CI must contain only digits and must not start with zero, optionally followed by one uppercase letter.");

        RuleFor(command => command.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Password is required.")
            .Length(8, 16)
            .WithMessage("Password must contain between 8 and 16 characters.");
    }
}