using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogService.Models.EF
{
    public class Item
    {
        public long ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }

        public long CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}