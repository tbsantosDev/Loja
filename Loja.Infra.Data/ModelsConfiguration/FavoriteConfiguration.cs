using Loja.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Infra.Data.Configurations
{
    public class FavoriteConfiguration : IEntityTypeConfiguration<FavoriteModel>
    {
        public void Configure(EntityTypeBuilder<FavoriteModel> builder)
        {
            builder.ToTable("Favorites");

            builder.HasKey(f => f.Id);

            builder.HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Product)
                .WithMany(p => p.FavoritedBy)
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
