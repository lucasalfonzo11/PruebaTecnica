namespace PruebaTecnica.Domain.Entities;

public class User{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string CI { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
}