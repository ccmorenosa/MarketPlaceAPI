/* MarketPlaceAPI.Models.ProductTag
 *
 * Define the model that holds the relation between Products and Tags.
 */

namespace MarketPlaceAPI.Models;


/// <summary>
/// Class <c>ProductTag</c> represents the Many To Many relation between
/// Products and Tags tables.
/// </summary>
public class ProductTag
{

    // Id and item for the Product table.
    public long ProductId { get; set; }
    public ProductItem Product { get; set; } = default!;

    // Id and item for the Tag table.
    public long TagId { get; set; }
    public TagItem Tag { get; set; } = default!;

}
