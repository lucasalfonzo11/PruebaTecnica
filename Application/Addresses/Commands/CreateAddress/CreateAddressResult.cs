namespace PruebaTecnica.Application.Addresses.Commands.CreateAddress;

public abstract record CreateAddressResult;

public sealed record AddressCreated(int Id) : CreateAddressResult;
public sealed record AddressUserNotFound : CreateAddressResult;