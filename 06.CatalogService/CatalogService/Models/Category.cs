using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogService.Models
{
    public class Category
    {
        [Key]
        public long CategoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = string.Empty;

        //[InverseProperty("Category")]
        public ICollection<Item> Items { get; set; } = new HashSet<Item>();

    }
}
