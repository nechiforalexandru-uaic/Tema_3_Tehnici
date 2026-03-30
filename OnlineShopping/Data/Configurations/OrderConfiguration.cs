using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShopping.Models;

namespace OnlineShopping.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Status).IsRequired().HasMaxLength(50);
        builder.Property(o => o.TotalAmount).IsRequired().HasPrecision(18, 2);
        builder.Property(o => o.ShippingAddress).HasMaxLength(500);
        builder.HasMany(o => o.OrderItems)
              .WithOne(oi => oi.Order)
              .HasForeignKey(oi => oi.OrderId)
              .OnDelete(DeleteBehavior.Cascade);
    }
}