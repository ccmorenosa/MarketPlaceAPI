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
    public class StoreItemsController : MarketItemsController
    {

        public StoreItemsController(MarketContext context) : base(context) { }

        // GET: StoreItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoreItemDTO>>> GetStores()
        {

            if (_context.Stores == null)
            {
                return NotFound();
            }

            return await _context.Stores
                .Select(s => storeToDTO(s))
                .ToListAsync();

        }

        // GET: StoreItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StoreItemDTO>> GetStoreItem(long id)
        {

            if (_context.Stores == null)
            {
                return NotFound();
            }

            var storeItem = await _context.Stores.FindAsync(id);

            if (storeItem == null)
            {
                return NotFound();
            }

            return storeToDTO(storeItem);

        }

        // GET: StoreItems/5/products
        [HttpGet("{id}/products")]
        public async
        Task<ActionResult<IEnumerable<ProductItemDTO>>> GetStoreItemProduct(
            long id
        )
        {

            if (_context.Stores == null)
            {
                return NotFound();
            }

            var storesItem = await _context.Stores
                .SingleAsync(p => p.StoreId == id);

            var storeProductItems = _context.Entry(storesItem)
                .Collection(p => p.Products)
                .Query();

            var productItems = new List<ProductItemDTO>();

            foreach (var storeProductItem in storeProductItems)
            {
                var productItem = await _context.Products
                    .FindAsync(storeProductItem.ProductId);

                if (productItem != null)
                productItems.Add(productToDTO(productItem));

            }

            return productItems;

        }

        // PUT: StoreItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async
        Task<IActionResult> PutStoreItem(long id, StoreItemDTO storeItemDTO)
        {

            if (id != storeItemDTO.StoreId)
            {
                return BadRequest();
            }

            var storeItem = await _context.Stores.FindAsync(id);
            if (storeItem == null)
            {
                return NotFound();
            }

            storeItem.Name = storeItemDTO.Name;
            storeItem.Currency = storeItemDTO.Currency;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreItemExists(id))
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

        // PUT: StoreItems/AddProduct/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{storeId}/AddProduct/{productId}")]
        public async Task<IActionResult> PutProductStoreItem(
            long storeId,
            long productId,
            ProductItemDTO productItemDTO
        )
        {

            if (productId != productItemDTO.ProductId)
            {
                return BadRequest();
            }

            var storeItem = await _context.Stores.FindAsync(storeId);
            if (storeItem == null)
            {
                return NotFound();
            }

            var productItem = await _context.Products.FindAsync(productId);
            if (productItem == null)
            {
                return NotFound();
            }

            var storeProduct = new StoreProduct();
            storeProduct.Store = storeItem;
            storeProduct.Product = productItem;

            storeItem.Products.Add(storeProduct);
            productItem.Stores.Add(storeProduct);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreItemExists(storeId))
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

        // POST: StoreItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StoreItemDTO>> PostStoreItem(
            StoreItemDTO storeItemDTO
        )
        {

            if (_context.Stores == null)
            {
                return Problem("Entity set 'MarketContext.Stores' is null.");
            }

            var storeItem = new StoreItem
            {
                Name = storeItemDTO.Name,
                Currency = storeItemDTO.Currency,
                Products = new List<StoreProduct>()
            };

            _context.Stores.Add(storeItem);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetStoreItem),
                new { id = storeItem.StoreId },
                storeToDTO(storeItem)
            );

        }

        // DELETE: StoreItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStoreItem(long id)
        {
            if (_context.Stores == null)
            {
                return NotFound();
            }

            var storeItem = await _context.Stores.FindAsync(id);
            if (storeItem == null)
            {
                return NotFound();
            }

            _context.Stores.Remove(storeItem);

            await _context.SaveChangesAsync();

            return NoContent();

        }

        private bool StoreItemExists(long id)
        {

            return (_context.Stores?.Any(e => e.StoreId == id))
                .GetValueOrDefault();

        }

    }


}
