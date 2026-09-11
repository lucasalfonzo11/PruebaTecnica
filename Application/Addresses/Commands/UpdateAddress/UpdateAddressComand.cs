namespace PruebaTecnica.Application.Addresses.Commands.UpdateAddress;

public sealed class UpdateAddressCommand{
    public required string Street { get; init; }
    public required string City { get; init; }
    public required string Country { get; init; }
    public string? ZipCode { get; init; }
}