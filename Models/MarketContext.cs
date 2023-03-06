/*
    Define the context for the Market database.
*/
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceAPI.Models;


/*
    Define class for the Market database.
*/
public class MarketContext: DbContext
{
    public MarketContext(DbContextOptions<MarketContext> options)
        : base(options) { }

    // Add tables for the DB.
    public DbSet<StoreItem> Stores { get; set; } = null!;
    public DbSet<ProductItem> Products { get; set; } = null!;
    public DbSet<TagItem> Tags { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Add key for store/product relation.
        modelBuilder.Entity<StoreProduct>()
            .HasKey(sp => new {sp.StoreId, sp.ProductId});

        // Add relation One-To-Many with the Store table.
        modelBuilder.Entity<StoreProduct>()
            .HasOne(sp => sp.Store)
            .WithMany(s => s.Products)
            .HasForeignKey(sp => sp.StoreId);

        // Add relation One-To-Many with the Product table.
        modelBuilder.Entity<StoreProduct>()
            .HasOne(sp => sp.Product)
            .WithMany(s => s.Stores)
            .HasForeignKey(sp => sp.ProductId);

        // Add key for product/tag relation.
        modelBuilder.Entity<ProductTag>()
            .HasKey(sp => new {sp.ProductId, sp.TagId});

        // Add relation One-To-Many with the Product table.
        modelBuilder.Entity<ProductTag>()
            .HasOne(sp => sp.Product)
            .WithMany(s => s.Tags)
            .HasForeignKey(sp => sp.ProductId);

        // Add relation One-To-Many with the Tag table.
        modelBuilder.Entity<ProductTag>()
            .HasOne(sp => sp.Tag)
            .WithMany(s => s.Products)
            .HasForeignKey(sp => sp.TagId);

    }

}
