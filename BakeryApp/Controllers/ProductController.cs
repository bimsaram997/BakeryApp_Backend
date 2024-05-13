using Microsoft.AspNetCore.Mvc;
using Models.Data.RawMaterialData;
using Models.Filters;
using Models.Requests;
using Models.Requests.Update_Requests;
using Models.ViewModels;
using Models.ViewModels.Product;
using Models.ViewModels.FoodType;
using Models.ViewModels.RawMaterial;
using Models.ViewModels.Recipe;
using Repositories;
using Repositories.ProductRepository;
using Repositories.RawMarerialRepository;
using Repositories.RecipeRepository;
using System.Collections;

namespace BakeryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
      
        public IRepositoryBase<ProductVM> _productRepository;
        public IRepositoryBase<FoodTypeVM> _foodTypeRepository;
        public IFoodTypeRepository _iFoodTypeRepository;
        public IRepositoryAllBase<AllProductVM> _productAllBase;
        public IRepositoryBase<RawMaterialVM> _rawMaterialRepository;
        public IRecipeRepository _iRecipeRepository;
        public IRawMaterialRepository _iRawMaterialRepository;
        public IProductRepository _iProductRepository;

        public ProductController( 
            IRepositoryBase<ProductVM> productRepository,
            IRepositoryAllBase<AllProductVM> productAllBase,
            IFoodTypeRepository ifoodTypeRepository,
            IRepositoryBase<FoodTypeVM> foodTypeRepository,
            IRepositoryBase<RawMaterialVM> rawMaterialRepository,
            IRecipeRepository iRecipeRepository,
            IRawMaterialRepository iRawMaterialRepository,
            IProductRepository iFoodItemRepository)
        {
  
           _productRepository = productRepository;
           _productAllBase= productAllBase;
           _iFoodTypeRepository = ifoodTypeRepository;
           _foodTypeRepository= foodTypeRepository;
           _rawMaterialRepository= rawMaterialRepository;
           _iRecipeRepository = iRecipeRepository;
           _iRawMaterialRepository= iRawMaterialRepository;
            _iProductRepository = iFoodItemRepository;
        }

        //Add food item
        [HttpPost("addProduct")]
        public IActionResult AddProduct([FromBody] AddProductRequest productRequest)
        {
            try
            {
                FoodTypeVM foodType = _foodTypeRepository.GetById(productRequest.FoodTypeId);
                RecipeVM recipeVM= _iRecipeRepository.GetByFoodTypeId(productRequest.FoodTypeId);
                
                if (foodType != null && productRequest.ProductCount > 0 && recipeVM != null) {
                    long batchId = GenerateBatchId();
                    var product = new ProductVM
                    {
                        ProductDescription = productRequest.ProductDescription,
                        ProductPrice = productRequest.ProductPrice,
                        ImageURL = productRequest.ImageURL,
                        AddedDate = DateTime.Now,
                        FoodTypeId = productRequest.FoodTypeId,
                        BatchId = batchId,
                    };

                    int[] rawMaterialIds = recipeVM.RawMaterials.Select(rrm => rrm.rawMaterialId).ToArray();
                    
                    // Generate a batchId for the current batch
                    
                    
                    // Initialize a dictionary to store the total quantity used for each raw material
                    Dictionary<int, double> rawMaterialQuantitiesUsed = new Dictionary<int, double>();
                    
                    List<int> foodIds = new List<int>();
                   
                    for (int i = 0; i < productRequest.ProductCount; ++i)
                    {

                        // calcualte raw merial count and update the reamaing qunatity
                        foreach (int id in rawMaterialIds)
                        {
                            RawMaterialVM rawMaterialVM = _rawMaterialRepository.GetById(id);
                            RawMatRecipeVM rawMatRecipeVM = _iRawMaterialRepository.GetRawMaterialRecipeByRawMatIdAndRecipeId(rawMaterialVM.Id, recipeVM.Id);
                            double quantityUsed = 0;

                            switch (rawMaterialVM.MeasureUnit)
                            {
                                // Reduce raw material count from current stock
                                case MeasureUnit.Kg:
                                    quantityUsed = rawMatRecipeVM.rawMaterialQuantity;
                                    UpdateQuantity(rawMaterialVM.Id, rawMaterialVM.Quantity, rawMatRecipeVM.rawMaterialQuantity);
                                    break;
                                case MeasureUnit.L:
                                    quantityUsed = rawMatRecipeVM.rawMaterialQuantity;
                                    UpdateQuantity(rawMaterialVM.Id, rawMaterialVM.Quantity, rawMatRecipeVM.rawMaterialQuantity);
                                    break;
                            }

                            // Add the quantity used to the dictionary
                            rawMaterialQuantitiesUsed[id] = quantityUsed;
                        }

                       
                        // add food item
                        int productId = _productRepository.Add(product);
                        foodIds.Add(productId);

                        // Associate the current productId with the generated batchId
                        AssociateFoodWithBatch(productId, batchId);
                        StoreRawMaterialQuantitiesUsed(productId, rawMaterialQuantitiesUsed);
                        

                    }
                    return Created(nameof(AddProduct), foodIds);
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
        public IActionResult GetAllProducts( [FromBody] ProductListAdvanceFilter productListAdvanceFilter)
        {
            try
            {
                var _products = _iProductRepository.GetAll(productListAdvanceFilter);
                return Created(nameof(GetAllProducts), _products);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding Food item: {ex.Message}");
            }

        }

        [HttpPost("updateProductsByBatchId/{batchId}")]
        public IActionResult UpdateProductsByBatchId(long batchId, [FromBody] UpdateProduct updateItem)
        {
            try
            {
                int updatedFoodItemId =  _iProductRepository.UpdateProductsByBatchId(batchId, updateItem);
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


        private void AssociateFoodWithBatch(int productId, long batchId)
        {
            if (productId > 0 && batchId > 0)
            {
                _iProductRepository.AddBatchDetails(productId, batchId);
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

        private void StoreRawMaterialQuantitiesUsed(int productId, Dictionary<int, double> rawMaterialQuantitiesUsed)
        {
           try
            {
                if (productId > 0 && rawMaterialQuantitiesUsed.Count > 0)
                {
                    _iRawMaterialRepository.StoreRawMaterialQuantitiesUsed(productId, rawMaterialQuantitiesUsed);
                }
            } catch(Exception ex)
            {

            }
        }






        /* [HttpGet("findById/{id}")]
         public IActionResult GetFoodItemById(int foodItemId)
         {
             var _products = _foodItemService.GetFoodItemById(foodItemId);
             return Ok(_products);
         }*/


        [HttpGet("getProudctById/{id}")]
        public IActionResult GetFoodTypeById(int id)
        {
            try
            {
                // Call the repository to get the recipe by ID
                ProductVM _product = _productRepository.GetById(id);

                if (_product != null)
                {
                    return Created(nameof(GetFoodTypeById), _product);
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
