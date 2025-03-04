using Loja.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Infra.Data.Configurations
{
    public class CouponConfiguration : IEntityTypeConfiguration<CouponModel>
    {
        public void Configure(EntityTypeBuilder<CouponModel> builder)
        {
            builder.ToTable("Coupons");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("varchar(20)")
                .HasComment("Código único do cupom de desconto");

            builder.Property(c => c.Description)
                .HasMaxLength(255)
                .HasColumnType("varchar(255)");

            builder.Property(c => c.Percentage)
                .IsRequired()
                .HasColumnType("decimal(5,2)")
                .HasComment("Porcentagem de desconto aplicada pelo cupom");

            builder.Property(c => c.MaximumQuantity)
                .HasColumnType("int")
                .HasComment("Número máximo de vezes que o cupom pode ser utilizado");

            builder.Property(c => c.UsesPerformed)
                .HasColumnType("int")
                .HasDefaultValue(0)
                .HasComment("Número de vezes que o cupom já foi utilizado");

            builder.Property(c => c.StartDate)
                .IsRequired()
                .HasColumnType("timestamp")
                .HasComment("Data de início da validade do cupom");

            builder.Property(c => c.EndDate)
                .IsRequired()
                .HasColumnType("timestamp")
                .HasComment("Data final de validade do cupom");

            builder.Property(c => c.Active)
                .IsRequired()
                .HasColumnType("boolean")
                .HasDefaultValue(false)
                .HasComment("Indica se o cupom está ativo ou não");

            builder.HasIndex(c => c.Code)
                .IsUnique()
                .HasDatabaseName("IX_Coupons_Code");
        }
    }
}
