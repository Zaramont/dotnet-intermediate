using CatalogService.Models.EF;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogService.Models.App
{
    public class ItemDetail
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }

        public long CategoryId { get; set; }
        public Category Category { get; set; } = new Category();
    }
}