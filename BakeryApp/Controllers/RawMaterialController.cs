
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
        public IRepositoryBase<RawMaterialVM> _rawMaterialRepository;
        public IRecipeRepository _iRecipeRepository;
        public RawMaterialController(IRepositoryBase<RawMaterialVM> rawMaterialRepository,
            IRecipeRepository iRecipeRepository)
        {

            _rawMaterialRepository = rawMaterialRepository;
            _iRecipeRepository = iRecipeRepository;
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

                int rawMaterialId = _rawMaterialRepository.Add(rawMaterial);
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
                RawMaterialVM rawMaterial = _rawMaterialRepository.GetById(rawMaterialId);
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
                int updatedRawMaterialId = _rawMaterialRepository.UpdateById(rawMaterialId, rawMaterialVM);
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
                bool hasRecords = _iRecipeRepository.CheckRawMaterialsAssociatedWithRecipe(rawMaterialId);
                if (hasRecords)
                {
                    throw new Exception("Error in Deleting.Raw mterial has beend added to the recipe!");
                }

                int deletedRawMaterialId = _rawMaterialRepository.DeleteById(rawMaterialId);
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
