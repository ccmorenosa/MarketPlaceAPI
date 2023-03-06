/* MarketPlaceAPI.Models.ProductItem
 *
 * Define the model for the Product items where the information of the products
 * will be saved.
 */

using System.ComponentModel.DataAnnotations;

namespace MarketPlaceAPI.Models;


/// <summary>
/// Class <c>ProductItem</c> represents the model for the Products table.
/// </summary>
public class ProductItem
{

    // Columns of the product table.

    /// <value>
    /// Property <c>ProductId</c> is the key identification for the product.
    /// </value>
    [Key]
    public long ProductId { get; set; }

    /// <value>
    /// Property <c>Name</c> is the name of the product.
    /// </value>
    public string Name { get; set; } = default!;

    /// <value>
    /// Property <c>Life</c> if the time before perish of the product.
    /// </value>
    public int Life { get; set; }

    /// <value>
    /// Property <c>Stores</c> represent the Many-To-Many relation with the
    /// Stores table via StoreProduct table.
    /// Product WITH MANY Stores.
    /// </value>
    public ICollection<StoreProduct> Stores {
        get;
        set;
    } = new List<StoreProduct>();

    /// <value>
    /// Property <c>Tags</c> represent the Many-To-Many relation with Tags
    /// table via ProductTag table.
    /// Product HAS MANY Tags.
    /// </value>
    public ICollection<ProductTag> Tags {
        get;
        set;
    } = new List<ProductTag>();

}


/// <summary>
/// Class <c>ProductItemDTO</c> is the corresponding DTO for the products
/// model.
/// </summary>
public class ProductItemDTO
{

    // Columns of the product table.

    /// <value>
    /// Property <c>ProductId</c> is the key identification for the product.
    /// </value>
    [Key]
    public long ProductId { get; set; }

    /// <value>
    /// Property <c>Name</c> is the name of the product.
    /// </value>
    public string Name { get; set; } = default!;

    /// <value>
    /// Property <c>Life</c> if the time before perish of the product.
    /// </value>
    public int Life { get; set; }

}
