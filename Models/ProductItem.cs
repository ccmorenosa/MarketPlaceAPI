/*
    Define the table for the Product items where the information of the
    products will be saved.
*/
using System.ComponentModel.DataAnnotations;

namespace  MarketPlaceAPI.Models;


/**
    Define class for the item in Product table.
*/
public class ProductItem
{

    // Columns of the product table.
    [Key]
    public long ProductId { get; set; }
    public string Name { get; set; } = default!;
    public int Life { get; set; }

    // Many-To-Many relation with stores table via StoreProduct table.
    // Product WITH MANY Stores.
    public ICollection<StoreProduct> Stores {
        get;
        set;
    } = new List<StoreProduct>();

    // Many-To-Many relation with tag table via ProductTag table.
    // Product HAS MANY Tags.
    public ICollection<ProductTag> Tags {
        get;
        set;
    } = new List<ProductTag>();

}


/**
    Define DTO class.
*/
public class ProductItemDTO
{

    // Columns of the product table.
    [Key]
    public long ProductId { get; set; }
    public string Name { get; set; } = default!;
    public int Life { get; set; }

}
