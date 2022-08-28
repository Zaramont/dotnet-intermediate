using CatalogService.Models.App;

namespace CatalogService.Services
{
    public interface ICategoryService
    {
        Task<CategoryDetail> CreateCategory(CategoryForCreate CategoryForCreate);
        Task DeleteCategory(long categoryId);
        Task<ICollection<CategoryDetail>> GetCategories(CategoryQuery query);
        Task<CategoryDetail> GetCategory(long categoryId);
        Task UpdateCategory(long categoryId, CategoryForUpdate categoryForUpdate);
    }
}