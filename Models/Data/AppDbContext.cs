using Microsoft.EntityFrameworkCore;
using Models.Data.FoodItemData;
using Models.Data.RawMaterialData;
using Models.Data.RecipeData;
using Models.Migrations;

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
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<FoodType> FoodTypes { get; set; }
        public DbSet<RawMaterial> RawMaterials { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RawMaterialRecipe> RawMaterialRecipe { get; set; }

    }
}
