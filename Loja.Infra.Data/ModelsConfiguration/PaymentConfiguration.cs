using Loja.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Infra.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<PaymentModel>
    {
        public void Configure(EntityTypeBuilder<PaymentModel> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(p => p.PaymentDateTime)
                .IsRequired();

            builder.Property(p => p.PaymentMethod)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(p => p.Value)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne(p => p.Order)
                .WithMany()
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
