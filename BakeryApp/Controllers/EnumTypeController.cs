using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Filters;
using Models.ViewModels.Address;
using Repositories;
using Repositories.EnumTypeRepository;
using Repositories.UserRepository;

namespace BakeryApp.Controllers
{
  /*  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]*/
    [Route("api/[controller]")]
    [ApiController]
    public class EnumTypeController : ControllerBase
    {
        public IEnumTypeRepository _iEnumTypeRepsoitory;
        private IConfiguration _config;
        public EnumTypeController(IEnumTypeRepository iEnumTypeRepsoitory, IRepositoryBase<AddressVM> addressRepository,
            IConfiguration config)
        {
            _iEnumTypeRepsoitory = iEnumTypeRepsoitory;
            _config = config;
        }

        [HttpGet("listAdvance")]
       
        public IActionResult GetAllEnumTypes()
        {
            try
            {
                var _enumTypes = _iEnumTypeRepsoitory.GetAll();
                return Created(nameof(GetAllEnumTypes), _enumTypes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error laoding Enum types: {ex.Message}");
            }

        }
    }
}
