namespace PruebaTecnica.Domain.Entities;

public class Address{
    public int Id { set; get; }
    public int UserId { get; set; }
    public required string Street { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public string? ZipCode { get; set; }
    public User User { get; set; } = null!;
}