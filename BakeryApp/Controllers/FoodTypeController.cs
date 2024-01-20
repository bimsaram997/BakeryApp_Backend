using Microsoft.AspNetCore.Mvc;
using Models.Requests;
using Models.ViewModels.FoodItem;
using Models.ViewModels;
using Repositories.FoodItemRepository;
using Repositories;
using Models.ViewModels.FoodType;
using Repositories.RecipeRepository;
using Models.ViewModels.Recipe;
using Models.Requests.Update_Requests;
using Models.ViewModels.RawMaterial;
using Repositories.RawMarerialRepository;

namespace BakeryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodTypeController : ControllerBase
    {

        public IRepositoryBase<FoodTypeVM> _foodTypeRepository;
        public IFoodItemRepository _ifoodItemRepository;
        public IRepositoryAllBase<AllFoodItemVM> _foodItemAllBase;
        public IRecipeRepository _iRecipeRepository;
        public IFoodTypeRepository _iFoodTypeRepository;
        public FoodTypeController(IRepositoryBase<FoodTypeVM> foodTypeRepository,
            IRepositoryAllBase<AllFoodItemVM> foodItemAllBase,
            IFoodItemRepository ifoodTypeRepository,
            IRecipeRepository iRecipeRepository,
            IFoodTypeRepository iFoodTypeRepository)
        {

            _foodTypeRepository = foodTypeRepository;
            _foodItemAllBase = foodItemAllBase;
            _ifoodItemRepository = ifoodTypeRepository;
            _iRecipeRepository = iRecipeRepository;
            _iFoodTypeRepository = iFoodTypeRepository;



        }

        //Add food item
        [HttpPost("addFoodType")]
        public IActionResult AddFoodType([FromBody] AddFoodTypeRequest foodTypeRequest)
        {

            var foodItem = new FoodTypeVM
            {
                FoodTypeName = foodTypeRequest.FoodTypeName,
                ImageURL = foodTypeRequest.ImageURL,
                AddedDate = DateTime.Now,

            };

            int foodId = _foodTypeRepository.Add(foodItem);
          
            return Ok();
        }


         [HttpGet("getFoodTypeId/{id}")]
         public IActionResult GetFoodTypeById(int id)
         {
            try
            {
                // Call the repository to get the recipe by ID
                FoodTypeVM foodType = _foodTypeRepository.GetById(id);

                if (foodType != null)
                {
                    return Created(nameof(GetFoodTypeById), foodType);
                }
                else
                {
                    // Handle the case where the recipe is not found
                    return NotFound($"Food type with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving Food type: {ex.Message}");
            }
        }

        [HttpDelete("deleteRecipe/{foodtypeId}")]
        public IActionResult DeleteFoodType(int foodtypeId)
        {
            try
            {
                // Call the repository to delete the recipe by ID
                bool isLinkedFoodItem = _ifoodItemRepository.IsFoodTypeLinked(foodtypeId);
                bool isLinkedRecipe = _iRecipeRepository.IsFoodTypeLinked(foodtypeId);

                if (!isLinkedFoodItem && !isLinkedRecipe)
                {
                    int deletedFoodTypeId = _foodTypeRepository.DeleteById(foodtypeId);
                    if (deletedFoodTypeId != -1) {
                        return Created(nameof(DeleteFoodType), deletedFoodTypeId);
                    }
                    else
                    {
                        return NotFound($"Food Type with ID {foodtypeId} not found.");
                    }

                }
                else
                {
                    // Handle the case where the recipe is not found
                    return NotFound($"Food type with ID {foodtypeId} can not delete. It has dependancies.");
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions if needed
                return BadRequest($"Error deleting Food type: {ex.Message}");
            }

           
        }

        [HttpPut("updateFoodType/{foodTypeId}")]
        public IActionResult UpdateFoodType(int foodTypeId, [FromBody] UpdateFoodType updateFoodType)
        {

            try
            {
                FoodTypeVM foodTypeVM = new FoodTypeVM
                {
                   FoodTypeName = updateFoodType.foodTypeName,
                   ImageURL =  updateFoodType.imageURL
                };
                int updatedFoodTypeId = _foodTypeRepository.UpdateById(foodTypeId, foodTypeVM); 
                if (updatedFoodTypeId != -1)
                {
                    // Return a successful response
                    return Created(nameof(UpdateFoodType), updatedFoodTypeId);
                }
                else
                {
                    // Handle the case where the recipe is not found
                    return NotFound($"Food Type with ID {foodTypeId} not found.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating Food Type : {ex.Message}");
            }

        }
        [HttpGet("listAdvance")]
        public IActionResult GetAllFoodItems()
        {
            var _foodItems = _foodItemAllBase.GetAll();
            return Ok(_foodItems);
        }

        [HttpGet("listSimpleFoodTypes")]
        public IActionResult ListSimpleFoodTypes()
        {
            try
            {
                // Call the repository to get the list of simple FoodTypes
                FoodTypeVM[] foodTypes = _iFoodTypeRepository.ListSimpeleFoodTypes();

                return Ok(foodTypes);
            }
            catch (Exception ex)
            {
                // Handle other exceptions if needed
                return BadRequest($"Error getting list of simple FoodTypes: {ex.Message}");
            }
        }

    }
}
