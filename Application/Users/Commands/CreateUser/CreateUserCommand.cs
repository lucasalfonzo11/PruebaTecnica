namespace PruebaTecnica.Application.Users.Commands.CreateUser;

public sealed class CreateUserCommand{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string CI { get; init; }
    public required string Password { get; init; }
}