namespace PruebaTecnica.Application.Users.Commands.UpdateUser;

public sealed class UpdateUserCommand{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required bool IsActive { get; init; }
}