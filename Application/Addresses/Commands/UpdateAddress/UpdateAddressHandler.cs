using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Infrastructure.Persistence;

namespace PruebaTecnica.Application.Addresses.Commands.UpdateAddress;

public sealed class UpdateAddressHandler(AppDbContext db){
    private readonly AppDbContext _db = db;

    public async Task<UpdateAddressResult> HandleAsync(int id, UpdateAddressCommand command, CancellationToken cancellationToken){
        var address = await _db.Addresses.SingleOrDefaultAsync(address => address.Id == id, cancellationToken);
        if (address is null){
            return UpdateAddressResult.NotFound;
        }

        address.Street = command.Street;
        address.City = command.City;
        address.Country = command.Country;
        address.ZipCode = command.ZipCode;

        await _db.SaveChangesAsync(cancellationToken);

        return UpdateAddressResult.Updated;
    }
}