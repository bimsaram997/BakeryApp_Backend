using Microsoft.EntityFrameworkCore;
using Models.Data.Address;
using Models.Data.EnumType;
using Models.Data.ProductData;
using Models.Data.RawMaterialData;
using Models.Data.RecipeData;
using Models.Data.ReferenceData;
using Models.Data.Role;
using Models.Data.Stock;
using Models.Data.Supplier;
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
        public DbSet<Addresses> Address { get; set; }
        public DbSet<EnumTypeTranslationMap> EnumTypeTranslationMap { get; set; }
        public DbSet<MasterData> MasterData { get; set; }
        public DbSet<Roles> Role { get; set; }
        public DbSet<Suppliers> Supplier { get; set; }
        public DbSet<SupplierProduct> SupplierProduct { get; set; }
        public DbSet<SupplierRawMaterial> SupplierRawMaterial { get; set; }
        public DbSet<Stocks> Stock { get; set; }
        public DbSet<StockRawMaterialHistory> StockRawMaterialHistory { get; set; }
        public DbSet<StockItem> StockItem { get; set; }
    }
}
