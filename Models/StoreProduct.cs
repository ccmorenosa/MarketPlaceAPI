/* MarketPlaceAPI.Models.StoreProduct
 *
 * Define the model that holds the relation between Stores and Products.
 */

namespace MarketPlaceAPI.Models;

/// <summary>
/// Class <c>StoreProduct</c> represents the Many To Many relation between
/// Stores and Products tables.
/// </summary>
public class StoreProduct
{

    // Id and item for the Store table.
    public long StoreId { get; set; }
    public StoreItem Store { get; set; } = new StoreItem();

    // Id and item for the Product table.
    public long ProductId { get; set; }
    public ProductItem Product { get; set; } = new ProductItem();

    /// <value>
    /// Property <c>Price</c> represent the price that a certain product has in
    /// a given store.
    /// </value>
    public decimal Price { get; set; }

}
