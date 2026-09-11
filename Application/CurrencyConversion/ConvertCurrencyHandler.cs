using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Domain.Entities;
using PruebaTecnica.Infrastructure.Persistence;

namespace PruebaTecnica.Application.CurrencyConversion;

public sealed class ConvertCurrencyHandler(AppDbContext db){
    private readonly AppDbContext _db = db;

    public async Task<ConvertCurrencyResult> HandleAsync(ConvertCurrencyQuery query, CancellationToken cancellationToken) {
        string fromCode = query.FromCurrencyCode.Trim().ToUpperInvariant();
        string toCode = query.ToCurrencyCode.Trim().ToUpperInvariant();

        Currency? fromCurrency = await FindCurrencyAsync(fromCode, cancellationToken);
        if (fromCurrency is null){
            return new CurrencyNotFound(fromCode);
        }

        Currency? toCurrency = await FindCurrencyAsync(toCode, cancellationToken);
        if(toCurrency is null){
            return new CurrencyNotFound(toCode);
        }

        try{
            decimal baseAmount = query.Amount * fromCurrency.RateToBase;
            decimal convertedAmount = baseAmount / toCurrency.RateToBase;
            return new CurrencyConvertionComplete(
                new CurrencyConversionResponse(
                    fromCurrency.Code,
                    toCurrency.Code,
                    query.Amount,
                    convertedAmount
                )
            );
        }catch(OverflowException){
            return new CurrencyConversionOverflow();
        }
    }

    private Task<Currency?> FindCurrencyAsync(string code, CancellationToken cancellationToken){
        return _db.Currencies.SingleOrDefaultAsync(currency => currency.Code == code, cancellationToken);
    }
}