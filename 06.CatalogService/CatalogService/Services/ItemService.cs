using AutoMapper;
using CatalogService.Data;
using CatalogService.Models.App;
using CatalogService.Models.EF;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Services
{
    public class ItemService
    {
        private readonly CategoryDbContext _context;
        private readonly IMapper _mapper;

        public ItemService(
            CategoryDbContext context,
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<ItemDetail> CreateItem(ItemForCreate ItemForCreate)
        {
            if (ItemForCreate is null)
                throw new ArgumentNullException(nameof(ItemForCreate));

            return CreateItem();

            async Task<ItemDetail> CreateItem()
            {
                var item = _mapper.Map<Item>(ItemForCreate);
                _context.Items.Add(item);
                await _context.SaveChangesAsync();
                return await GetItem(item.ItemId);
            }
        }

        public async Task DeleteItem(long itemId)
        {
            var itemFromDb = await _context
                .Items
                .FirstOrDefaultAsync(item => item.ItemId == itemId);

            if (itemFromDb == null)
                throw new EntityNotFoundException($"A item having id '{itemId}' could not be found");

            _context.Items.Remove(itemFromDb);
            await _context.SaveChangesAsync();
        }

        public async Task<ItemDetail> GetItem(long itemId)
        {
            var itemFromDb = await _context
                .Items
                .FirstOrDefaultAsync(item => item.ItemId == itemId);

            return _mapper.Map<ItemDetail>(itemFromDb);
        }

        public Task<ICollection<ItemDetail>> GetItems(ItemQuery query)
        {
            if (query is null)
                throw new ArgumentNullException(nameof(query));

            return GetItems();

            async Task<ICollection<ItemDetail>> GetItems()
            {
                var categoriesFromDb = await PagedList<Item>.ToPagedListAsync(_context.Items, query.PageNumber, query.PageSize);

                var mappedItems = _mapper.Map<PagedList<ItemDetail>>(categoriesFromDb);

                return mappedItems;
            }
        }

        public Task UpdateItem(long itemId, ItemForUpdate itemForUpdate)
        {
            if (itemForUpdate is null)
                throw new ArgumentNullException(nameof(itemForUpdate));

            return UpdateItem();

            async Task UpdateItem()
            {
                var itemFromDb = await _context
                    .Items
                    .FirstOrDefaultAsync(item => item.ItemId == itemId);

                if (itemFromDb == null)
                    throw new EntityNotFoundException($"A item having id '{itemId}' could not be found");

                itemFromDb.Name = itemForUpdate.Name;
                itemFromDb.Description = itemForUpdate.Description;

                await _context.SaveChangesAsync();
            }
        }
    }
}
