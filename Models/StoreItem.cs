/*
    Define the table for the Store items where the information of the stores
    will be saved.
*/
using System.ComponentModel.DataAnnotations;
namespace  MarketPlaceAPI.Models;


/**
    Define class for the item in Store table.
*/
public class StoreItem
{

    // Columns of the store table.
    [Key]
    public long StoreId { get; set; }
    public string Name { get; set; } = default!;
    public string Currency { get; set; } = "EUR";

    // Many-To-Many relation with products table via StoreProduct table.
    // Store HAS MANY Products.
    public ICollection<StoreProduct> Products {
        get;
        set;
    } = new List<StoreProduct>();

}


/**
    Define DTO class.
*/
public class StoreItemDTO
{

    // Columns of the store table.
    [Key]
    public long StoreId { get; set; }
    public string Name { get; set; } = default!;
    public string Currency { get; set; } = "EUR";

}
