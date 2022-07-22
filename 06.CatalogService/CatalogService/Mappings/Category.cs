using CatalogService.Api.Web.Models;
using CatalogService.Models;

namespace CatalogService.Mappings
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public ICollection<ItemUpdate> ItemList { get; set; }
    }
}
