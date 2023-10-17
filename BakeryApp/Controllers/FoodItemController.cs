using Microsoft.AspNetCore.Mvc;
using Models.Data;
using Models.Requests;
using Repositories;

namespace BakeryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemController : ControllerBase
    {
      
        public IRepositoryBase<FoodItemVM> _foodItemRepository;
        public FoodItemController( IRepositoryBase<FoodItemVM> foodItemRepository)
        {
  
           _foodItemRepository = foodItemRepository;
        }

        //Add food item
        [HttpPost("addFood")]
        public IActionResult AddFoodItem([FromBody] AddFoodItemRequest foodItemRequest)
        {
            
            var foodItem = new FoodItemVM
            {
                FoodCode = foodItemRequest.FoodCode,
                FoodName = foodItemRequest.FoodName,
                FoodDescription = foodItemRequest.FoodDescription,
                FoodPrice = foodItemRequest.FoodPrice,
                ImageURL = foodItemRequest.ImageURL,
                AddedDate= foodItemRequest.AddedDate,

            };

            int foodId = _foodItemRepository.Add(foodItem);
            return Ok();
        }


        [HttpGet("listAdvance")]
        public IActionResult GetAllFoodItems()
        {
            var _foodItems = _foodItemRepository.GetAll();
            return Ok(_foodItems);
        }

       /* [HttpGet("findById/{id}")]
        public IActionResult GetFoodItemById(int foodItemId)
        {
            var _foodItems = _foodItemService.GetFoodItemById(foodItemId);
            return Ok(_foodItems);
        }*/
    }
}
