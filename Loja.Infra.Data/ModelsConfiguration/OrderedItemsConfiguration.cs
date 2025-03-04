using Loja.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Infra.Data.Configurations
{
    public class OrderedItemsConfiguration : IEntityTypeConfiguration<OrderedItemsModel>
    {
        public void Configure(EntityTypeBuilder<OrderedItemsModel> builder)
        {
            builder.ToTable("OrderedItems");

            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.Amount)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(oi => oi.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oi => oi.Product)
                .WithMany(p => p.OrderedIn)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
