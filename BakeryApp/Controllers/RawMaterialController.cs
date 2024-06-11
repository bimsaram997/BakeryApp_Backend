
using Microsoft.AspNetCore.Mvc;
using Models.ActionResults;
using Models.Filters;
using Models.Requests;
using Models.Requests.Update_Requests;
using Models.ViewModels.Custom_action_result;
using Models.ViewModels;
using Models.ViewModels.FoodType;
using Models.ViewModels.RawMaterial;
using Repositories;
using Repositories.RawMarerialRepository;
using Repositories.RecipeRepository;
using Models.ViewModels.Recipe;
using Models.Data.RecipeData;

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
        public CustomActionResult<AddResultVM> AddRawMaterial([FromBody] AddRawMaterialRequest rawMaterialRequest)
        {
            try
            {
                var rawMaterial = new RawMaterialVM
                {
                    Name = rawMaterialRequest.Name,
                    Price = rawMaterialRequest.Price,
                    Quantity = rawMaterialRequest.Quantity,
                    LocationId = rawMaterialRequest.LocationId,
                    ImageURL = rawMaterialRequest.ImageURL,
                    AddedDate = DateTime.Now,
                    MeasureUnit = rawMaterialRequest.MeasureUnit
                };

                int rawMaterialId = _rawMaterialRepository.Add(rawMaterial);
                if (rawMaterialId > 0)
                {
                    var result = new AddResultVM
                    {
                        Id = rawMaterialId
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
                        Exception = new Exception("Raw material can't add!")
                    });
                }
            } catch (Exception ex)
            {
                return new CustomActionResult<AddResultVM>(new CustomActionResultVM<AddResultVM>
                {
                    Exception = ex
                });
            }
            
        }

        [HttpGet("getRawMaterialById/{rawMaterialId}")]
        public CustomActionResult<ResultView<RawMaterialVM>> GetRawMaterialById(int rawMaterialId)
        {
            try
            {
                RawMaterialVM rawMaterial = _rawMaterialRepository.GetById(rawMaterialId);
                if (rawMaterial != null)
                {
                    var result = new ResultView<RawMaterialVM>
                    {
                        Item = rawMaterial

                    };

                    var responseObj = new CustomActionResultVM<ResultView<RawMaterialVM>>
                    {
                        Data = result

                    };
                    return new CustomActionResult<ResultView<RawMaterialVM>>(responseObj);
                }
                else
                {
                    var result = new ResultView<RawMaterialVM>
                    {
                        Exception = new Exception($"Raw Material with Id {rawMaterialId} not found")
                    };

                    var responseObj = new CustomActionResultVM<ResultView<RawMaterialVM>>
                    {
                        Exception = result.Exception
                    };

                    return new CustomActionResult<ResultView<RawMaterialVM>>(responseObj);
                }
            } catch(Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<RawMaterialVM>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<RawMaterialVM>>(responseObj);
            }            
        }

        [HttpPut("updateRawMaterial/{rawMaterialId}")]
        public CustomActionResult<AddResultVM> UpdateRecipe(int rawMaterialId, [FromBody] UpdateRawMaterial updateRawMaterial)
        {

            try
            {
                RawMaterialVM rawMaterial = _rawMaterialRepository.GetById(rawMaterialId);
                if (rawMaterial != null)
                {
                    RawMaterialVM rawMaterialVM = new RawMaterialVM
                    {
                        Name = updateRawMaterial.Name,
                        Price = updateRawMaterial.Price,
                        LocationId = updateRawMaterial.LocationId,
                        ImageURL = updateRawMaterial.ImageURL,
                        Quantity = updateRawMaterial.Quantity,
                        MeasureUnit = updateRawMaterial.MeasureUnit
                    };
                    int updatedRawMaterialId = _rawMaterialRepository.UpdateById(rawMaterialId, rawMaterialVM);
                    var result = new AddResultVM
                    {
                        Id = updatedRawMaterialId
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
                        Exception = new Exception($"Raw material with Id {rawMaterialId} not found.")
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
        public CustomActionResult<ResultView<PaginatedRawMaterials>> GetAlRawMaterials([FromBody] RawMaterialListAdvanceFilter rawMaterialListAdvanceFilter)
        {
            try
            {
                var _rawMaterials= _iIRawMaterialRepository.GetAll(rawMaterialListAdvanceFilter);
                var result = new ResultView<PaginatedRawMaterials>
                {
                    Item = _rawMaterials

                };

                var responseObj = new CustomActionResultVM<ResultView<PaginatedRawMaterials>>
                {
                    Data = result

                };
                return new CustomActionResult<ResultView<PaginatedRawMaterials>>(responseObj);
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<PaginatedRawMaterials>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<PaginatedRawMaterials>>(responseObj);
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

        [HttpGet("listSimpleRawmaterials")]
        public CustomActionResult<ResultView<RawMaterialListSimpleVM[]>> ListSimpleRawMaterials()
        {
            try
            {
                // Call the repository to get the list of simple FoodTypes
                RawMaterialListSimpleVM[] rawMaterials = _iIRawMaterialRepository.ListSimpeRawMaterials();

                var result = new ResultView<RawMaterialListSimpleVM[]>
                {
                    Item = rawMaterials

                };

                var responseObj = new CustomActionResultVM<ResultView<RawMaterialListSimpleVM[]>>
                {
                    Data = result

                };
                return new CustomActionResult<ResultView<RawMaterialListSimpleVM[]>>(responseObj);
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<RawMaterialListSimpleVM[]>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<RawMaterialListSimpleVM[]>>(responseObj); ;
            }
        }
    }
}
