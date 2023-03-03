/*
    Define the table for the Tag items where the information of the
    tags will be saved.
*/
using System.ComponentModel.DataAnnotations;

namespace  MarketPlaceAPI.Models;


/**
    Define class for the item in Product table.
*/
public class TagItem
{

    // Columns of the tag table.
    [Key]
    public long TagId { get; set; }
    public string Name { get; set; } = default!;

    // Many-To-Many relation with product table via ProductTag table.
    // Tag WITH MANY Products.
    public ICollection<ProductTag> Products {
        get;
        set;
    } = new List<ProductTag>();

}


/**
    Define DTO class.
*/
public class TagItemDTO
{

    // Columns of the tag table.
    [Key]
    public long TagId { get; set; }
    public string Name { get; set; } = default!;

}
