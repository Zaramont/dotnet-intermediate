using AutoMapper;
using CatalogService.Data;
using CatalogService.Models.App;
using CatalogService.Models.EF;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Services
{
    public class CategoryService
    {
        private readonly CategoryDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(
            CategoryDbContext context,
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<CategoryDetail> CreateCategory(CategoryForCreate CategoryForCreate)
        {
            if (CategoryForCreate is null)
                throw new ArgumentNullException(nameof(CategoryForCreate));

            return CreateCategory();

            async Task<CategoryDetail> CreateCategory()
            {
                var category = _mapper.Map<Category>(CategoryForCreate);
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return await GetCategory(category.CategoryId);
            }
        }

        public async Task DeleteCategory(long categoryId)
        {
            var categoryFromDb = await _context
                .Categories
                .FirstOrDefaultAsync(category => category.CategoryId == categoryId);

            if (categoryFromDb == null) 
                throw new EntityNotFoundException($"A category having id '{categoryId}' could not be found");

            _context.Categories.Remove(categoryFromDb);
            await _context.SaveChangesAsync();
        }

        public async Task<CategoryDetail> GetCategory(long categoryId)
        {
            var categoryFromDb = await _context
                .Categories
                .FirstOrDefaultAsync(category => category.CategoryId == categoryId);

            return _mapper.Map<CategoryDetail>(categoryFromDb);
        }

        public async Task<CategoryForPatch?> GetCategoryForPatch(long categoryId)
        {
            var categoryFromDb = await _context
                .Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(category => category.CategoryId == categoryId);

            return categoryFromDb == null ? null : _mapper.Map<CategoryForPatch>(categoryFromDb);
        }

        public Task<ICollection<CategoryDetail>> GetCategories(CategoryQuery query)
        {
            if (query is null)
                throw new ArgumentNullException(nameof(query));

            return GetCategories();

            async Task<ICollection<CategoryDetail>> GetCategories()
            {
                var categoriesFromDb = await _context.Categories.ToListAsync();
                var mappedCategories = _mapper.Map<IList<CategoryDetail>>(categoriesFromDb);

                return mappedCategories;
            }
        }

        public Task PatchCategory(long categoryId, CategoryForPatch categoryForPatch)
        {
            if (categoryForPatch is null)
                throw new ArgumentNullException(nameof(categoryForPatch));

            return PatchCategory();

            async Task PatchCategory()
            {
                var categoryFromDb = await _context
                    .Categories
                    .FirstOrDefaultAsync(category => category.CategoryId == categoryId);

                if (categoryFromDb == null)
                    throw new EntityNotFoundException($"A category having id '{categoryId}' could not be found");

                categoryFromDb.Name = categoryForPatch.Name;
                categoryFromDb.Description = categoryForPatch.Description;

                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public Task UpdateCategory(long categoryId, CategoryForUpdate categoryForUpdate)
        {
            if (categoryForUpdate is null)
                throw new ArgumentNullException(nameof(categoryForUpdate));

            return UpdateCategory();

            async Task UpdateCategory()
            {
                var categoryFromDb = await _context
                    .Categories
                    .FirstOrDefaultAsync(category => category.CategoryId == categoryId)
                    .ConfigureAwait(false);

                if (categoryFromDb == null)
                    throw new EntityNotFoundException($"A category having id '{categoryId}' could not be found");

                categoryFromDb.Name = categoryForUpdate.Name;
                categoryFromDb.Description = categoryForUpdate.Description;

                await _context.SaveChangesAsync();
            }
        }
    }
}
