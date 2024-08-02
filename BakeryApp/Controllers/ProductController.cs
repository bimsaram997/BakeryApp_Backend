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
using Models.ActionResults;
using Models.Data.RecipeData;
using Models.ViewModels.Custom_action_result;

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
        public CustomActionResult<AddResultVM> AddProduct([FromBody] AddProductRequest productRequest)
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
                        Weight = productRequest.Weight,
                        Status= productRequest.Status,
                        DaysToExpires= productRequest.DaysToExpires,
                        ReOrderLevel = productRequest.ReOrderLevel
                    };

                   int productId = _productRepository.Add(product);
                if (productId > 0)
                {
                    var result = new AddResultVM
                    {
                        Id = productId
                    };
                    var responseObj = new CustomActionResultVM<AddResultVM>
                    {
                        Data = result
                    };
                    return new CustomActionResult<AddResultVM>(responseObj);
                }
                else
                {
                    return new CustomActionResult<AddResultVM>(new CustomActionResultVM<AddResultVM>
                    {
                        Exception = new Exception("Product can't add!")
                    });
                }

            }
            catch (Exception ex)
            {
                return new CustomActionResult<AddResultVM>(new CustomActionResultVM<AddResultVM>
                {
                    Exception = ex
                });
            }
        }


        [HttpPost("listAdvance")]
        public CustomActionResult<ResultView<PaginatedProducts>> GetAllProducts([FromBody] ProductListAdvanceFilter productListAdvanceFilter)
        {
            try
            {
                var _products = _iProductRepository.GetAll(productListAdvanceFilter);
                var result = new ResultView<PaginatedProducts>
                {
                    Item = _products

                };
                var responseObj = new CustomActionResultVM<ResultView<PaginatedProducts>>
                {
                    Data = result

                };
                return new CustomActionResult<ResultView<PaginatedProducts>>(responseObj);
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<PaginatedProducts>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<PaginatedProducts>>(responseObj);
            }

        }

        [HttpPost("updateProductsById/{productId}")]
        public CustomActionResult<AddResultVM> UpdateProductsById(int productId, [FromBody] UpdateProduct updateItem)
        {
            try
            {
                ProductVM productVM = _productRepository.GetById(productId);
                if(productVM != null)
                {
                    ProductVM product = new ProductVM
                    {
                        Name = updateItem.Name,
                        Unit = updateItem.Unit,
                        CostCode = updateItem.CostCode,
                        CostPrice = updateItem.CostPrice,
                        SellingPrice = updateItem.SellingPrice,
                        RecipeId = updateItem.RecipeId,
                        ProductDescription = updateItem.ProductDescription,
                        ImageURL = updateItem.ImageURL,
                        Weight = updateItem.Weight,
                        Status = updateItem.Status,
                        DaysToExpires = updateItem.DaysToExpires,
                        ReOrderLevel = updateItem.ReOrderLevel

                    };
                    int updatedFoodItemId = _productRepository.UpdateById(productId, product);
                    var result = new AddResultVM
                    {
                        Id = updatedFoodItemId
                    };
                    var responseObj = new CustomActionResultVM<AddResultVM>
                    {
                        Data = result
                    };
                    return new CustomActionResult<AddResultVM>(responseObj);
                } else
                {
                    return new CustomActionResult<AddResultVM>(new CustomActionResultVM<AddResultVM>
                    {
                        Exception = new Exception($"Product with Id {productId} not found.")
                    });
                }
              
               
            }
            catch (Exception ex)
            {
                return new CustomActionResult<AddResultVM>(new CustomActionResultVM<AddResultVM>
                {
                    Exception = ex
                });
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
                    var result = new ResultView<ProductVM>
                    {
                        Item = _product

                    };

                    var responseObj = new CustomActionResultVM<ResultView<ProductVM>>
                    {
                        Data = result

                    };
                    return new CustomActionResult<ResultView<ProductVM>>(responseObj);
                }
                else
                {
                    var result = new ResultView<ProductVM>
                    {
                        Exception = new Exception($"Product with Id {id} not found")
                    };

                    var responseObj = new CustomActionResultVM<ResultView<ProductVM>>
                    {
                        Exception = result.Exception
                    };

                    return new CustomActionResult<ResultView<ProductVM>>(responseObj);
                }
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<ProductVM>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<ProductVM>>(responseObj); 
            }
        }

        [HttpGet("listSimpleProducts")]
        public CustomActionResult<ResultView<ProductListSimpleVM[]>> ListSimpleRawMaterials()
        {
            try
            {
                // Call the repository to get the list of simple FoodTypes
                ProductListSimpleVM[] products = _iProductRepository.ListSimpeProducts();

                var result = new ResultView<ProductListSimpleVM[]>
                {
                    Item = products

                };

                var responseObj = new CustomActionResultVM<ResultView<ProductListSimpleVM[]>>
                {
                    Data = result

                };
                return new CustomActionResult<ResultView<ProductListSimpleVM[]>>(responseObj);
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<ProductListSimpleVM[]>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<ProductListSimpleVM[]>>(responseObj); ;
            }
        }
    }
}
