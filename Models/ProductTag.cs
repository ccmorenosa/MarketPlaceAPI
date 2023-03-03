/*
    Table defining the relation between Products and Tags.
*/
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceAPI.Models;

/*
    Define class for the relation between Product and Tag.
*/
public class ProductTag
{

    // Id and item for the Product table.
    public long ProductId { get; set; }
    public ProductItem Product { get; set; } = default!;

    // Id and item for the Tag table.
    public long TagId { get; set; }
    public TagItem Tag { get; set; } = default!;

}
