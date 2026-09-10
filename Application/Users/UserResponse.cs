namespace PruebaTecnica.Application.Users;

public sealed record UserResponse(int Id, string Name, string Email, string CI, bool IsActive);