using Loja.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Infra.Data.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImageModel>
    {
        public void Configure(EntityTypeBuilder<ProductImageModel> builder)
        {
            builder.ToTable("ProductImages");

            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.Url)
                .IsRequired()
                .HasColumnType("bytea"); // Tipo específico para PostgreSQL

            builder.Property(pi => pi.Sequence)
                .IsRequired();

            builder.HasOne(pi => pi.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
