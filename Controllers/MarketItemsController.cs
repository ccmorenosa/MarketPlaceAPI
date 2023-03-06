/* MarketPlaceAPI.Controllers.MarketItemsController
 *
 * Base controller for the tables of the Market database containing common
 * functions and properties shared by the other controllers.
 *
 */

using Microsoft.AspNetCore.Mvc;
using MarketPlaceAPI.Models;

namespace MarketPlaceAPI.Controllers
{


    /// <summary>
    /// Class <c>MarketItemsController</c> is a base class for the Market
    /// database. It contains functions to convert from tableItems to
    /// tableItemsDTO.
    /// </summary>
    /// <remarks>
    /// It contains common functions used along all the controllers in the
    /// database.
    /// </remarks>
    public class MarketItemsController : ControllerBase
    {

        /// <value>
        /// Property <c>_context</c> is a BdContext that represent the
        /// database.
        /// </value>
        protected readonly MarketContext _context;

        /// <summary>
        /// This constructor assigns the MarketContext object to the class
        /// corresponding property.
        /// </summary>
        public MarketItemsController(MarketContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Convert a StoreItem instance into a StoreItemDTO instance.
        /// (<paramref name="storeItem"/>).
        /// </summary>
        /// <param name="storeItem">Store table item.</param>
        /// <returns>
        /// A StoreItemDTO object with no information about the products of the
        /// store.
        /// </returns>
        protected static StoreItemDTO storeToDTO(
            StoreItem storeItem
        ) => new StoreItemDTO
        {
            StoreId = storeItem.StoreId,
            Name = storeItem.Name,
            Currency = storeItem.Currency
        };

        /// <summary>
        /// Convert a ProductItem instance into a ProductItemDTO instance.
        /// (<paramref name="productItem"/>).
        /// </summary>
        /// <param name="productItem">Product table item.</param>
        /// <returns>
        /// A ProductItemDTO object with no information about the stores that
        /// sells the product, nor the tags of the product.
        /// </returns>
        protected static ProductItemDTO productToDTO(
            ProductItem productItem
        ) => new ProductItemDTO
        {
            ProductId = productItem.ProductId,
            Name = productItem.Name,
            Life = productItem.Life
        };

        /// <summary>
        /// Convert a TagItem instance into a TagItemDTO instance.
        /// (<paramref name="tagItem"/>).
        /// </summary>
        /// <param name="tagItem">Tag table item.</param>
        /// <returns>
        /// A TagItemDTO object with no information about the products
        /// associated with the tag.
        /// </returns>
        protected static TagItemDTO tagToDTO(
            TagItem tagItem
        ) => new TagItemDTO
        {
            TagId = tagItem.TagId,
            Name = tagItem.Name
        };

    }


}
