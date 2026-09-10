using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Application.Users.Commands.DeleteUser;
using PruebaTecnica.Infrastructure.Persistence;

namespace PruebaTecnica.Application.Users.Commands.DeleteUser;

public sealed class DeleteUserHandler(AppDbContext db)
{
    private readonly AppDbContext _db = db;

    public async Task<DeleteUserResult> HandleAsync(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var deleteRows = await _db.Users
            .Where(user => user.Id == command.Id)
            .ExecuteDeleteAsync(cancellationToken);

        return deleteRows == 0 ? DeleteUserResult.NotFound : DeleteUserResult.Deleted;
    }
}