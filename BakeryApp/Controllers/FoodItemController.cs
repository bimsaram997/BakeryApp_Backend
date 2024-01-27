using Microsoft.AspNetCore.Mvc;
using Models.Data.FoodItemData;
using Models.Data.RawMaterialData;
using Models.Filters;
using Models.Requests;
using Models.Requests.Update_Requests;
using Models.ViewModels;
using Models.ViewModels.FoodItem;
using Models.ViewModels.FoodType;
using Models.ViewModels.RawMaterial;
using Models.ViewModels.Recipe;
using Repositories;
using Repositories.FoodItemRepository;
using Repositories.RawMarerialRepository;
using Repositories.RecipeRepository;
using System.Collections;

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
        public IRepositoryBase<RawMaterialVM> _rawMaterialRepository;
        public IRecipeRepository _iRecipeRepository;
        public IRawMaterialRepository _iRawMaterialRepository;
        public IFoodItemRepository _iFoodItemRepository;

        public FoodItemController( 
            IRepositoryBase<FoodItemVM> foodRepository,
            IRepositoryAllBase<AllFoodItemVM> foodItemAllBase,
            IFoodTypeRepository ifoodTypeRepository,
            IRepositoryBase<FoodTypeVM> foodTypeRepository,
            IRepositoryBase<RawMaterialVM> rawMaterialRepository,
            IRecipeRepository iRecipeRepository,
            IRawMaterialRepository iRawMaterialRepository,
            IFoodItemRepository iFoodItemRepository)
        {
  
           _foodRepository = foodRepository;
           _foodItemAllBase= foodItemAllBase;
           _iFoodTypeRepository = ifoodTypeRepository;
           _foodTypeRepository= foodTypeRepository;
           _rawMaterialRepository= rawMaterialRepository;
           _iRecipeRepository = iRecipeRepository;
           _iRawMaterialRepository= iRawMaterialRepository;
            _iFoodItemRepository = iFoodItemRepository;
        }

        //Add food item
        [HttpPost("addFood")]
        public IActionResult AddFoodItem([FromBody] AddFoodItemRequest foodItemRequest, int foodItemCount)
        {
            try
            {
                FoodTypeVM foodType = _foodTypeRepository.GetById(foodItemRequest.FoodTypeId);
                RecipeVM recipeVM= _iRecipeRepository.GetByFoodTypeId(foodItemRequest.FoodTypeId);
                
                if (foodType != null && foodItemCount > 0 && recipeVM != null) {

                    var foodItem = new FoodItemVM
                    {
                        FoodDescription = foodItemRequest.FoodDescription,
                        FoodPrice = foodItemRequest.FoodPrice,
                        ImageURL = foodItemRequest.ImageURL,
                        AddedDate = DateTime.Now,
                        FoodTypeId = foodItemRequest.FoodTypeId,
                    };

                    int[] rawMaterialIds = recipeVM.rawMaterials.Select(rrm => rrm.rawMaterialId).ToArray();
                    
                    // Generate a batchId for the current batch
                    long batchId = GenerateBatchId();
                    
                    // Initialize a dictionary to store the total quantity used for each raw material
                    Dictionary<int, double> rawMaterialQuantitiesUsed = new Dictionary<int, double>();
                    
                    List<int> foodIds = new List<int>();
                   
                    for (int i = 0; i < foodItemCount; ++i)
                    {

                        // calcualte raw merial count and update the reamaing qunatity
                        foreach (int id in rawMaterialIds)
                        {
                            RawMaterialVM rawMaterialVM = _rawMaterialRepository.GetById(id);
                            RawMatRecipeVM rawMatRecipeVM = _iRawMaterialRepository.GetRawMaterialRecipeByRawMatIdAndRecipeId(rawMaterialVM.id, recipeVM.id);
                            double quantityUsed = 0;

                            switch (rawMaterialVM.rawMaterialQuantityType)
                            {
                                // Reduce raw material count from current stock
                                case RawMaterialQuantityType.Kg:
                                    quantityUsed = rawMatRecipeVM.rawMaterialQuantity;
                                    UpdateQuantity(rawMaterialVM.id, rawMaterialVM.quantity, rawMatRecipeVM.rawMaterialQuantity);
                                    break;
                                case RawMaterialQuantityType.L:
                                    quantityUsed = rawMatRecipeVM.rawMaterialQuantity;
                                    UpdateQuantity(rawMaterialVM.id, rawMaterialVM.quantity, rawMatRecipeVM.rawMaterialQuantity);
                                    break;
                            }

                            // Add the quantity used to the dictionary
                            rawMaterialQuantitiesUsed[id] = quantityUsed;
                        }

                       
                        // add food item
                        int foodId = _foodRepository.Add(foodItem);
                        foodIds.Add(foodId);

                        // Associate the current foodId with the generated batchId
                        AssociateFoodWithBatch(foodId, batchId);
                        StoreRawMaterialQuantitiesUsed(foodId, rawMaterialQuantitiesUsed);
                        

                    }
                    return Created(nameof(AddFoodItem), foodIds);
                } else
                {
                    throw new Exception("Food type or Recipe is not available");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding Food item: {ex.Message}");
            }
        }


        [HttpPost("listAdvance")]
        public IActionResult GetAllFoodItems( [FromBody] ProductListAdvanceFilter productListAdvanceFilter)
        {
            try
            {
                var _foodItems = _iFoodItemRepository.GetAll(productListAdvanceFilter);
                return Created(nameof(GetAllFoodItems), _foodItems);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding Food item: {ex.Message}");
            }

        }

        [HttpPost("updateItemsByBatchId/{batchId}")]
        public IActionResult UpdateItemsByBatchId(long batchId, [FromBody] UpdateFoodItem updateItem)
        {
            try
            {
                int updatedFoodItemId =  _iFoodItemRepository.UpdateItemsByBatchId(batchId, updateItem);
                return Ok(updatedFoodItemId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private long GenerateBatchId()
        {
            var timestamp = DateTime.UtcNow;
            var random = new Random();
            var randomPart = random.Next(1, 100);

            // Combine timestamp and random number to create a unique batch ID
            var batchIdString = $"{timestamp:yyyyMMdd}{randomPart}";

            // Convert the string to a long
            if (long.TryParse(batchIdString, out var batchId))
            {
                return batchId;
            }

            throw new InvalidOperationException("Failed to generate a valid batch ID.");
        }


        private void AssociateFoodWithBatch(int foodId, long batchId)
        {
            if (foodId > 0 && batchId > 0)
            {
                _iFoodItemRepository.AddBatchDetails(foodId, batchId);
            }
        }

        private void UpdateQuantity(int rawMatId, double currentQuantity, double quantityRecipe)
        {
            try
            {
                if (currentQuantity <= quantityRecipe)
                {
                    throw new Exception("Error updating raw material quantity: Insufficient quantity.");
                }
                double newQuantity = currentQuantity - quantityRecipe;
                int updatedRawMatId = _iRawMaterialRepository.UpdateRawMaterialCountbyRawMatId(rawMatId, newQuantity);
                if (updatedRawMatId == -1)
                {
                    throw new Exception($"Raw material with ID {rawMatId} not found.");
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception here if needed
                Console.WriteLine($"Error updating: {ex.Message}");
                // Rethrow the exception
                throw;
            }
        }

        private void StoreRawMaterialQuantitiesUsed(int foodId, Dictionary<int, double> rawMaterialQuantitiesUsed)
        {
           try
            {
                if (foodId > 0 && rawMaterialQuantitiesUsed.Count > 0)
                {
                    _iRawMaterialRepository.StoreRawMaterialQuantitiesUsed(foodId, rawMaterialQuantitiesUsed);
                }
            } catch(Exception ex)
            {

            }
        }






        /* [HttpGet("findById/{id}")]
         public IActionResult GetFoodItemById(int foodItemId)
         {
             var _foodItems = _foodItemService.GetFoodItemById(foodItemId);
             return Ok(_foodItems);
         }*/


        [HttpGet("getFoodItemId/{id}")]
        public IActionResult GetFoodTypeById(int id)
        {
            try
            {
                // Call the repository to get the recipe by ID
                FoodItemVM _foodItem = _foodRepository.GetById(id);

                if (_foodItem != null)
                {
                    return Created(nameof(GetFoodTypeById), _foodItem);
                }
                else
                {
                    // Handle the case where the recipe is not found
                    return NotFound($"Food item with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving Food item: {ex.Message}");
            }
        }
    }
}
