/* MarketPlaceAPI.Models.MarketContext
 *
 * DbContext class that define the tables and the configuration of the
 * marketplace database. This class inherits from DbContext.
 */

using Microsoft.EntityFrameworkCore;

namespace MarketPlaceAPI.Models;


/// <summary>
/// Class <c>MarketContext</c> is the database context with Products, Stores
/// and Tags tables.
/// </summary>
/// <remarks>
/// This class inherits from DbContext class.
/// </remarks>
public class MarketContext: DbContext
{

    /// <summary>
    /// This constructor just call the mother class constructor with the
    /// options object.
    /// </summary>
    public MarketContext(DbContextOptions<MarketContext> options)
        : base(options) { }

    // Add tables for the DB.

    /// <value>
    /// Property <c>Stores</c> corresponds to the table with stores
    /// information.
    /// </value>
    public DbSet<StoreItem> Stores { get; set; } = null!;

    /// <value>
    /// Property <c>Products</c> corresponds to the table with products
    /// information.
    /// </value>
    public DbSet<ProductItem> Products { get; set; } = null!;

    /// <value>
    /// Property <c>Tags</c> corresponds to the table with tags information.
    /// </value>
    public DbSet<TagItem> Tags { get; set; } = null!;

    /// <summary>
    /// Configure the relations between Products, Stores and Tags tables.
    /// <list type="bullet">
    ///     <listheader>
    ///        <term>Relations between tables.</term>
    ///        <description>Description of the relations that the Products,
    ///        Stores and Tags tables have with each other.</description>
    ///     </listheader>
    ///     <item>
    ///        <term>Products/Stores</term>
    ///        <description>Many to Many relation.</description>
    ///     </item>
    ///     <item>
    ///        <term>Products/Tags</term>
    ///        <description>Many to Many relation.</description>
    ///     </item>
    /// </list>
    /// (
    ///     <param name="modelBuilder">Builder of the models of the
    ///     database.</param>
    /// )
    /// </summary>
    /// <paramref name="modelBuilder"/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Define the key for store/product relation.
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

        // Define the key for product/tag relation.
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
