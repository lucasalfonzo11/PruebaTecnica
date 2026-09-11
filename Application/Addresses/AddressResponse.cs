namespace PruebaTecnica.Application.Addresses;
public sealed record AddressResponse(
    int Id,
    int UserId,
    string Street,
    string City,
    string Country,
    string? ZipCode
);