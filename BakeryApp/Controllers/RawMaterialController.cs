
using Microsoft.AspNetCore.Mvc;
using Models.Requests;
using Models.Requests.Update_Requests;
using Models.ViewModels.RawMaterial;
using Repositories;
using Repositories.RecipeRepository;

namespace BakeryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RawMaterialController : ControllerBase
    {
        private IRepositoryBase<RawMaterialVM> rawMaterialRepository;
        private IRecipeRepository iRecipeRepository;

        public IRepositoryBase<RawMaterialVM> RawMaterialRepository { get => rawMaterialRepository; set => rawMaterialRepository = value; }
        public IRecipeRepository IRecipeRepository { get => iRecipeRepository; set => iRecipeRepository = value; }

        public RawMaterialController(IRepositoryBase<RawMaterialVM> rawMaterialRepository,
            IRecipeRepository iRecipeRepository)
        {

            RawMaterialRepository = rawMaterialRepository;
            IRecipeRepository = iRecipeRepository;
        }

        [HttpPost("addRawMaterial")]
        public IActionResult AddRawMaterial([FromBody] AddRawMaterialRequest rawMaterialRequest)
        {
            try
            {
                var rawMaterial = new RawMaterialVM
                {
                    name = rawMaterialRequest.Name,
                    quantity = rawMaterialRequest.Quantity,
                    imageURL = rawMaterialRequest.ImageURL,
                    addedDate = DateTime.Now,
                    rawMaterialQuantityType = rawMaterialRequest.RawMaterialQuantityType
                };

                int rawMaterialId = RawMaterialRepository.Add(rawMaterial);
                return Created(nameof(AddRawMaterial), rawMaterialId);
            } catch (Exception ex)
            {
                return BadRequest($"Error adding raw material: {ex.Message}");
            }
            
        }

        [HttpGet("getRawMaterialById/{rawMaterialId}")]
        public IActionResult GetRawMaterialById(int rawMaterialId)
        {
            try
            {
                RawMaterialVM rawMaterial = RawMaterialRepository.GetById(rawMaterialId);
                if(rawMaterial != null)
                {
                    return Created(nameof(GetRawMaterialById), rawMaterial);
                }else
                {
                    return NotFound($"Raw material with ID {rawMaterialId} not found.");
                }
            } catch(Exception ex)
            {
                return BadRequest($"Error retrieving Raw material: {ex.Message}");
            }            
        }

        [HttpPut("updateRawMaterial/{rawMaterialId}")]
        public IActionResult UpdateRecipe(int rawMaterialId, [FromBody] UpdateRawMaterial updateRawMaterial)
        {

            try
            {
                 RawMaterialVM  rawMaterialVM = new RawMaterialVM
                {
                    name = updateRawMaterial.name,
                    imageURL =  updateRawMaterial.imageURL,
                    quantity = updateRawMaterial.quantity,
                    rawMaterialQuantityType = updateRawMaterial.rawMaterialQuantityType
                 };
                int updatedRawMaterialId = RawMaterialRepository.UpdateById(rawMaterialId, rawMaterialVM);
                if (updatedRawMaterialId != -1)
                {
                    // Return a successful response
                    return Created(nameof(UpdateRecipe), updatedRawMaterialId);
                }
                else
                {
                    // Handle the case where the recipe is not found
                    return NotFound($"Raw material with ID {rawMaterialId} not found.");
                }
               
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating Raw material: {ex.Message}");
            }

        }

        [HttpDelete("deleteRawMaterialByID/{rawMaterialId}")]
        public IActionResult DeleteRawMaterial(int rawMaterialId)
        {
            try
            {
                bool hasRecords = IRecipeRepository.CheckRawMaterialsAssociatedWithRecipe(rawMaterialId);
                if (hasRecords)
                {
                    throw new Exception("Error in Deleting.Raw mterial has beend added to the recipe!");
                }

                int deletedRawMaterialId = RawMaterialRepository.DeleteById(rawMaterialId);
                if (deletedRawMaterialId != -1)
                {
                    // Return a successful response
                    return Created(nameof(DeleteRawMaterial), deletedRawMaterialId);
                }
                else
                {
                    // Handle the case where the recipe is not found
                    return NotFound($"Raw material with ID {rawMaterialId} not found.");
                }

            } catch (Exception ex)
            {
                return BadRequest($"Error deleting raw material: {ex.Message}");
            }
          
        }
    }
}
