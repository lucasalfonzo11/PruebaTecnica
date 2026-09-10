using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Domain.Entities;
using PruebaTecnica.Infrastructure.Persistence;

namespace PruebaTecnica.Application.Users.Queries.GetUsers;

public sealed class GetUsersHandler(AppDbContext db){
    private readonly AppDbContext _db = db;

    public Task<List<UserResponse>> HandleAsync(GetUsersQuery query, CancellationToken cancellationToken){
        var users = _db.Users.AsQueryable();
        if (query.IsActive.HasValue)
        {
            var isActive = query.IsActive.Value;
            users = users.Where(user => user.IsActive == isActive);
        }

        return users
            .OrderBy(user => user.Id)
            .Select(user => new UserResponse(
                user.Id,
                user.Name,
                user.Email,
                user.CI,
                user.IsActive
            ))
            .ToListAsync(cancellationToken);
    }
}