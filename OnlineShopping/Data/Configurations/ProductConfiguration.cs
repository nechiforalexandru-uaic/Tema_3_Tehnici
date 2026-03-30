using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShopping.Models;

namespace OnlineShopping.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Description).IsRequired().HasMaxLength(500);
        builder.Property(p => p.Price).IsRequired().HasPrecision(18, 2);
        builder.Property(p => p.StockQuantity).IsRequired();
        builder.Property(p => p.Category).IsRequired().HasMaxLength(100);
        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => p.Category);
    }
}