using Microsoft.AspNetCore.Mvc;
using Models.Requests;
using Models.ViewModels;
using Models.ViewModels.FoodItem;
using Models.ViewModels.FoodType;
using Repositories;
using Repositories.FoodItemRepository;

namespace BakeryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemController : ControllerBase
    {
      
        public IRepositoryBase<FoodItemVM> _foodRepository;
        public IRepositoryBase<FoodTypeVM> _foodTypeRepository;
        public IFoodTypeRepository _iFoodTypeRepository;
        public IRepositoryAllBase<AllFoodItemVM> _foodItemAllBase;
      
        public FoodItemController( 
            IRepositoryBase<FoodItemVM> foodRepository,
            IRepositoryAllBase<AllFoodItemVM> foodItemAllBase,
            IFoodTypeRepository ifoodTypeRepository,
            IRepositoryBase<FoodTypeVM> foodTypeRepository)
        {
  
           _foodRepository = foodRepository;
           _foodItemAllBase= foodItemAllBase;
           _iFoodTypeRepository = ifoodTypeRepository;
           _foodTypeRepository= foodTypeRepository;

        }

        //Add food item
        [HttpPost("addFood")]
        public IActionResult AddFoodItem([FromBody] AddFoodItemRequest foodItemRequest)
        {
            
            var foodItem = new FoodItemVM
            {
                FoodDescription = foodItemRequest.FoodDescription,
                FoodPrice = foodItemRequest.FoodPrice,
                ImageURL = foodItemRequest.ImageURL,
                AddedDate = DateTime.Now,
                FoodTypeId=foodItemRequest.FoodTypeId,

            };

            int foodId = _foodRepository.Add(foodItem);
            FoodTypeVM foodTypeVM =  _foodTypeRepository.GetById(foodItemRequest.FoodTypeId);
            if (foodTypeVM != null ) {
                _iFoodTypeRepository.UpdateFoodTypeCountByFoodTypeId(foodTypeVM.Id);

            }

            return Ok();
        }


        [HttpGet("listAdvance")]
        public IActionResult GetAllFoodItems()
        {
            var _foodItems = _foodItemAllBase.GetAll();
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
