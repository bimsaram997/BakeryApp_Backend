using Microsoft.AspNetCore.Mvc;
using Models.Data.RawMaterialData;
using Models.Requests;
using Models.ViewModels;
using Models.ViewModels.FoodItem;
using Models.ViewModels.FoodType;
using Models.ViewModels.RawMaterial;
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
        public IRepositoryBase<RawMaterialVM> _rawMaterialRepository;

        public FoodItemController( 
            IRepositoryBase<FoodItemVM> foodRepository,
            IRepositoryAllBase<AllFoodItemVM> foodItemAllBase,
            IFoodTypeRepository ifoodTypeRepository,
            IRepositoryBase<FoodTypeVM> foodTypeRepository,
            IRepositoryBase<RawMaterialVM> rawMaterialRepository)
        {
  
           _foodRepository = foodRepository;
           _foodItemAllBase= foodItemAllBase;
           _iFoodTypeRepository = ifoodTypeRepository;
           _foodTypeRepository= foodTypeRepository;
           _rawMaterialRepository= rawMaterialRepository;

        }

        //Add food item
        [HttpPost("addFood")]
        public IActionResult AddFoodItem([FromBody] AddFoodItemRequest foodItemRequest, int foodItemCount)
        {
            if(foodItemCount > 0)
            {
                for (int i = 0; i < foodItemCount; i++)
                {
                    var foodItem = new FoodItemVM
                    {
                        FoodDescription = foodItemRequest.FoodDescription,
                        FoodPrice = foodItemRequest.FoodPrice,
                        ImageURL = foodItemRequest.ImageURL,
                        AddedDate = DateTime.Now,
                        FoodTypeId = foodItemRequest.FoodTypeId,
                    };

                    int foodId = _foodRepository.Add(foodItem);

                    FoodTypeVM foodTypeVM = _foodTypeRepository.GetById(foodItemRequest.FoodTypeId);

                    if (foodTypeVM != null)
                    {
                        _iFoodTypeRepository.UpdateFoodTypeCountByFoodTypeId(foodTypeVM.Id);

                        /*var rawMaterials = _iFoodTypeRawMaterialRepository.GetByFoodTypeId(foodItemRequest.FoodTypeId).ToList();

                        foreach (var rawMaterial in rawMaterials)
                        {
                            var rawMaterialVM = _rawMaterialRepository.GetById(rawMaterial.RawMaterialId);

                            switch (rawMaterialVM.RawMaterialQuantityType)
                            {
                                case RawMaterialQuantityType.Kg:
                                    // do nothing, add the required quantity to FoodType+Rawmaterial table
                                    break;
                            }
                        }*/
                    }
                }
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
