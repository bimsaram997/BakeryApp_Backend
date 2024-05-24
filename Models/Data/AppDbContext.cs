using Microsoft.EntityFrameworkCore;
using Models.Data.ProductData;
using Models.Data.RawMaterialData;
using Models.Data.RecipeData;
using Models.Data.User;

namespace Models.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public AppDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
        public DbSet<Product> Product { get; set; }
        public DbSet<RawMaterial> RawMaterials { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RawMaterialRecipe> RawMaterialRecipe { get; set; }
        public DbSet<RawMaterialUsage> rawMaterialUsage { get; set; }
        public DbSet<Users> Users { get; set; }

    }
}
