namespace PruebaTecnica.Application.Users.Commands.CreateUser;

public abstract record CreateUserResult;

public sealed record UserCreated(int Id) : CreateUserResult;

public sealed record UserCreationConflict(string Message) : CreateUserResult;