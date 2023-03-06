/* MarketPlaceAPI.Controllers.TagItemsController
 *
 * Controller class to administrate the items in the Tags table. This class
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
    /// Class <c>TagItemsController</c> routed in "/TagItems" creates,
    /// modifies, queries and deletes items from the Tags table in the
    /// database.
    /// </summary>
    /// <remarks>
    /// This class inherits from MarketItemsController class.
    /// </remarks>
    [Route("[controller]")]
    [ApiController]
    public class TagItemsController : MarketItemsController
    {

        /// <summary>
        /// This constructor just call the mother class constructor with the
        /// context object.
        /// </summary>
        public TagItemsController(MarketContext context) : base(context) { }

        /// <summary>
        /// Query all the elements in the Tags table as DTOs.
        /// </summary>
        /// <returns>
        /// NotFound if the Tag table is null. Otherwise it returns the list of
        /// tags in the table as DTOs.
        /// </returns>
        /// <remarks>
        /// Routed as <c>GET: /TagItems</c>.
        /// </remarks>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagItemDTO>>> GetTags()
        {

            // Check the Tags table exists.
            if (_context.Tags == null)
            {
                return NotFound();
            }

            // Return items in the Tags table as DTOs.
            return await _context.Tags.Select(t => tagToDTO(t)).ToListAsync();

        }

        /// <summary>
        /// Query the element in the Tags table with the given ID as DTO.
        /// (<paramref name="id"/>).
        /// </summary>
        /// <param name="id">Tag item ID.</param>
        /// <returns>
        /// NotFound if the Tags table is null or if the query for the given ID
        /// is null. Otherwise it returns the tag in the table that has the
        /// given ID as a DTO.
        /// </returns>
        /// <remarks>
        /// Routed as <c>GET: /TagItems/{id}</c>.
        /// <example>
        /// For example:
        /// <code>
        /// GET [API_URL]/TagItems/5
        /// </code>
        /// Will return element with ID=5 in the Tags table.
        /// </example>
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<ActionResult<TagItemDTO>> GetTagItem(long id)
        {

            // Check the Tags table exists.
            if (_context.Tags == null)
            {
                return NotFound();
            }

            // Find the item with the given ID.
            var tagItem = await _context.Tags.FindAsync(id);

            // Check the item exists.
            if (tagItem == null)
            {
                return NotFound();
            }

            // Return the queried item as DTO.
            return tagToDTO(tagItem);

        }

        /// <summary>
        /// Modify the tag item identified with the given ID.
        /// (<paramref name="id"/>, <paramref name="tagItemDTO"/>).
        /// </summary>
        /// <param name="id">Tag item ID.</param>
        /// <param name="tagItemDTO">Tag DTO structure with the modified
        /// info.</param>
        /// <returns>
        /// BadRequest if the ID in the request URL and in of the DTO object
        /// are different. NotFound if the Tags table is null, if the query for
        /// the given ID is null or if when updating, the tag item was not
        /// found. Otherwise it returns NoContent.
        /// </returns>
        /// <remarks>
        /// Routed as <c>PUT: /TagItems/{id}</c>.
        /// <example>
        /// For example:
        /// <code>
        /// PUT [API_URL]/TagItems/5
        /// </code>
        /// Along with a body which contain a TagItemDTO structure, will update
        /// the item that match ID=5 in the Tags table.
        /// </example>
        /// </remarks>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTagItem(
            long id,
            TagItemDTO tagItemDTO
        )
        {

            // Check if the request is valid.
            if (id != tagItemDTO.TagId)
            {
                return BadRequest();
            }

            // Check the Tags table exists.
            if (_context.Tags == null)
            {
                return NotFound();
            }

            // Find the item with the given ID.
            var tagItem = await _context.Tags.FindAsync(id);

            // Check the item exists.
            if (tagItem == null)
            {
                return NotFound();
            }

            // Update the item with the information in the DTO object.
            tagItem.Name = tagItemDTO.Name;

            // Try to save the changes.
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagItemExists(id))
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
        /// Create a new tag item in the table.
        /// (<paramref name="tagItemDTO"/>).
        /// </summary>
        /// <param name="tagItemDTO">New tag DTO structure to save in the
        /// table</param>
        /// <returns>
        /// NotFound if the Tags table is null. Otherwise it returns a DTO of
        /// the new item.
        /// </returns>
        /// <remarks>
        /// Routed as <c>POST: /TagItems</c>.
        /// <example>
        /// For example:
        /// <code>
        /// POST [API_URL]/TagItems
        /// </code>
        /// Along with a body which contain a TagItemDTO structure, will create
        /// a tag item in the Tags table.
        /// </example>
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<TagItemDTO>> PostTagItem(
            TagItemDTO tagItemDTO
        )
        {

            // Check the Tags table exists.
            if (_context.Tags == null)
            {
                return Problem("Entity set 'MarketContext.Tags' is null.");
            }

            // Create the new tag item with the DTO input object and empty
            // product association list.
            var tagItem = new TagItem
            {
                Name = tagItemDTO.Name,
                Products = new List<ProductTag>()
            };

            // Add the new item to the Tags table.
            _context.Tags.Add(tagItem);

            // Save the changes.
            await _context.SaveChangesAsync();

            // Return the DTO of the object.
            return CreatedAtAction(
                nameof(GetTagItem),
                new { id = tagItem.TagId },
                tagToDTO(tagItem)
            );

        }

        /// <summary>
        /// Delete the item identified with the given ID from the Tag table.
        /// (<paramref name="id"/>).
        /// </summary>
        /// <param name="id">Tag item ID.</param>
        /// <returns>
        /// NotFound if the Tags table is null or if the query for the
        /// given ID is null. Otherwise it returns NoContent.
        /// </returns>
        /// <remarks>
        /// Routed as <c>DELETE: /TagItems/{id}</c>.
        /// <example>
        /// For example:
        /// <code>
        /// DELETE [API_URL]/TagItems/5
        /// </code>
        /// Will deletes the item that match ID=5 from the Tags table.
        /// </example>
        /// </remarks>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTagItem(long id)
        {

            // Check the Tags table exists.
            if (_context.Tags == null)
            {
                return NotFound();
            }

            // Find the tag item with the given ID.
            var tagItem = await _context.Tags.FindAsync(id);

            // Check the tag item exists.
            if (tagItem == null)
            {
                return NotFound();
            }

            // Remove item from the table.
            _context.Tags.Remove(tagItem);

            // Save changes.
            await _context.SaveChangesAsync();

            // Return NoContent.
            return NoContent();

        }

        /// <summary>
        /// Check if the item with a given ID exists in the Tags table.
        /// (<paramref name="id"/>).
        /// </summary>
        /// <param name="id">Id of the requested object.</param>
        /// <returns>
        /// A bool indicating whether the item exists or not.
        /// </returns>
        private bool TagItemExists(long id)
        {

            return (_context.Tags?.Any(e => e.TagId == id))
                .GetValueOrDefault();

        }

    }


}
