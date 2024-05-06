
using Microsoft.AspNetCore.Mvc;
using Models.Filters;
using Models.Requests;
using Models.Requests.Update_Requests;
using Models.ViewModels.RawMaterial;
using Repositories;
using Repositories.RawMarerialRepository;
using Repositories.RecipeRepository;

namespace BakeryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RawMaterialController : ControllerBase
    {
        private IRepositoryBase<RawMaterialVM> rawMaterialRepository;
        private IRecipeRepository iRecipeRepository;
        public IRepositoryBase<RawMaterialVM> _rawMaterialRepository { get => rawMaterialRepository; set => rawMaterialRepository = value; }
        public IRecipeRepository IRecipeRepository { get => iRecipeRepository; set => iRecipeRepository = value; }
        public IRawMaterialRepository _iIRawMaterialRepository;
        public RawMaterialController(IRepositoryBase<RawMaterialVM> rawMaterialRepository,
            IRecipeRepository iRecipeRepository, IRawMaterialRepository iRawMaterialRepository)
        {

            _rawMaterialRepository = rawMaterialRepository;
            IRecipeRepository = iRecipeRepository;
            _iIRawMaterialRepository = iRawMaterialRepository;
        }

        [HttpPost("addRawMaterial")]
        public IActionResult AddRawMaterial([FromBody] AddRawMaterialRequest rawMaterialRequest)
        {
            try
            {
                var rawMaterial = new RawMaterialVM
                {
                    Name = rawMaterialRequest.Name,
                    Quantity = rawMaterialRequest.Quantity,
                    ImageURL = rawMaterialRequest.ImageURL,
                    AddedDate = DateTime.Now,
                    RawMaterialQuantityType = rawMaterialRequest.RawMaterialQuantityType
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
                    Name = updateRawMaterial.Name,
                    ImageURL =  updateRawMaterial.ImageURL,
                    Quantity = updateRawMaterial.Quantity,
                    RawMaterialQuantityType = updateRawMaterial.RawMaterialQuantityType
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

        [HttpPost("listAdvance")]
        public IActionResult GetAlRawMaterials([FromBody] RawMaterialListAdvanceFilter rawMaterialListAdvanceFilter)
        {
            try
            {
                var _rawMaterials= _iIRawMaterialRepository.GetAll(rawMaterialListAdvanceFilter);
                return Created(nameof(GetAlRawMaterials), _rawMaterials);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error loading raw materials: {ex.Message}");
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
