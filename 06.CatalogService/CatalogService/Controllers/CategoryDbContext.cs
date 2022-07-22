using CatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Controllers
{
    public class CategoryDbContext : DbContext
    {
        public CategoryDbContext(DbContextOptions<CategoryDbContext> options)
            : base(options) {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //base.OnModelCreating(builder);
            builder.Entity<Category>();
            builder.Entity<Item>().HasOne(p => p.Category).WithMany(p => p.Items);
        }
        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Item> Items { get; set; } = null!;
    }
}
