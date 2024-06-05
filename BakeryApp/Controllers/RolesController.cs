using Microsoft.AspNetCore.Mvc;
using Models.ViewModels.Address;
using Repositories.EnumTypeRepository;
using Repositories;
using Repositories.RolesRepository;

namespace BakeryApp.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        public IRolesRepository _iRolesRepository;
        private IConfiguration _config;
        public RolesController(IRolesRepository iRolesRepository,
            IConfiguration config)
        {
            _iRolesRepository = iRolesRepository;
            _config = config;
        }

        [HttpGet("listAdvance")]

        public IActionResult GetAllRoles()
        {
            try
            {
                var roles = _iRolesRepository.GetAll();
                return Created(nameof(GetAllRoles), roles);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error laoding roles: {ex.Message}");
            }

        }
    }
}
