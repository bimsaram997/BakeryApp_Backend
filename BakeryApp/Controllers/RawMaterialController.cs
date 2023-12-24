using Microsoft.AspNetCore.Mvc;
using Models.Requests;
using Models.ViewModels;
using Models.ViewModels.FoodType;
using Models.ViewModels.RawMaterial;
using Repositories;
using Repositories.FoodItemRepository;
using Repositories.FoodTypeRepository;

namespace BakeryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RawMaterialController : ControllerBase
    {
        public IRepositoryBase<RawMaterialVM> _rawMaterialRepository;
        public RawMaterialController(IRepositoryBase<RawMaterialVM> rawMaterialRepository )
        {

            _rawMaterialRepository = rawMaterialRepository;      
        }

        [HttpPost("addRawMaterial")]
        public IActionResult AddRawMaterial([FromBody] AddRawMaterialRequest rawMaterialRequest)
        {

            var rawMaterial = new RawMaterialVM
            {
                name = rawMaterialRequest.Name,
                quantity = rawMaterialRequest.Quantity,
                imageURL = rawMaterialRequest.ImageURL,
                addedDate = DateTime.Now,
                rawMaterialQuantityType = rawMaterialRequest.RawMaterialQuantityType


            };

            int rawMaterialId = _rawMaterialRepository.Add(rawMaterial);

            return Ok();
        }
    }
}
