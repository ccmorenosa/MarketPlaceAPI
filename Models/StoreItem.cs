/* MarketPlaceAPI.Models.StoreItem
 *
 * Define the model for the Store items where the information of the stores
 * will be saved.
 */

using System.ComponentModel.DataAnnotations;
namespace  MarketPlaceAPI.Models;


/// <summary>
/// Class <c>StoreItem</c> represents the model for the Stores table.
/// </summary>
public class StoreItem
{

    // Columns of the store table.

    /// <value>
    /// Property <c>StoreId</c> is the key identification for the store.
    /// </value>
    [Key]
    public long StoreId { get; set; }

    /// <value>
    /// Property <c>Name</c> is the name of the store.
    /// </value>
    public string Name { get; set; } = default!;

    /// <value>
    /// Property <c>Currency</c> is the currency used in the store.
    /// </value>
    public string Currency { get; set; } = "EUR";

    /// <value>
    /// Property <c>Products</c> represent the Many-To-Many relation with the
    /// Products table via StoreProduct table.
    /// Store HAS MANY Products.
    /// </value>
    public ICollection<StoreProduct> Products {
        get;
        set;
    } = new List<StoreProduct>();

}


/// <summary>
/// Class <c>StoreItemDTO</c> is the corresponding DTO for the stores model.
/// </summary>
public class StoreItemDTO
{

    // Columns of the store table.

    /// <value>
    /// Property <c>StoreId</c> is the key identification for the store.
    /// </value>
    [Key]
    public long StoreId { get; set; }

    /// <value>
    /// Property <c>Name</c> is the name of the store.
    /// </value>
    public string Name { get; set; } = default!;

    /// <value>
    /// Property <c>Currency</c> is the currency used in the store.
    /// </value>
    public string Currency { get; set; } = "EUR";

}
