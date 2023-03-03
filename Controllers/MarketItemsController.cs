using Microsoft.AspNetCore.Mvc;
using MarketPlaceAPI.Models;

namespace MarketPlaceAPI.Controllers
{

    public class MarketItemsController : ControllerBase
    {
        protected readonly MarketContext _context;

        public MarketItemsController(MarketContext context)
        {
            _context = context;
        }

        protected static StoreItemDTO storeToDTO(StoreItem storeItem) =>
        new StoreItemDTO
        {
            StoreId = storeItem.StoreId,
            Name = storeItem.Name,
            Currency = storeItem.Currency
        };

        protected static ProductItemDTO productToDTO(ProductItem productItem) =>
        new ProductItemDTO
        {
            ProductId = productItem.ProductId,
            Name = productItem.Name,
            Life = productItem.Life
        };

        protected static TagItemDTO tagToDTO(TagItem tagItem) =>
        new TagItemDTO
        {
            TagId = tagItem.TagId,
            Name = tagItem.Name
        };

    }

}
