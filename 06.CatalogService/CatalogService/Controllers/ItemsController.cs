using AutoMapper;
using CatalogService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Controllers
{
    [Route("v1/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly CategoryDbContext _context;

        public ItemsController(IMapper mapper, CategoryDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            if (_context.Items == null)
            {
                return NotFound();
            }
            var items =  await _context.Items.ToListAsync();
            var itemsMapped = _mapper.Map<IList<Item>>(items);

            return Ok(itemsMapped);
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(long id)
        {
            if (_context.Items == null)
            {
                return NotFound();
            }
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // PUT: api/Items/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(long id, Item item)
        {
            if (id != item.ItemId)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
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

        // POST: api/Items
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(ItemUpdate item)
        {
            if (_context.Items == null)
            {
                return Problem("Entity set 'CategoryDbContext.Item'  is null.");
            }
            var itemBs = _mapper.Map<Item>(item);
            _context.Items.Add(itemBs);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.ItemId }, item);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(long id)
        {
            if (_context.Items == null)
            {
                return NotFound();
            }
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemExists(long id)
        {
            return (_context.Items?.Any(e => e.ItemId == id)).GetValueOrDefault();
        }
    }
}
