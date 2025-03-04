using Loja.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Infra.Data.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<ReviewModel>
    {
        public void Configure(EntityTypeBuilder<ReviewModel> builder)
        {
            builder.ToTable("Reviews");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Grade)
                .IsRequired()
                .HasColumnType("int")
                .HasComment("Nota da avaliação entre 1 e 5");

            builder.Property(r => r.Comment)
                .HasMaxLength(500)
                .HasColumnType("text");

            builder.Property(r => r.EvaluationDate)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            builder.HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
