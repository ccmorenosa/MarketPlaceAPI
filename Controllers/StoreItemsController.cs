/* MarketPlaceAPI.Controllers.StoreItemsController
 *
 * Controller class to administrate the items in the Stores table. This class
 * inherits from the MarketItemsController.
 */

// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarketPlaceAPI.Models;

namespace MarketPlaceAPI.Controllers
{

    /// <summary>
    /// Class <c>StoreItemsController</c> routed in "/StoreItems" creates,
    /// modifies, queries and deletes items from the Stores table in the
    /// database.
    /// </summary>
    /// <remarks>
    /// This class inherits from MarketItemsController class.
    /// </remarks>
    [Route("[controller]")]
    [ApiController]
    public class StoreItemsController : MarketItemsController
    {

        /// <summary>
        /// This constructor just call the mother class constructor with the
        /// context object.
        /// </summary>
        public StoreItemsController(MarketContext context) : base(context) { }

        /// <summary>
        /// Query all the elements in the Stores table as DTOs.
        /// </summary>
        /// <returns>
        /// NotFound if the Stores table is null. Otherwise it returns the
        /// list of store in the table as DTOs.
        /// </returns>
        /// <remarks>
        /// Routed as <c>GET: /StoreItems</c>.
        /// </remarks>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoreItemDTO>>> GetStores()
        {

            // Check the Stores table exists.
            if (_context.Stores == null)
            {
                return NotFound();
            }

            // Return items in the Stores table as DTOs.
            return await _context.Stores
                .Select(s => storeToDTO(s))
                .ToListAsync();

        }

        /// <summary>
        /// Query the element in the Stores table with the given ID as DTO.
        /// (<paramref name="id"/>).
        /// </summary>
        /// <param name="id">Store item ID.</param>
        /// <returns>
        /// NotFound if the Stores table is null or if the query for the given
        /// ID is null. Otherwise it returns the store in the table that has
        /// the given ID as a DTO.
        /// </returns>
        /// <remarks>
        /// Routed as <c>GET: /StoreItems/{id}</c>.
        /// <example>
        /// For example:
        /// <code>
        /// GET [API_URL]/StoreItems/5
        /// </code>
        /// Will return element with ID=5 in the Stores table.
        /// </example>
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<ActionResult<StoreItemDTO>> GetStoreItem(long id)
        {

            // Check the Stores table exists.
            if (_context.Stores == null)
            {
                return NotFound();
            }

            // Find the item with the given ID.
            var storeItem = await _context.Stores.FindAsync(id);

            // Check the item exists.
            if (storeItem == null)
            {
                return NotFound();
            }

            // Return the queried item as DTO.
            return storeToDTO(storeItem);

        }

        /// <summary>
        /// Query a list of products that are sold in the store identified with
        /// the given ID.
        /// (<paramref name="id"/>).
        /// </summary>
        /// <param name="id">Store item ID.</param>
        /// <returns>
        /// NotFound if the Stores table is null or if the query for the given
        /// ID is null. Otherwise it returns the list of products as a DTOs
        /// related with the store in the table that has the given ID.
        /// </returns>
        /// <remarks>
        /// Routed as <c>GET: /StoreItems/{id}/products</c>.
        /// <example>
        /// For example:
        /// <code>
        /// GET [API_URL]/StoreItems/5/products
        /// </code>
        /// Will return a list of products associates with the item that match
        /// ID=5 in the Stores table.
        /// </example>
        /// </remarks>
        [HttpGet("{id}/products")]
        public async
        Task<ActionResult<IEnumerable<ProductItemDTO>>> GetStoreItemProduct(
            long id
        )
        {

            // Check the Stores table exists.
            if (_context.Stores == null)
            {
                return NotFound();
            }

            // Find the item with the given ID.
            var storeItem = await _context.Stores.FindAsync(id);

            // Check the item exists.
            if (storeItem == null)
            {
                return NotFound();
            }

            // Query the list of products that are associated with the item.
            var storeProductItems = _context.Entry(storeItem)
                .Collection(p => p.Products)
                .Query();

            // Create the list that holds the products items as DTOs.
            var productItems = new List<ProductItemDTO>();

            // Collect all product items querying them by ID.
            foreach (var storeProductItem in storeProductItems)
            {
                // Query product by ID.
                var productItem = await _context.Products
                    .FindAsync(storeProductItem.ProductId);

                // If the product exists, add to the list.
                if (productItem != null)
                productItems.Add(productToDTO(productItem));

            }

            // Return the product list.
            return productItems;

        }

        /// <summary>
        /// Modify the store item identified with the given ID.
        /// (<paramref name="id"/>, <paramref name="storeItemDTO"/>).
        /// </summary>
        /// <param name="id">Store item ID.</param>
        /// <param name="storeItemDTO">Store DTO structure with the modified
        /// info.</param>
        /// <returns>
        /// BadRequest if the ID in the request URL and in of the DTO object
        /// are different. NotFound if the Stores table is null, if the query
        /// for the given ID is null or if when updating, the store item was
        /// not found. Otherwise it returns NoContent.
        /// </returns>
        /// <remarks>
        /// Routed as <c>PUT: /StoreItems/{id}</c>.
        /// <example>
        /// For example:
        /// <code>
        /// PUT [API_URL]/StoreItems/5
        /// </code>
        /// Along with a body which contain a StoreItemDTO structure, will
        /// update the item that match ID=5 in the Stores table.
        /// </example>
        /// </remarks>
        [HttpPut("{id}")]
        public async
        Task<IActionResult> PutStoreItem(long id, StoreItemDTO storeItemDTO)
        {

            // Check if the request is valid.
            if (id != storeItemDTO.StoreId)
            {
                return BadRequest();
            }

            // Check the Products table exists.
            if (_context.Products == null)
            {
                return NotFound();
            }

            // Find the item with the given ID.
            var storeItem = await _context.Stores.FindAsync(id);

            // Check the item exists.
            if (storeItem == null)
            {
                return NotFound();
            }

            // Update the item with the information in the DTO object.
            storeItem.Name = storeItemDTO.Name;
            storeItem.Currency = storeItemDTO.Currency;

            // Try to save the changes.
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

            // Return NoContent if the saving was successfull.
            return NoContent();

        }

        /// <summary>
        /// Associate the store item identified with the given ID and the
        /// product given. Also allow to modify the relation item.
        /// (
        ///     <paramref name="storeId"/>,
        ///     <paramref name="productId"/>,
        ///     <paramref name="productItemDTO"/>
        /// ).
        /// </summary>
        /// <param name="storeId">Store item ID.</param>
        /// <param name="productId">Product item ID.</param>
        /// <param name="productItemDTO">Product DTO structure with the product
        /// info. (TODO: Change to storeProductItem object!)</param>
        /// <returns>
        /// BadRequest if the ID in the request URL and in of the DTO object
        /// are different (deprecate). NotFound if the Stores table is null,
        /// if the queries for any of the given IDs (store and product) are
        /// null or if when updating, the store item was not found. Otherwise
        /// it returns NoContent.
        /// </returns>
        /// <remarks>
        /// Routed as <c>PUT: /StoreItems/{storeId}/AddProduct/{storeId}</c>.
        /// <example>
        /// For example:
        /// <code>
        /// PUT [API_URL]/StoreItems/5/AddProduct/3
        /// </code>
        /// Along with a body which contain a ProductItemDTO structure, will
        /// associate the store item that match ID=5 in the Stores table
        /// with the product item that match ID=3 in the Products table.
        /// </example>
        /// </remarks>
        [HttpPut("{storeId}/AddProduct/{productId}")]
        public async Task<IActionResult> PutProductStoreItem(
            long storeId,
            long productId,
            ProductItemDTO productItemDTO
        )
        {

            // Check if the request is valid.
            // TODO: Deprecate.
            if (productId != productItemDTO.ProductId)
            {
                return BadRequest();
            }

            // Find the store item with the given ID.
            var storeItem = await _context.Stores.FindAsync(storeId);

            // Check the store item exists.
            if (storeItem == null)
            {
                return NotFound();
            }

            // Find the product item with the given ID.
            var productItem = await _context.Products.FindAsync(productId);

            // Check the product item exists.
            if (productItem == null)
            {
                return NotFound();
            }

            // Create the new storeProductItem object to store the relation.
            // TODO: Verify if the relation exists and if so, get the item and
            // modify it.
            var storeProduct = new StoreProduct();

            // Add store and product.
            storeProduct.Store = storeItem;
            storeProduct.Product = productItem;

            // Add the relation to both store and product.
            storeItem.Products.Add(storeProduct);
            productItem.Stores.Add(storeProduct);

            // Try to save the changes.
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

            // Return NoContent if the saving was successfull.
            return NoContent();

        }

        /// <summary>
        /// Create a new store item in the table.
        /// (<paramref name="storeItemDTO"/>).
        /// </summary>
        /// <param name="storeItemDTO">New store DTO structure to save in the
        /// table</param>
        /// <returns>
        /// NotFound if the Stores table is null. Otherwise it returns a DTO of
        /// the new item.
        /// </returns>
        /// <remarks>
        /// Routed as <c>POST: /StoreItems</c>.
        /// <example>
        /// For example:
        /// <code>
        /// POST [API_URL]/StoreItems
        /// </code>
        /// Along with a body which contain a StoreItemDTO structure, will
        /// create a store item in the Stores table.
        /// </example>
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<StoreItemDTO>> PostStoreItem(
            StoreItemDTO storeItemDTO
        )
        {

            // Check the Stores table exists.
            if (_context.Stores == null)
            {
                return Problem("Entity set 'MarketContext.Stores' is null.");
            }

            // Create the new store item with the DTO input object and empty
            // product association list.
            var storeItem = new StoreItem
            {
                Name = storeItemDTO.Name,
                Currency = storeItemDTO.Currency,
                Products = new List<StoreProduct>()
            };

            // Add the new item to the Stores table.
            _context.Stores.Add(storeItem);

            // Save the changes.
            await _context.SaveChangesAsync();

            // Return the DTO of the object.
            return CreatedAtAction(
                nameof(GetStoreItem),
                new { id = storeItem.StoreId },
                storeToDTO(storeItem)
            );

        }

        /// <summary>
        /// Delete the item identified with the given ID from the Stores table.
        /// (<paramref name="id"/>).
        /// </summary>
        /// <param name="id">Store item ID.</param>
        /// <returns>
        /// NotFound if the Stores table is null or if the query for the given
        /// ID is null. Otherwise it returns NoContent.
        /// </returns>
        /// <remarks>
        /// Routed as <c>DELETE: /StoreItems/{id}</c>.
        /// <example>
        /// For example:
        /// <code>
        /// DELETE [API_URL]/StoreItems/5
        /// </code>
        /// Will deletes the item that match ID=5 from the Stores table.
        /// </example>
        /// </remarks>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStoreItem(long id)
        {

            // Check the Products table exists.
            if (_context.Stores == null)
            {
                return NotFound();
            }

            // Find the store item with the given ID.
            var storeItem = await _context.Stores.FindAsync(id);

            // Check the store item exists.
            if (storeItem == null)
            {
                return NotFound();
            }

            // Remove item from the table.
            _context.Stores.Remove(storeItem);

            // Save changes.
            await _context.SaveChangesAsync();

            // Return NoContent.
            return NoContent();

        }

        /// <summary>
        /// Check if the item with a given ID exists in the Stores table.
        /// (<paramref name="id"/>).
        /// </summary>
        /// <param name="id">Id of the requested object.</param>
        /// <returns>
        /// A bool indicating whether the item exists or not.
        /// </returns>
        private bool StoreItemExists(long id)
        {

            return (_context.Stores?.Any(e => e.StoreId == id))
                .GetValueOrDefault();

        }

    }


}
