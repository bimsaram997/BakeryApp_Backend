using Microsoft.AspNetCore.Mvc;
using Models.ActionResults;
using Models.Data.RecipeData;
using Models.Requests;
using Models.ViewModels;
using Models.ViewModels.Custom_action_result;
using Models.ViewModels.Product;
using Models.ViewModels.RawMaterial;
using Models.ViewModels.Recipe;
using Models.ViewModels.Stock;
using Models.ViewModels.StockItem;
using Repositories;
using Repositories.ProductRepository;
using Repositories.RawMarerialRepository;
using Repositories.RecipeRepository;
using Repositories.StockRepository;

namespace BakeryApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class StockController : Controller
    {
        public IRepositoryBase<StockVM> _stockRepository;
        public IRepositoryBase<RawMaterialVM> _rawMaterialRepository;
        public IRepositoryBase<RecipeVM> _recipeRepository;
        public IRepositoryBase<ProductVM> _productRepository;
        public IRecipeRepository _iRecipeRepository;
        public IRawMaterialRepository _iIRawMaterialRepository;
        public IStockItemRepository _iStockItemRepository;
        public IStockRepository _iStockRepository;

        public StockController(IRepositoryBase<StockVM> stockRepository,
            IRepositoryBase<RawMaterialVM> rawMaterialRepository,
              IRepositoryBase<ProductVM> productRepository,
          IRepositoryBase<RecipeVM> recipeRepository,
          IRawMaterialRepository iRawMaterialRepository,
           IRecipeRepository iRecipeRepository,
           IStockItemRepository iStockItemRepository,
           IStockRepository iStockRepository)
        {

            _stockRepository = stockRepository;
            _rawMaterialRepository = rawMaterialRepository;
            _recipeRepository = recipeRepository;
            _productRepository = productRepository;
            _iRecipeRepository = iRecipeRepository;
            _iIRawMaterialRepository = iRawMaterialRepository;
            _iStockItemRepository = iStockItemRepository;
            _iStockRepository = iStockRepository;
        }
        [HttpPost("addStock")]
        public CustomActionResult<AddResultVM> AddStock([FromBody] AddStockRequest stockRequest)
        {
            try
            {
                RecipeVM recipe = _recipeRepository.GetById(stockRequest.RecipeId);
                ProductVM product = _productRepository.GetById(stockRequest.ProductId);
                Dictionary<int, double> rawMaterialQuantitiesUsed = new Dictionary<int, double>();
                if (recipe != null && product != null && stockRequest.ItemQuantity > 0)
                {
                    RawMaterialQuantity[] rawMaterialQuantities = CalculateRawMaterialQuantities(stockRequest.RecipeId, stockRequest.ItemQuantity);
                    int batchId = GenerateBatchId();
                    int stockId = 0;
                    var stock = new StockVM
                    {
                        BatchId = batchId,
                        ProductId = stockRequest.ProductId,
                        CostCode = stockRequest.CostCode,
                        SellingPrice = stockRequest.SellingPrice,
                        CostPrice = stockRequest.CostPrice,
                        RecipeId = stockRequest.RecipeId,
                        SupplyTypeId = stockRequest.SupplyTypeId,
                       SupplierId = stockRequest.SupplierId,
                        ManufacturedDate = stockRequest.ManufacturedDate,
                        ExpiredDate = stockRequest.ExpiredDate,
                        ItemQuantity = stockRequest.ItemQuantity,
                        ReorderLevel = stockRequest.ReorderLevel,
                        AddedDate = stockRequest.AddedDate,
                        RawMaterialQuantities = rawMaterialQuantities,
                    };
                    if (stockRequest.SupplierId != null)
                    {
                        stockId = _stockRepository.Add(stock);
                        AddStockItems(stock, stockId);
                        if (stockId > 0)
                        {
                            var result = new AddResultVM
                            {
                                Id = stockId
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
                                Exception = new Exception("Stock can't add!")
                            });
                        }
                    }
                    else
                    {
                        if (CheckAvailability(rawMaterialQuantities))
                        {
                            foreach (var rawMaterialQuantity in rawMaterialQuantities)
                            {
                                // Get the current quantity from the raw materials repository
                                double currentQuantity = _rawMaterialRepository.GetById(rawMaterialQuantity.RawMaterialId).Quantity;

                                // Update the quantity of raw material
                                UpdateQuantity(rawMaterialQuantity.RawMaterialId, currentQuantity, rawMaterialQuantity.TotalQuantityNeeded);
                            }
                            stockId = _stockRepository.Add(stock);
                            AddStockItems(stock, stockId);
                            if (stockId > 0)
                            {
                                var result = new AddResultVM
                                {
                                    Id = stockId
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
                                    Exception = new Exception("Stock can't add!")
                                });
                            }
                        }
                        else
                        {
                            return new CustomActionResult<AddResultVM>(new CustomActionResultVM<AddResultVM>
                            {
                                Exception = new Exception("Raw materials stock is not enough to make operation")
                            });
                        }
                    }
                }
                else
                {
                    return new CustomActionResult<AddResultVM>(new CustomActionResultVM<AddResultVM>
                    {
                        Exception = new Exception(recipe == null ? "No recipe found" : product == null ? "No Product found" : "Item count is zero")
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

        [HttpGet("getProductId/{prodId}")]
        public CustomActionResult<AddResultVM> GetProductId(int prodId)
        {

            try
            {
               
                int? productId = _iStockRepository.CheckProductAssociatedWithStock(prodId);
                if (productId == 0)
                {
                    var result = new AddResultVM
                    {
                        Id = 0
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
                        Exception = new Exception("Product is associated with stock. Please select new product.")
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


        private void AddStockItems(StockVM stockVM, int stockId)
        {
            var stockItem = new StockItemVM()
            {
                StockId = stockId,
                SellingPrice = stockVM.SellingPrice,
                ProductId = stockVM.ProductId,
                CostPrice = stockVM.CostPrice,
                BatchId = stockVM.BatchId,
                ManufacturedDate = stockVM.ManufacturedDate,
                ExpiredDate = stockVM.ExpiredDate,
                SupplierId = stockVM.SupplierId,
                ItemQuantity = stockVM.ItemQuantity
            };
            if(stockId > 0)
            {
                int[] stockItems = _iStockItemRepository.AddStockItem(stockItem);
            }
        } 

        private RawMaterialQuantity[] CalculateRawMaterialQuantities(int recipeId, int itemCount)
        {
            RecipeRawMaterial[] recipeRawMaterials = _iRecipeRepository.GetRawMaterialsByRecipeId(recipeId);

            List<RawMaterialQuantity> rawMaterialQuantities = new List<RawMaterialQuantity>();

            foreach (var rawMaterial in recipeRawMaterials)
            {
                double totalQuantityNeeded = rawMaterial.rawMaterialQuantity * itemCount;

                // Add to array or collection
                rawMaterialQuantities.Add(new RawMaterialQuantity
                {
                    RawMaterialId = rawMaterial.rawMaterialId,
                    TotalQuantityNeeded = totalQuantityNeeded
                });
            }

            return rawMaterialQuantities.ToArray();
        }

        private bool CheckAvailability(RawMaterialQuantity[] rawMaterialQuantities)
        {
            RawMaterialListSimpleVM[] rawMaterialListSimples = _iIRawMaterialRepository.ListSimpeRawMaterials();

            // Create a dictionary to quickly access available quantities by rawMaterialId
            Dictionary<int, double> availableQuantities = rawMaterialListSimples.ToDictionary(r => r.Id, r => r.Quantity);

            foreach (var rawMaterialQuantity in rawMaterialQuantities)
            {
                if (availableQuantities.ContainsKey(rawMaterialQuantity.RawMaterialId) &&
                    availableQuantities[rawMaterialQuantity.RawMaterialId] < rawMaterialQuantity.TotalQuantityNeeded)
                {
                    // Insufficient quantity for this raw material
                    return false;
                }
            }

            // All raw materials have sufficient quantity
            return true;

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
                int updatedRawMatId = _iIRawMaterialRepository.UpdateRawMaterialCountbyRawMatId(rawMatId, newQuantity);
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
                throw ex;
            }
        }

        private int GenerateBatchId()
        {
            var timestamp = DateTime.UtcNow;
            var random = new Random();
            var randomPart = random.Next(1, 100);

            // Combine timestamp and random number to create a unique batch ID
            var batchIdString = $"{timestamp:yyyyMMdd}{randomPart}";

            // Convert the string to a long
            if (int.TryParse(batchIdString, out var batchId))
            {
                return batchId;
            }

            throw new InvalidOperationException("Failed to generate a valid batch ID.");
        }
    }
}
