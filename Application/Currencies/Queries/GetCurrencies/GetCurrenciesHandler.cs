using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Domain.Entities;
using PruebaTecnica.Infrastructure.Persistence;

namespace PruebaTecnica.Application.Currencies.Queries.GetCurrencies;

public sealed class GetCurrenciesHandler(AppDbContext db){
    private readonly AppDbContext _db = db;

    public Task<List<CurrencyResponse>> HandleAsync (GetCurrenciesQuery query, CancellationToken cancellationToken){
        return _db.Currencies
            .OrderBy(currency => currency.Code)
            .Select(currency => new CurrencyResponse(
                currency.Id,
                currency.Code,
                currency.Name,
                currency.RateToBase
            ))
            .ToListAsync(cancellationToken);
    }
}