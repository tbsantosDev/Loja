using Loja.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Infra.Data.Configurations
{
    public class ShipmentConfiguration : IEntityTypeConfiguration<ShipmentModel>
    {
        public void Configure(EntityTypeBuilder<ShipmentModel> builder)
        {
            builder.ToTable("Shipments");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.TrackingCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(s => s.Carrier)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(s => s.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.HasOne(s => s.Order)
                .WithMany()
                .HasForeignKey(s => s.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
