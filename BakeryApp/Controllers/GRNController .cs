using Microsoft.AspNetCore.Mvc;
using Repositories.GRNRepository;

namespace BakeryApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GRNController : ControllerBase
    {
        private readonly IGRNRepository _grnRepository;

        public GRNController(IGRNRepository grnRepository)
        {
            _grnRepository = grnRepository;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateGRN()
        {
            try
            {
                var grnNumber = await _grnRepository.GenerateGRNAsync();
                return Ok(new { GRNNumber = grnNumber });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
