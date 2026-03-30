using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShopping.Models;

namespace OnlineShopping.Data.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems");
        builder.HasKey(ci => ci.Id);
        builder.Property(ci => ci.Quantity).IsRequired();
        builder.HasOne(ci => ci.Cart)
              .WithMany(c => c.CartItems)
              .HasForeignKey(ci => ci.CartId)
              .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(ci => ci.Product)
              .WithMany(p => p.CartItems)
              .HasForeignKey(ci => ci.ProductId)
              .OnDelete(DeleteBehavior.Restrict);
    }
}