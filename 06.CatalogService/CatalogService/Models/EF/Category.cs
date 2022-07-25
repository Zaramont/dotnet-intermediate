using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogService.Models.EF
{
    public class Category
    {
        [Key]
        public long CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        //[InverseProperty("Category")]
        public ICollection<Item> Items { get; } = new HashSet<Item>();

    }
}
