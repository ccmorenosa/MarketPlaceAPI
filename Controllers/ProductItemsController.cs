using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarketPlaceAPI.Models;

namespace MarketPlaceAPI.Controllers
{


    [Route("[controller]")]
    [ApiController]
    public class ProductItemsController : MarketItemsController
    {
        public ProductItemsController(MarketContext context)
            : base(context) { }

        // GET: ProductItems
        [HttpGet]
        public async
        Task<ActionResult<IEnumerable<ProductItemDTO>>> GetProducts()
        {

            if (_context.Products == null)
            {
                return NotFound();
            }

            return await _context.Products
                .Select(p => productToDTO(p))
                .ToListAsync();

        }

        // GET: ProductItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductItemDTO>> GetProductItem(long id)
        {

            if (_context.Products == null)
            {
                return NotFound();
            }

            var productItem = await _context.Products.FindAsync(id);

            if (productItem == null)
            {
                return NotFound();
            }

            return productToDTO(productItem);

        }

        // GET: ProductItems/5/stores
        [HttpGet("{id}/stores")]
        public async
        Task<ActionResult<IEnumerable<StoreItemDTO>>> GetProductItemStores (
            long id
        )
        {

            if (_context.Products == null)
            {
                return NotFound();
            }

            var productItem = await _context.Products
                .SingleAsync(p => p.ProductId == id);

            var storeProductItems = _context.Entry(productItem)
                .Collection(p => p.Stores)
                .Query();

            var storeItems = new List<StoreItemDTO>();

            foreach (var storeProductItem in storeProductItems)
            {
                var storeItem = await _context.Stores
                    .FindAsync(storeProductItem.StoreId);

                if (storeItem != null)
                storeItems.Add(storeToDTO(storeItem));

            }

            return storeItems;

        }

        // PUT: ProductItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult> PutProductItem(
            long id,
            ProductItemDTO productItemDTO
        )
        {

            if (id != productItemDTO.ProductId)
            {
                return BadRequest();
            }

            var productItem = await _context.Products.FindAsync(id);
            if (productItem == null)
            {
                return NotFound();
            }

            productItem.Name = productItemDTO.Name;
            productItem.Life = productItemDTO.Life;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();

        }

        // PUT: ProductItems/3/AddStore/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{productId}/AddStore/{storeId}")]
        public async Task<IActionResult> PutProductStoreItem(
            long productId,
            long storeId,
            StoreItemDTO storeItemDTO
        )
        {

            if (storeId != storeItemDTO.StoreId)
            {
                return BadRequest();
            }

            var productItem = await _context.Products.FindAsync(productId);
            if (productItem == null)
            {
                return NotFound();
            }

            var storeItem = await _context.Stores.FindAsync(storeId);
            if (storeItem == null)
            {
                return NotFound();
            }

            var storeProduct = new StoreProduct();
            storeProduct.Store = storeItem;
            storeProduct.Product = productItem;

            productItem.Stores.Add(storeProduct);
            storeItem.Products.Add(storeProduct);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductItemExists(storeId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();

        }

        // POST: ProductItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductItemDTO>> PostProductItem(
            ProductItemDTO productItemDTO
        )
        {

            if (_context.Products == null)
            {
                return Problem("Entity set 'MarketContext.Products' is null.");
            }

            var productItem = new ProductItem
            {
                Name = productItemDTO.Name,
                Life = productItemDTO.Life,
                Stores = new List<StoreProduct>(),
                Tags = new List<ProductTag>()
            };

            _context.Products.Add(productItem);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetProductItem),
                new { id = productItem.ProductId },
                productToDTO(productItem)
            );

        }

        // DELETE: ProductItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductItem(long id)
        {

            if (_context.Products == null)
            {
                return NotFound();
            }

            var productItem = await _context.Products.FindAsync(id);
            if (productItem == null)
            {
                return NotFound();
            }

            _context.Products.Remove(productItem);

            await _context.SaveChangesAsync();

            return NoContent();

        }

        private bool ProductItemExists(long id)
        {

            return (_context.Products?.Any(e => e.ProductId == id))
                .GetValueOrDefault();

        }

    }


}
