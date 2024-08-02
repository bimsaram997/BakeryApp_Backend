using Microsoft.AspNetCore.Mvc;
using Models.ViewModels.Address;
using Repositories.EnumTypeRepository;
using Repositories;
using Repositories.RolesRepository;
using Models.ViewModels.Location;
using Models.ViewModels.Roles;
using Models.ActionResults;
using Models.Filters;
using Models.ViewModels.Custom_action_result;
using Models.ViewModels;
using Models.Requests.Update_Requests;
using Models.ViewModels.Product;

namespace BakeryApp.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        public IRolesRepository _iRolesRepository;
        public IRepositoryBase<RolesVM> _roleRepository;
        
        public RolesController(IRolesRepository iRolesRepository,
            IRepositoryBase<RolesVM> roleRepository
            )
        {
            _iRolesRepository = iRolesRepository;
            _roleRepository = roleRepository;
            
        }


        [HttpGet("getRoleById/{roleId}")]
        public CustomActionResult<ResultView<RolesVM>> GetRoleById(int roleId)
        {
            try
            {
                RolesVM role = _roleRepository.GetById(roleId);
                if (role != null)
                {
                    var result = new ResultView<RolesVM>
                    {
                        Item = role

                    };

                    var responseObj = new CustomActionResultVM<ResultView<RolesVM>>
                    {
                        Data = result

                    };
                    return new CustomActionResult<ResultView<RolesVM>>(responseObj);
                }
                else
                {
                    var result = new ResultView<RolesVM>
                    {
                        Exception = new Exception($"Role with Id {roleId} not found")
                    };

                    var responseObj = new CustomActionResultVM<ResultView<RolesVM>>
                    {
                        Exception = result.Exception
                    };

                    return new CustomActionResult<ResultView<RolesVM>>(responseObj);
                }
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<RolesVM>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<RolesVM>>(responseObj);
            }
        }


        [HttpPost("updateRoleById/{roleId}")]
        public CustomActionResult<AddResultVM> UpdateRoleById(int roleId, [FromBody] UpdateRole updateItem)
        {
            try
            {
                RolesVM rolesVM = _roleRepository.GetById(roleId);
                if (rolesVM != null)
                {
                    RolesVM role = new RolesVM
                    {
                        RoleName = updateItem.RoleName,
                        RoleDescription = updateItem.RoleDescription,
                        Status = updateItem.Status,
                        LocationId = updateItem.LocationId,
                       

                    };
                    int updatedRoleId = _roleRepository.UpdateById(roleId, role);
                    var result = new AddResultVM
                    {
                        Id = updatedRoleId
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
                        Exception = new Exception($"Role with Id {roleId} not found.")
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
        public CustomActionResult<ResultView<PaginatedRoles>> GellAllRoles([FromBody] RoleAdvanceFilter roleAdvanceFilter)
        {
            try
            {
                PaginatedRoles _roles = _iRolesRepository.GetAll(roleAdvanceFilter);


                var result = new ResultView<PaginatedRoles>
                {
                    Item = _roles

                };

                var responseObj = new CustomActionResultVM<ResultView<PaginatedRoles>>
                {
                    Data = result

                };
                return new CustomActionResult<ResultView<PaginatedRoles>>(responseObj);
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<PaginatedRoles>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<PaginatedRoles>>(responseObj);
            }

        }
    }
    }
