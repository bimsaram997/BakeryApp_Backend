using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Data.EnumType;
using Models.Filters;
using Models.Requests;
using Models.Requests.Update_Requests;
using Models.ViewModels.MasterData;
using Models.ViewModels.Product;
using Repositories;
using Repositories.MasterDataRepository;

namespace BakeryApp.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class MasterDataController : ControllerBase
    {
        public IRepositoryBase<MasterDataVM> _masterDataRepository;
        public IMasterDataRepository _iMasterDataRepository;
        public MasterDataController(IRepositoryBase<MasterDataVM> masterDataRepository,
            IMasterDataRepository iMasterDataRepository)
        {
            _masterDataRepository = masterDataRepository;
            _iMasterDataRepository= iMasterDataRepository;
        }
        [HttpPost("addMasterData")]
        public IActionResult AddMasterData([FromBody] AddMasterDataRequest masterDataRequest)
        {
            try
            {
                var masterData = new MasterDataVM
                {
                    MasterDataName = masterDataRequest.MasterDataName,
                    AddedDate = masterDataRequest.AddedDate,
                    MasterColorCode = masterDataRequest.MasterColorCode,
                    MasterDataSymbol = masterDataRequest.MasterDataSymbol,
                    EnumTypeId = masterDataRequest.EnumTypeId,

                };

                int id = _masterDataRepository.Add(masterData);
                return Created(nameof(AddMasterData), id);

            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding Mater data: {ex.Message}");
            }
        }

        [HttpPost("listAdvance")]
        public IActionResult GetAllMasterData([FromBody] MasterDataListAdvanceFilter masterDataListAdvanceFilter)
        {
            try
            {
                var _masterData = _iMasterDataRepository.GetAll(masterDataListAdvanceFilter);
                return Created(nameof(GetAllMasterData), _masterData);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error loading master data: {ex.Message}");
            }

        }

        [HttpGet("getMasterDataById/{id}")]
        public IActionResult GetMasterDataById(int id)
        {
            try
            {
                // Call the repository to get the recipe by ID
                MasterDataVM _masterData = _masterDataRepository.GetById(id);

                if (_masterData != null)
                {
                    return Created(nameof(GetMasterDataById), _masterData);
                }
                else
                {
                    // Handle the case where the recipe is not found
                    return NotFound($"Master data with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving Master data: {ex.Message}");
            }
        }

        [HttpGet("getMasterDataByEnumTypeId/{id}")]
        public IActionResult GetMasterDataByEnumTypeId(int id)
        {
            try
            {
                // Call the repository to get the recipe by ID
                var _masterData = _iMasterDataRepository.GetByEnumType(id);

                if (_masterData != null)
                {
                    return Created(nameof(GetMasterDataByEnumTypeId), _masterData);
                }
                else
                {
                    // Handle the case where the recipe is not found
                    return NotFound($"Master data with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving Master data: {ex.Message}");
            }
        }

        [HttpPost("updateMasterDataById/{masterDataId}")]
        public IActionResult UpdateMasterDataById(int masterDataId, [FromBody] UpdateMasterData updateItem)
        {
            try
            {
                MasterDataVM masterData = new MasterDataVM
                {
                    MasterDataName = updateItem.MasterDataName,
                    MasterColorCode = updateItem.MasterColorCode,
                    MasterDataSymbol = updateItem.MasterDataSymbol,
                    EnumTypeId = updateItem.EnumTypeId,
                    

                };
                int updatedMasterData = _masterDataRepository.UpdateById(masterDataId, masterData);
                return Ok(updatedMasterData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("deleteMasterDataById/{masterDataId}")]
        public IActionResult DeleteById(int masterDataId)
        {
            try
            {
               
                int updatedMasterData = _masterDataRepository.DeleteById(masterDataId);
                return Ok(updatedMasterData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
