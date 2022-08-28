using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogService.Models.App
{
    public class ItemForCreate
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }

        public long CategoryId { get; set; }
    }
}