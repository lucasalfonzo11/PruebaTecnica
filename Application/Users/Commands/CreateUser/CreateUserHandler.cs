using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Domain.Entities;
using PruebaTecnica.Infrastructure.Persistence;

namespace PruebaTecnica.Application.Users.Commands.CreateUser;

public sealed class CreateUserHandler(AppDbContext db, IPasswordHasher<User> passwordHasher){
    private const int SqliteUniqueConstraintError = 2067;

    private readonly AppDbContext _db = db;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

    public async Task<CreateUserResult> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken){
        // VERIFICACION DE UNICIDAD DE EMAIL
        bool emailExists = await _db.Users.AnyAsync(user => user.Email == command.Email, cancellationToken);
        if (emailExists){
            return new UserCreationConflict("A user with this email already exists.");
        }

        // VERIFICACION DE UNICIDAD DE CI
        bool ciExists = await _db.Users.AnyAsync(user => user.CI == command.CI, cancellationToken);
        if (ciExists){
            return new UserCreationConflict("A user with this CI already exists.");
        }

        var user = new User{
            Name = command.Name,
            Email = command.Email,
            CI = command.CI,
            PasswordHash = string.Empty,
            IsActive = true
        };

        // HASHING DE LA CONTRASEÑA
        user.PasswordHash = _passwordHasher.HashPassword(user, command.Password);

        // AGREGANDO EL USUARIO A LA BASE DE DATOS
        _db.Users.Add(user);

        // GUARDANDO LOS CAMBIOS EN LA BASE DE DATOS
        try{
            await _db.SaveChangesAsync(cancellationToken);
        } catch (DbUpdateException exception)
            when (exception.InnerException is SqliteException sqliteException
                    && sqliteException.SqliteExtendedErrorCode == SqliteUniqueConstraintError)
        {
            // DESHACIENDO LA ADICIÓN DEL USUARIO EN CASO DE CONFLICTO
            _db.Entry(user).State = EntityState.Detached;
            return new UserCreationConflict("A user with this email or CI already exists.");
        }

        return new UserCreated(user.Id);
    }
}