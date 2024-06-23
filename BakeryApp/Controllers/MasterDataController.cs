using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ActionResults;
using Models.Data.EnumType;
using Models.Filters;
using Models.Requests;
using Models.Requests.Update_Requests;
using Models.ViewModels.Custom_action_result;
using Models.ViewModels;
using Models.ViewModels.MasterData;
using Models.ViewModels.Product;
using Repositories;
using Repositories.MasterDataRepository;
using Models.ViewModels.Recipe;
using Models.Data.RecipeData;

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
        public CustomActionResult<AddResultVM> AddMasterData([FromBody] AddMasterDataRequest masterDataRequest)
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
                    MasterValueCode = masterDataRequest.MasterValueCode

                };

                int id = _masterDataRepository.Add(masterData);
                if (id > 0)
                {
                    var result = new AddResultVM
                    {
                        Id = id
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
                        Exception = new Exception("Master data can't add!")
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
        public CustomActionResult<ResultView<PaginatedMasterData>> GetAllMasterData([FromBody] MasterDataListAdvanceFilter masterDataListAdvanceFilter)
        {
            try
            {
                var _masterData = _iMasterDataRepository.GetAll(masterDataListAdvanceFilter);
                var result = new ResultView<PaginatedMasterData>
                {
                    Item = _masterData

                };

                var responseObj = new CustomActionResultVM<ResultView<PaginatedMasterData>>
                {
                    Data = result

                };
                return new CustomActionResult<ResultView<PaginatedMasterData>>(responseObj);
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<PaginatedMasterData>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<PaginatedMasterData>>(responseObj);
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
                    var result = new ResultView<MasterDataVM>
                    {
                        Item = _masterData

                    };

                    var responseObj = new CustomActionResultVM<ResultView<MasterDataVM>>
                    {
                        Data = result

                    };
                    return new CustomActionResult<ResultView<MasterDataVM>>(responseObj);
                }
                else
                {
                    var result = new ResultView<MasterDataVM>
                    {
                        Exception = new Exception($"Master data with Id {id} not found")
                    };

                    var responseObj = new CustomActionResultVM<ResultView<MasterDataVM>>
                    {
                        Exception = result.Exception
                    };

                    return new CustomActionResult<ResultView<MasterDataVM>>(responseObj);
                }
            
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<MasterDataVM>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<MasterDataVM>>(responseObj);
            }
        }

        [HttpGet("getMasterDataByEnumTypeId/{id}")]
        public CustomActionResult<ResultView<AllMasterData>> GetMasterDataByEnumTypeId(int id)
        {
            try
            {
                // Call the repository to get the recipe by ID
                AllMasterData _masterData = _iMasterDataRepository.GetByEnumType(id);

                if (_masterData != null)
                {
                    var result = new ResultView<AllMasterData>
                    {
                        Item = _masterData

                    };

                    var responseObj = new CustomActionResultVM<ResultView<AllMasterData>>
                    {
                        Data = result

                    };
                    return new CustomActionResult<ResultView<AllMasterData>>(responseObj);
                }
                else
                {
                    var result = new ResultView<AllMasterData>
                    {
                        Exception = new Exception($"Master data with Id {id} not found")
                    };

                    var responseObj = new CustomActionResultVM<ResultView<AllMasterData>>
                    {
                        Exception = result.Exception
                    };

                    return new CustomActionResult<ResultView<AllMasterData>>(responseObj);
                }
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<AllMasterData>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<AllMasterData>>(responseObj);
            }
        }

        [HttpPost("updateMasterDataById/{masterDataId}")]
        public CustomActionResult<AddResultVM> UpdateMasterDataById(int masterDataId, [FromBody] UpdateMasterData updateItem)
        {
            try
            {
                MasterDataVM masterDataVM = _masterDataRepository.GetById(masterDataId);
                if (masterDataVM != null)
                {

                    MasterDataVM masterData = new MasterDataVM
                    {
                        MasterDataName = updateItem.MasterDataName,
                        MasterColorCode = updateItem.MasterColorCode,
                        MasterDataSymbol = updateItem.MasterDataSymbol,
                        EnumTypeId = updateItem.EnumTypeId,
                        MasterValueCode = updateItem.MasterValueCode,

                    };
                    int updatedMasterData = _masterDataRepository.UpdateById(masterDataId, masterData);
                    var result = new AddResultVM
                    {
                        Id = updatedMasterData
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
                        Exception = new Exception($"Master data with Id {masterDataId} not found")
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

        [HttpDelete("deleteMasterDataById/{masterDataId}")]
        public CustomActionResult<AddResultVM> DeleteById(int masterDataId)
        {
            try
            {
                MasterDataVM masterData = _masterDataRepository.GetById(masterDataId);
                if (masterData != null)
                {
                    MasterDataVM masterDataVM = _masterDataRepository.GetById(masterDataId);
                    int updatedMasterData = _masterDataRepository.DeleteById(masterDataId);
                    var result = new AddResultVM
                    {
                        Id = updatedMasterData
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
                        Exception = new Exception($"Master data with Id {masterDataId} not found")
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

    }
}
