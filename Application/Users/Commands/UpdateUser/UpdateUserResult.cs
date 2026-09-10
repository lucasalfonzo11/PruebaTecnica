using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace PruebaTecnica.Application.Users.Commands.UpdateUser;

public abstract record UpdateUserResult;

public sealed record UserUpdated(int Id) : UpdateUserResult;
public sealed record UserUpdateNotFound : UpdateUserResult;
public sealed record UserUpdateConflict(string Message) : UpdateUserResult;