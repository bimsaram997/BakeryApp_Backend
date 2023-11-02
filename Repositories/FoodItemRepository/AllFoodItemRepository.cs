using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Data.FoodItemData;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.FoodItemRepository
{
    public class AllFoodItemRepository: IRepositoryAllBase<AllFoodItemVM>
    {
        private AppDbContext _context;
        public AllFoodItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<AllFoodItemVM> GetAll()
        {
            var allFoodItems = _context.FoodItems.Include(fi => fi.foodType).ToList();
            var foodItemVMs = allFoodItems.Select(foodItem => new AllFoodItemVM
            {
                Id = foodItem.Id,
                FoodCode = foodItem.FoodCode,
                FoodDescription = foodItem.FoodDescription,
                FoodPrice = foodItem.FoodPrice,
                AddedDate = foodItem.AddedDate,
                ImageURL = foodItem.ImageURL,
                FoodTypeId = foodItem.FoodTypeId,
                FoodTypeName = foodItem.foodType.FoodTypeName
            });

            return foodItemVMs;
        }
    }
}
