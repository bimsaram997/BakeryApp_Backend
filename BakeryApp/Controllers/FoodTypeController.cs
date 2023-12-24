using Microsoft.AspNetCore.Mvc;
using Models.Requests;
using Models.ViewModels.FoodItem;
using Models.ViewModels;
using Repositories.FoodItemRepository;
using Repositories;
using Models.ViewModels.FoodType;

namespace BakeryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodTypeController : ControllerBase
    {

        public IRepositoryBase<FoodTypeVM> _foodTypeRepository;
        public IFoodItemRepository _ifoodItemRepository;
        public IRepositoryAllBase<AllFoodItemVM> _foodItemAllBase;
        public FoodTypeController(IRepositoryBase<FoodTypeVM> foodTypeRepository,
            IRepositoryAllBase<AllFoodItemVM> foodItemAllBase, IFoodItemRepository ifoodTypeRepository)
        {

            _foodTypeRepository = foodTypeRepository;
            _foodItemAllBase = foodItemAllBase;
            _ifoodItemRepository = ifoodTypeRepository;
           
            
        }

        //Add food item
        [HttpPost("addFoodType")]
        public IActionResult AddFoodType([FromBody] AddFoodTypeRequest foodTypeRequest)
        {

            var foodItem = new FoodTypeVM
            {
                FoodTypeCount = foodTypeRequest.FoodTypeCount,
                FoodTypeName = foodTypeRequest.FoodTypeName,
                ImageURL = foodTypeRequest.ImageURL,
                AddedDate = DateTime.Now,

            };

            int foodId = _foodTypeRepository.Add(foodItem);
          
            return Ok();
        }


        [HttpGet("listAdvance")]
        public IActionResult GetAllFoodItems()
        {
            var _foodItems = _foodItemAllBase.GetAll();
            return Ok(_foodItems);
        }

         [HttpGet("findByFoodTypeId/{id}")]
         public IActionResult GetFoodItemById(int id)
         {
             //var _foodTypeRawMaterials = _iFoodTypeRawMaterialRepository.GetByFoodTypeId(id);
             return Ok(1);
         }
    }
}
