using CatalogService.Models.EF;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Data
{
    public class CategoryDbContext : DbContext
    {
        public CategoryDbContext(DbContextOptions<CategoryDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            base.OnModelCreating(builder);
        }
        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Item> Items { get; set; } = null!;
    }
}
