using AutoMapper;
using CatalogService.Data;
using CatalogService.Models.App;
using CatalogService.Models.EF;
using CatalogService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Controllers
{
    [Route("v1/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ItemService _itemService;

        public ItemsController(IMapper mapper, ItemService context)
        {
            _mapper = mapper;
            _itemService = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems([FromQuery] ItemQuery query)
        {
            var items = await _itemService.GetItems(query);
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(long id)
        {
            var item = await _itemService.GetItem(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(long id, ItemForUpdate item)
        {
            await _itemService.UpdateItem(id, item);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDetail>> PostItem(ItemForCreate itemForCreate)
        {
            var createdItem = await _itemService.CreateItem(itemForCreate);

            return CreatedAtAction("PostItem", new { id = createdItem.ItemId }, createdItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(long id)
        {
            await _itemService.DeleteItem(id);
            return NoContent();
        }
    }
}
