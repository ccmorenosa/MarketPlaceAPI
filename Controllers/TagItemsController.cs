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
    public class TagItemsController : ControllerBase
    {
        private readonly MarketContext _context;

        public TagItemsController(MarketContext context)
        {
            _context = context;
        }

        // GET: api/TagItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagItem>>> GetTags()
        {
          if (_context.Tags == null)
          {
              return NotFound();
          }
            return await _context.Tags.ToListAsync();
        }

        // GET: api/TagItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TagItem>> GetTagItem(long id)
        {
          if (_context.Tags == null)
          {
              return NotFound();
          }
            var tagItem = await _context.Tags.FindAsync(id);

            if (tagItem == null)
            {
                return NotFound();
            }

            return tagItem;
        }

        // PUT: api/TagItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTagItem(long id, TagItem tagItem)
        {
            if (id != tagItem.TagId)
            {
                return BadRequest();
            }

            _context.Entry(tagItem).State = EntityState.Modified;

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

            return NoContent();
        }

        // POST: api/TagItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TagItem>> PostTagItem(TagItem tagItem)
        {
          if (_context.Tags == null)
          {
              return Problem("Entity set 'MarketContext.Tags'  is null.");
          }
            _context.Tags.Add(tagItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTagItem), new { id = tagItem.TagId }, tagItem);
        }

        // DELETE: api/TagItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTagItem(long id)
        {
            if (_context.Tags == null)
            {
                return NotFound();
            }
            var tagItem = await _context.Tags.FindAsync(id);
            if (tagItem == null)
            {
                return NotFound();
            }

            _context.Tags.Remove(tagItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TagItemExists(long id)
        {
            return (_context.Tags?.Any(e => e.TagId == id)).GetValueOrDefault();
        }
    }
}
