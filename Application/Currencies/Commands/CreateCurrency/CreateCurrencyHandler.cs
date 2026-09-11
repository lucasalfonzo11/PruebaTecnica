using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Domain.Entities;
using PruebaTecnica.Infrastructure.Persistence;

namespace PruebaTecnica.Application.Currencies.Commands.CreateCurrency;

public sealed class CreateCurrencyHandler(AppDbContext db){
    private const int SqliteUniqueConstraintError = 2067;
    private readonly AppDbContext _db = db;

    public async Task<CreateCurrencyResult> HandleAsync(CreateCurrencyCommand command, CancellationToken cancellationToken){
        var code = command.Code.Trim().ToUpperInvariant();

        var codeExists = await _db.Currencies.AnyAsync(currency => currency.Code == code, cancellationToken);
        if (codeExists)
        {
            return new CurrencyCreationConflict("A currency with this code already exists.");
        }

        var currency = new Currency
        {
            Code = code,
            Name = command.Name,
            RateToBase = command.RateToBase
        };
        _db.Currencies.Add(currency);

        try
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
            when (exception.InnerException is SqliteException sqliteException
            && sqliteException.SqliteExtendedErrorCode == SqliteUniqueConstraintError)
        {
            _db.Entry(currency).State = EntityState.Detached;
            return new CurrencyCreationConflict("A currency with this code already exists.");
        }
        return new CurrencyCreated(currency.Id);
    }
}