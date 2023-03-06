/* MarketPlaceAPI.Models.TagsItem
 *
 * Define the model for the Tag items where the information of the tags will be
 * saved.
 */

using System.ComponentModel.DataAnnotations;

namespace  MarketPlaceAPI.Models;


/// <summary>
/// Class <c>TagItem</c> represents the model for the Tags table.
/// </summary>
public class TagItem
{

    // Columns of the tag table.

    /// <value>
    /// Property <c>TagId</c> is the key identification for the tag.
    /// </value>
    [Key]
    public long TagId { get; set; }

    /// <value>
    /// Property <c>Name</c> is the name of the tag.
    /// </value>
    public string Name { get; set; } = default!;

    /// <value>
    /// Property <c>Products</c> represent the Many-To-Many relation with the
    /// Products table via ProductTag table.
    /// Tag WITH MANY Products.
    /// </value>
    public ICollection<ProductTag> Products {
        get;
        set;
    } = new List<ProductTag>();

}


/// <summary>
/// Class <c>TagItemDTO</c> is the corresponding DTO for the tags model.
/// </summary>
public class TagItemDTO
{

    // Columns of the tag table.

    /// <value>
    /// Property <c>TagId</c> is the key identification for the tag.
    /// </value>
    [Key]
    public long TagId { get; set; }

    /// <value>
    /// Property <c>Name</c> is the name of the tag.
    /// </value>
    public string Name { get; set; } = default!;

}
