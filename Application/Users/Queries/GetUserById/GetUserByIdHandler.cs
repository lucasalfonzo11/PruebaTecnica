using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Application.Users;
using PruebaTecnica.Domain.Entities;
using PruebaTecnica.Infrastructure.Persistence;

namespace PruebaTecnica.Application.Users.Queries.GetUserById;

public sealed class GetUserByIdHandler(AppDbContext db){
    private readonly AppDbContext _db = db;
    public Task<UserResponse?> HandleAsync(GetUserByIdQuery query, CancellationToken cancellationToken){
        return _db.Users
            .Where(user => user.Id == query.Id)
            .Select(user => new UserResponse(
                user.Id,
                user.Name,
                user.Email,
                user.CI,
                user.IsActive
            ))
            .SingleOrDefaultAsync(cancellationToken);
    }
}