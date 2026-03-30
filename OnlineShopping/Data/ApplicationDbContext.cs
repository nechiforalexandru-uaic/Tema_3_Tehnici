using Microsoft.EntityFrameworkCore;
using OnlineShopping.Models;

namespace OnlineShopping.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Description).IsRequired().HasMaxLength(500);
            entity.Property(p => p.Price).IsRequired().HasPrecision(18, 2);
            entity.Property(p => p.StockQuantity).IsRequired();
            entity.Property(p => p.Category).IsRequired().HasMaxLength(100);
            entity.HasIndex(p => p.Name);
            entity.HasIndex(p => p.Category);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(150);
            entity.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Carts");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.UserId).IsRequired();
            entity.HasOne(c => c.User)
                  .WithOne(u => u.Cart)
                  .HasForeignKey<Cart>(c => c.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.ToTable("CartItems");
            entity.HasKey(ci => ci.Id);
            entity.Property(ci => ci.Quantity).IsRequired();
            entity.HasOne(ci => ci.Cart)
                  .WithMany(c => c.CartItems)
                  .HasForeignKey(ci => ci.CartId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(ci => ci.Product)
                  .WithMany(p => p.CartItems)
                  .HasForeignKey(ci => ci.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Status).IsRequired().HasMaxLength(50);
            entity.Property(o => o.TotalAmount).IsRequired().HasPrecision(18, 2);
            entity.Property(o => o.ShippingAddress).HasMaxLength(500);
            entity.HasMany(o => o.OrderItems)
                  .WithOne(oi => oi.Order)
                  .HasForeignKey(oi => oi.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItems");
            entity.HasKey(oi => oi.Id);
            entity.Property(oi => oi.Quantity).IsRequired();
            entity.Property(oi => oi.UnitPrice).IsRequired().HasPrecision(18, 2);
            entity.HasOne(oi => oi.Product)
                  .WithMany(p => p.OrderItems)
                  .HasForeignKey(oi => oi.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "John Doe", Email = "john@example.com", CreatedAt = DateTime.UtcNow },
            new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com", CreatedAt = DateTime.UtcNow }
        );

        modelBuilder.Entity<Cart>().HasData(
            new Cart { Id = 1, UserId = 1, CreatedAt = DateTime.UtcNow },
            new Cart { Id = 2, UserId = 2, CreatedAt = DateTime.UtcNow }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m, StockQuantity = 10, Category = "Electronics", CreatedAt = DateTime.UtcNow },
            new Product { Id = 2, Name = "Mouse", Description = "Wireless mouse", Price = 29.99m, StockQuantity = 50, Category = "Electronics", CreatedAt = DateTime.UtcNow },
            new Product { Id = 3, Name = "Keyboard", Description = "Mechanical keyboard", Price = 89.99m, StockQuantity = 30, Category = "Electronics", CreatedAt = DateTime.UtcNow },
            new Product { Id = 4, Name = "Monitor", Description = "27-inch 4K monitor", Price = 399.99m, StockQuantity = 15, Category = "Electronics", CreatedAt = DateTime.UtcNow }
        );
    }
}