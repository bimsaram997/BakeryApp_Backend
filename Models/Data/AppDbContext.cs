using Microsoft.EntityFrameworkCore;

namespace Models.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<FoodType> FoodTypes { get; set; }


    }
}
