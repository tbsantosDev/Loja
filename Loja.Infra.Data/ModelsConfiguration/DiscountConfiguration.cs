using Loja.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Infra.Data.Configurations
{
    public class DiscountConfiguration : IEntityTypeConfiguration<DiscountModel>
    {
        public void Configure(EntityTypeBuilder<DiscountModel> builder)
        {
            builder.ToTable("Discounts");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Percentage)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            builder.Property(d => d.StartDate)
                .IsRequired();

            builder.Property(d => d.EndDate)
                .IsRequired();

            builder.Property(d => d.Active)
                .IsRequired()
                .HasDefaultValue(false); // Padrão: inativo

            builder.HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Category)
                .WithMany()
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
