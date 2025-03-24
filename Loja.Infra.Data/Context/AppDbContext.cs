 using Loja.Domain.Models;
using Loja.Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Loja.Infra.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<OrderedItemsModel> OrderedItems { get; set; }
        public DbSet<FavoriteModel> Favorites { get; set; }
        public DbSet<PaymentModel> Payments { get; set; }
        public DbSet<CartModel> Carts { get; set; }
        public DbSet<ShipmentModel> Shipments { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<DiscountModel> Discounts { get; set; }
        public DbSet<ProductImageModel> ProductImages { get; set; }
        public DbSet<ReviewModel> Reviews { get; set; }
        public DbSet<CouponModel> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new FavoriteConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new ShipmentConfiguration());
            modelBuilder.ApplyConfiguration(new OrderedItemsConfiguration());
            modelBuilder.ApplyConfiguration(new DiscountConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewConfiguration());
            modelBuilder.ApplyConfiguration(new CouponConfiguration());
        }
    }
}
