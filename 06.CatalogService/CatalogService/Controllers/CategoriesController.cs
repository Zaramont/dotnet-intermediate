using AutoMapper;
using CatalogService.Data;
using CatalogService.Models.App;
using CatalogService.Models.EF;
using CatalogService.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Controllers
{
    [Route("v1/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories(CategoryQuery query)
        {
            var categories = await _categoryService.GetCategories(query);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(long id)
        {
            var category = await _categoryService.GetCategory(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }
        
        
        /*[HttpGet("{categoryId:long}/items", Name = nameof(GetCategoryItemsList))]
        public async Task<IActionResult> GetCategoryItemsList([FromRoute] long categoryId)
        {
            var itemsFromDb = await _context.Items.Where(x => x.CategoryId == categoryId).ToListAsync();
            
            return Ok(itemsFromDb);
        }*/

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(long id, CategoryForUpdate category)
        {
            await _categoryService.UpdateCategory(id, category);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(CategoryForCreate category)
        {
            var createdCategory = await _categoryService.CreateCategory(category);

            return CreatedAtAction("PostCategory", new { id = createdCategory.CategoryId }, category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(long id)
        {
            await _categoryService.DeleteCategory(id);
            return NoContent();
        }

        [HttpPatch("{categoryId:long}", Name = nameof(PathCategory))]
        public async Task<IActionResult> PathCategory([FromRoute] long categoryId, [FromBody] JsonPatchDocument<CategoryForPatch> patch)
        {
            var categoryForPatch = await _categoryService.GetCategoryForPatch(categoryId);

            if (categoryForPatch == null)
                return NotFound();

            patch.ApplyTo(categoryForPatch);

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            await _categoryService.PatchCategory(categoryId, categoryForPatch);
            return NoContent();
        }
    }
}
