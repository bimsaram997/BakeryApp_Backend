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
        public IRepositoryBase<RawMaterialVM> _rawMaterialRepository;
        public IRecipeRepository _iRecipeRepository;
        public IRawMaterialRepository _iRawMaterialRepository;
        public IProductRepository _iProductRepository;

        public ProductController( 
            IRepositoryBase<ProductVM> productRepository,
            IRepositoryBase<RawMaterialVM> rawMaterialRepository,
            IRecipeRepository iRecipeRepository,
            IRawMaterialRepository iRawMaterialRepository,
            IProductRepository iFoodItemRepository
            )
        {
  
           _productRepository = productRepository;
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
                    var product = new ProductVM
                    {
                        Name =  productRequest.Name,
                        Unit =  productRequest.Unit,
                        CostCode =  productRequest.CostCode,
                        CostPrice =  productRequest.CostPrice,
                        SellingPrice= productRequest.SellingPrice,
                        RecipeId =  productRequest.RecipeId,
                        ProductDescription = productRequest.ProductDescription,
                        ImageURL = productRequest.ImageURL,
                        AddedDate = productRequest.AddedDate,
                        
                    };

                   int productId = _productRepository.Add(product);
                    return Created(nameof(AddProduct), productId);
                
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding Food item: {ex.Message}");
            }
        }


        [HttpPost("listAdvance")]
        public IActionResult GetAllProducts([FromBody] ProductListAdvanceFilter productListAdvanceFilter)
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

        [HttpPost("updateProductsById/{productId}")]
        public IActionResult UpdateProductsById(int productId, [FromBody] UpdateProduct updateItem)
        {
            try
            {
                ProductVM product = new ProductVM
                {
                    Name = updateItem.Name,
                    Unit = updateItem.Unit,
                    CostCode = updateItem.CostCode,
                    CostPrice = updateItem.CostPrice,
                    SellingPrice = updateItem.SellingPrice,
                    RecipeId= updateItem.RecipeId,
                    ProductDescription = updateItem.ProductDescription,
                    ImageURL = updateItem.ImageURL

                };
                int updatedFoodItemId = _productRepository.UpdateById(productId, product);
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


      /*  private void AssociateFoodWithBatch(int productId, long batchId)
        {
            if (productId > 0 && batchId > 0)
            {
                _iProductRepository.AddBatchDetails(productId, batchId);
            }
        }*/

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
