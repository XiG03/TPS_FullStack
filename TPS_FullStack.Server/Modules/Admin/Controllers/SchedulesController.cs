using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TPS_FullStack.Server.Modules.Admin
{
    [Route("api/v1/schedule")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly ISchedulesService _schedulesService;

        public SchedulesController(ISchedulesService schedulesService)
        {
            _schedulesService = schedulesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _schedulesService.ScheduleGetAllAsync();
            return StatusCode(result.statusCode, result);
        }

        [HttpGet("{LichhocID}")]
        public async Task<IActionResult> GetDetail(string LichhocID)
        {
            var result = await _schedulesService.ScheduleGetByIDAsync(LichhocID);
            return StatusCode(result.statusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ScheduleCreateRequest createRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _schedulesService.ScheduleCreateAsync(createRequest);
            return StatusCode(result.statusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ScheduleUpdateRequest updateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _schedulesService.ScheduleUpdateAsync(updateRequest);
            return StatusCode(result.statusCode, result);
        }

        [HttpDelete("{LichhocID}")]
        public async Task<IActionResult> Delete(string LichhocID)
        {
            var result = await _schedulesService.ScheduleDeleteAsync(LichhocID);
            return StatusCode(result.statusCode, result);
        }
    }
}