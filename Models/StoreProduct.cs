/*
    Table defining the relation between Stores and Products.
*/
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceAPI.Models;


/*
    Define class for the relation between Store and Product.
*/
public class StoreProduct
{

    // Id and item for the Store table.
    public long StoreId { get; set; }
    public StoreItem Store { get; set; } = new StoreItem();

    // Id and item for the Product table.
    public long ProductId { get; set; }
    public ProductItem Product { get; set; } = new ProductItem();

    // Include price for this combination.
    public decimal price { get; set; }

}
