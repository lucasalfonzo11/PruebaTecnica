using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Infrastructure.Persistence;

namespace PruebaTecnica.Application.Addresses.Commands.DeleteAddress;

public sealed class DeleteAddressHandler(AppDbContext db){
    private readonly AppDbContext _db = db;

    public async Task<DeleteAddressResult> HandleAsync(DeleteAddressCommand command, CancellationToken cancellationToken){
        var deletedRows = await _db.Addresses
            .Where(address => address.Id == command.Id)
            .ExecuteDeleteAsync(cancellationToken);
        return deletedRows == 0 ? DeleteAddressResult.NotFound : DeleteAddressResult.Deleted;
    }
}