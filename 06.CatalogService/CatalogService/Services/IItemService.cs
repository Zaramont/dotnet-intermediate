using CatalogService.Models.App;

namespace CatalogService.Services
{
    public interface IItemService
    {
        Task<ItemDetail> CreateItem(ItemForCreate ItemForCreate);
        Task DeleteItem(long itemId);
        Task<ItemDetail> GetItem(long itemId);
        Task<ICollection<ItemDetail>> GetItems(ItemQuery query);
        Task UpdateItem(long itemId, ItemForUpdate itemForUpdate);
    }
}