using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Domain.Entities;
using PruebaTecnica.Infrastructure.Persistence;

namespace PruebaTecnica.Application.Users.Commands.UpdateUser;

public sealed class UpdateUserHandler(AppDbContext db){
    private const int SqliteUniqueConstraintError = 2067;
    private readonly AppDbContext _db = db;

    public async Task<UpdateUserResult> HandleAsync(int id, UpdateUserCommand command, CancellationToken cancellationToken){
        // VALIDAR QUE EL USUARIO EXISTA
        User? user = await _db.Users.SingleOrDefaultAsync(user => user.Id == id, cancellationToken);
        if (user is null){
            return new UserUpdateNotFound();
        }

        // VALIDAR QUE EL EMAIL NO ESTÉ REPETIDO
        bool emailExists = await _db.Users.AnyAsync(other => other.Email == command.Email && other.Id != id, cancellationToken);
        if (emailExists){
            return new UserUpdateConflict("A user with email already exists.");
        }

        // ACTUALIZAR LOS DATOS DEL USUARIO
        user.Name = command.Name;
        user.Email = command.Email;
        user.IsActive = command.IsActive;

        try{
            // GUARDAR LOS CAMBIOS EN LA BASE DE DATOS
            await _db.SaveChangesAsync(cancellationToken);
        }catch (DbUpdateException exception)
            when(exception.InnerException is SqliteException sqliteException && sqliteException.SqliteExtendedErrorCode == SqliteUniqueConstraintError){
            // DESHACER LOS CAMBIOS EN LA ENTIDAD USUARIO EN CASO DE CONFLICTO
            _db.Entry(user).State = EntityState.Detached;
            return new UserUpdateConflict("A user with this email already exists.");
        }
        return new UserUpdated(user.Id);
    }
}