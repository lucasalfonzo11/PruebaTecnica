using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Domain.Entities;
using PruebaTecnica.Infrastructure.Persistence;
namespace PruebaTecnica.Application.Addresses.Commands.CreateAddress;

public sealed class CreateAddressHandler(AppDbContext db){
    private const int SqliteForeignKeyConstraintError = 787;
    private readonly AppDbContext _db = db;

    public async Task<CreateAddressResult> HandleAsync(int userId, CreateAddressCommand command, CancellationToken cancellationToken){
        var userExists = await _db.Users.AnyAsync(user => user.Id == userId, cancellationToken);
        if(!userExists){
            return new AddressUserNotFound();
        }

        var address = new Address{
            UserId = userId,
            Street = command.Street,
            City = command.City,
            Country = command.Country,
            ZipCode = command.ZipCode
        };

        _db.Addresses.Add(address);

        try{
            await _db.SaveChangesAsync(cancellationToken);
        }catch(DbUpdateException exception)
            when(exception.InnerException is SqliteException sqliteException && sqliteException.SqliteExtendedErrorCode == SqliteForeignKeyConstraintError){
            _db.Entry(address).State = EntityState.Detached;
            return new AddressUserNotFound();
        }
        return new AddressCreated(address.Id);
    }
}