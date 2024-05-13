using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ProductRepository
{
    public class AllRecipeRepository: IRepositoryAllBase<AllProductVM>
    {
        private AppDbContext _context;
        public AllRecipeRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<AllProductVM> GetAll()
        {
            var allFoodItems = _context.Product.Include(fi => fi.foodType).ToList();
            var productVMs = allFoodItems.Select(product => new AllProductVM
            {
                Id = product.Id,
                ProductCode = product.ProductCode,
                ProductDescription = product.ProductDescription,
                ProductPrice = product.ProductPrice,
                AddedDate = product.AddedDate,
                ImageURL = product.ImageURL,
                FoodTypeId = product.FoodTypeId,
                FoodTypeName = product.foodType.FoodTypeName
            });

            return productVMs;
        }
    }
}
