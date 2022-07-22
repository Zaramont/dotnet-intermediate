using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogService.Models
{
    public class ItemUpdate
    {
        public long ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;        
        public double Price { get; set; }

        public long CategoryId { get; set; }
    }
}