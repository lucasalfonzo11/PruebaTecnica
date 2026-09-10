using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Infrastructure.Persistence;

public class AppDbContext : DbContext{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<User> Users => Set<User>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Currency> Currencies => Set<Currency>();

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>{
            entity.HasIndex(user => user.Email).IsUnique();
            entity.HasIndex(user => user.CI).IsUnique();
            entity.Property(user => user.PasswordHash).HasColumnName("Password");
            entity.Property(user => user.IsActive).HasDefaultValue(true);
            entity.HasMany(user => user.Addresses)
                .WithOne(address => address.User)
                .HasForeignKey(address => address.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Currency>(entity => entity.HasIndex(currency => currency.Code).IsUnique());
    }
}