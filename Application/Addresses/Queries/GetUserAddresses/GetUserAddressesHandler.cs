using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Infrastructure.Persistence;

namespace PruebaTecnica.Application.Addresses.Queries.GetUserAddresses;

public sealed class GetUserAddressesHandler(AppDbContext db){
    private readonly AppDbContext _db = db;

    public async Task<List<AddressResponse>?> HandleAsync(GetUserAddressesQuery query, CancellationToken cancellationToken){
        var userExists = await _db.Users.AnyAsync(user => user.Id == query.UserId, cancellationToken);
        if (!userExists){
            return null;
        }

        return await _db.Addresses
            .Where(address => address.UserId == query.UserId)
            .OrderBy(address => address.Id)
            .Select(address => new AddressResponse(
                address.Id,
                address.UserId,
                address.Street,
                address.City,
                address.Country,
                address.ZipCode))
            .ToListAsync(cancellationToken);
    }
}