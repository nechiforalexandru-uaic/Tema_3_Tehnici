using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShopping.Models;

namespace OnlineShopping.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");
        builder.HasKey(oi => oi.Id);
        builder.Property(oi => oi.Quantity).IsRequired();
        builder.Property(oi => oi.UnitPrice).IsRequired().HasPrecision(18, 2);
        builder.HasOne(oi => oi.Product)
              .WithMany(p => p.OrderItems)
              .HasForeignKey(oi => oi.ProductId)
              .OnDelete(DeleteBehavior.Restrict);
    }
}