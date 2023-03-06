/* MarketPlaceAPI.Controllers.ProductItemsController
 *
 * Controller class to administrate the items in the Products table. This class
 * inherits from the MarketItemsController class.
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
    /// Class <c>ProductItemsController</c> routed in "/ProductItems" creates,
    /// modifies, queries and deletes items from the Products table in the
    /// database.
    /// </summary>
    /// <remarks>
    /// This class inherits from MarketItemsController class.
    /// </remarks>
    [Route("[controller]")]
    [ApiController]
    public class ProductItemsController : MarketItemsController
    {

        /// <summary>
        /// This constructor just call the mother class constructor with the
        /// context object.
        /// </summary>
        public ProductItemsController(MarketContext context)
            : base(context) { }

        /// <summary>
        /// Query all the elements in the Products table as DTOs.
        /// </summary>
        /// <returns>
        /// NotFound if the Products table is null. Otherwise it returns the
        /// list of products in the table as DTOs.
        /// </returns>
        /// <remarks>
        /// Routed as <c>GET: /ProductItems</c>.
        /// </remarks>
        [HttpGet]
        public async
        Task<ActionResult<IEnumerable<ProductItemDTO>>> GetProducts()
        {

            // Check the Products table exists.
            if (_context.Products == null)
            {
                return NotFound();
            }

            // Return items in the Products table as DTOs.
            return await _context.Products
                .Select(p => productToDTO(p))
                .ToListAsync();

        }

        /// <summary>
        /// Query the element in the Products table with the given ID as DTO.
        /// (<paramref name="id"/>).
        /// </summary>
        /// <param name="id">Product item ID.</param>
        /// <returns>
        /// NotFound if the Products table is null or if the query for the
        /// given ID is null. Otherwise it returns the product in the table
        /// that has the given ID as a DTO.
        /// </returns>
        /// <remarks>
        /// Routed as <c>GET: /ProductItems/{id}</c>.
        /// <example>
        /// For example:
        /// <code>
        /// GET [API_URL]/ProductItems/5
        /// </code>
        /// Will return element with ID=5 in the Products table.
        /// </example>
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductItemDTO>> GetProductItem(long id)
        {

            // Check the Products table exists.
            if (_context.Products == null)
            {
                return NotFound();
            }

            // Find the item with the given ID.
            var productItem = await _context.Products.FindAsync(id);

            // Check the item exists.
            if (productItem == null)
            {
                return NotFound();
            }

            // Return the queried item as DTO.
            return productToDTO(productItem);

        }

        /// <summary>
        /// Query a list of stores that sell the product identified with the
        /// given ID.
        /// (<paramref name="id"/>).
        /// </summary>
        /// <param name="id">Product item ID.</param>
        /// <returns>
        /// NotFound if the Products table is null or if the query for the
        /// given ID is null. Otherwise it returns the list of stores as a DTOs
        /// related with the product in the table that has the given ID.
        /// </returns>
        /// <remarks>
        /// Routed as <c>GET: /ProductItems/{id}/stores</c>.
        /// <example>
        /// For example:
        /// <code>
        /// GET [API_URL]/ProductItems/5/stores
        /// </code>
        /// Will return a list of stores associates with the item that match
        /// ID=5 in the Products table.
        /// </example>
        /// </remarks>
        [HttpGet("{id}/stores")]
        public async
        Task<ActionResult<IEnumerable<StoreItemDTO>>> GetProductItemStores (
            long id
        )
        {

            // Check the Products table exists.
            if (_context.Products == null)
            {
                return NotFound();
            }

            // Find the item with the given ID.
            var productItem = await _context.Products.FindAsync(id);

            // Check the item exists.
            if (productItem == null)
            {
                return NotFound();
            }

            // Query the list of stores that are associated with the item.
            var storeProductItems = _context.Entry(productItem)
                .Collection(p => p.Stores)
                .Query();

            // Create the list that holds the store items as DTOs.
            var storeItems = new List<StoreItemDTO>();

            // Collect all store items querying them by ID.
            foreach (var storeProductItem in storeProductItems)
            {

                // Query store by ID.
                var storeItem = await _context.Stores
                    .FindAsync(storeProductItem.StoreId);

                // If the store exists, add to the list.
                if (storeItem != null)
                storeItems.Add(storeToDTO(storeItem));

            }

            // Return the store list.
            return storeItems;

        }

        /// <summary>
        /// Modify the product item identified with the given ID.
        /// (<paramref name="id"/>, <paramref name="productItemDTO"/>).
        /// </summary>
        /// <param name="id">Product item ID.</param>
        /// <param name="productItemDTO">Product DTO structure with the
        /// modified info.</param>
        /// <returns>
        /// BadRequest if the ID in the request URL and in of the DTO object
        /// are different. NotFound if the Products table is null, if the query
        /// for the given ID is null or if when updating, the product item was
        /// not found. Otherwise it returns NoContent.
        /// </returns>
        /// <remarks>
        /// Routed as <c>PUT: /ProductItems/{id}</c>.
        /// <example>
        /// For example:
        /// <code>
        /// PUT [API_URL]/ProductItems/5
        /// </code>
        /// Along with a body which contain a ProductItemDTO structure, will
        /// update the item that match ID=5 in the Products table.
        /// </example>
        /// </remarks>
        [HttpPut("{id}")]
        public async Task<ActionResult> PutProductItem(
            long id,
            ProductItemDTO productItemDTO
        )
        {

            // Check if the request is valid.
            if (id != productItemDTO.ProductId)
            {
                return BadRequest();
            }

            // Check the Products table exists.
            if (_context.Products == null)
            {
                return NotFound();
            }

            // Find the item with the given ID.
            var productItem = await _context.Products.FindAsync(id);

            // Check the item exists.
            if (productItem == null)
            {
                return NotFound();
            }

            // Update the item with the information in the DTO object.
            productItem.Name = productItemDTO.Name;
            productItem.Life = productItemDTO.Life;

            // Try to save the changes.
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

            // Return NoContent if the saving was successfull.
            return NoContent();

        }

        /// <summary>
        /// Associate the product item identified with the given ID and the
        /// store given. Also allow to modify the relation item.
        /// (
        ///     <paramref name="productId"/>,
        ///     <paramref name="storeId"/>,
        ///     <paramref name="storeItemDTO"/>
        /// ).
        /// </summary>
        /// <param name="productId">Product item ID.</param>
        /// <param name="storeId">Store item ID.</param>
        /// <param name="storeItemDTO">Store DTO structure with the store info.
        /// (TODO: Change to storeProductItem object!)</param>
        /// <returns>
        /// BadRequest if the ID in the request URL and in of the DTO object
        /// are different (deprecate). NotFound if the Products table is null,
        /// if the queries for any of the given IDs (product and store) are
        /// null or if when updating, the product item was not found. Otherwise
        /// it returns NoContent.
        /// </returns>
        /// <remarks>
        /// Routed as <c>PUT: /ProductItems/{productId}/AddStore/{storeId}</c>.
        /// <example>
        /// For example:
        /// <code>
        /// PUT [API_URL]/ProductItems/5/AddStore/3
        /// </code>
        /// Along with a body which contain a StoreItemDTO structure, will
        /// associate the product item that match ID=5 in the Products table
        /// with the store item that match ID=3 in the Stores table.
        /// </example>
        /// </remarks>
        [HttpPut("{productId}/AddStore/{storeId}")]
        public async Task<IActionResult> PutProductStoreItem(
            long productId,
            long storeId,
            StoreItemDTO storeItemDTO
        )
        {

            // Check if the request is valid.
            // TODO: Deprecate.
            if (storeId != storeItemDTO.StoreId)
            {
                return BadRequest();
            }

            // Find the product item with the given ID.
            var productItem = await _context.Products.FindAsync(productId);

            // Check the product item exists.
            if (productItem == null)
            {
                return NotFound();
            }

            // Find the store item with the given ID.
            var storeItem = await _context.Stores.FindAsync(storeId);

            // Check the store item exists.
            if (storeItem == null)
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
            productItem.Stores.Add(storeProduct);
            storeItem.Products.Add(storeProduct);

            // Try to save the changes.
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

            // Return NoContent if the saving was successfull.
            return NoContent();

        }

        /// <summary>
        /// Create a new product item in the table.
        /// (<paramref name="productItemDTO"/>).
        /// </summary>
        /// <param name="productItemDTO">New product DTO structure to save in
        /// the table</param>
        /// <returns>
        /// NotFound if the Products table is null. Otherwise it returns a DTO
        /// of the new item.
        /// </returns>
        /// <remarks>
        /// Routed as <c>POST: /ProductItems</c>.
        /// <example>
        /// For example:
        /// <code>
        /// POST [API_URL]/ProductItems
        /// </code>
        /// Along with a body which contain a ProductItemDTO structure, will
        /// create a product item in the Products table.
        /// </example>
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<ProductItemDTO>> PostProductItem(
            ProductItemDTO productItemDTO
        )
        {

            // Check the Products table exists.
            if (_context.Products == null)
            {
                return Problem("Entity set 'MarketContext.Products' is null.");
            }

            // Create the new product item with the DTO input object and empty
            // store/tag association lists.
            var productItem = new ProductItem
            {
                Name = productItemDTO.Name,
                Life = productItemDTO.Life,
                Stores = new List<StoreProduct>(),
                Tags = new List<ProductTag>()
            };

            // Add the new item to the Products table.
            _context.Products.Add(productItem);

            // Save the changes.
            await _context.SaveChangesAsync();

            // Return the DTO of the object.
            return CreatedAtAction(
                nameof(GetProductItem),
                new { id = productItem.ProductId },
                productToDTO(productItem)
            );

        }

        /// <summary>
        /// Delete the item identified with the given ID from the Products
        /// table.
        /// (<paramref name="id"/>).
        /// </summary>
        /// <param name="id">Product item ID.</param>
        /// <returns>
        /// NotFound if the Products table is null or if the query for the
        /// given ID is null. Otherwise it returns NoContent.
        /// </returns>
        /// <remarks>
        /// Routed as <c>DELETE: /ProductItems/{id}</c>.
        /// <example>
        /// For example:
        /// <code>
        /// DELETE [API_URL]/ProductItems/5
        /// </code>
        /// Will deletes the item that match ID=5 from the Products table.
        /// </example>
        /// </remarks>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductItem(long id)
        {

            // Check the Products table exists.
            if (_context.Products == null)
            {
                return NotFound();
            }

            // Find the product item with the given ID.
            var productItem = await _context.Products.FindAsync(id);

            // Check the product item exists.
            if (productItem == null)
            {
                return NotFound();
            }

            // Remove item from the table.
            _context.Products.Remove(productItem);

            // Save changes.
            await _context.SaveChangesAsync();

            // Return NoContent.
            return NoContent();

        }

        /// <summary>
        /// Check if the item with a given ID exists in the Products table.
        /// (<paramref name="id"/>).
        /// </summary>
        /// <param name="id">Id of the requested object.</param>
        /// <returns>
        /// A bool indicating whether the item exists or not.
        /// </returns>
        private bool ProductItemExists(long id)
        {

            return (_context.Products?.Any(e => e.ProductId == id))
                .GetValueOrDefault();

        }

    }


}
