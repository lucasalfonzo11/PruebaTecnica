namespace PruebaTecnica.Application.Addresses.Commands.CreateAddress;

public sealed class CreateAddressCommand{
    public required string Street { get; init; }
    public required string City { get; init; }
    public required string Country { get; init; }
    public string? ZipCode { get; init; }
}
