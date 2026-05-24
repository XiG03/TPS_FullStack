using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TPS_FullStack.Server.Modules.Admin
{
    [Route("api/v1/teacher")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _teacherService.TeacherGetAllAsync();
            return StatusCode(result.statusCode, result);
        }

        [HttpGet("{MaID}")]
        public async Task<IActionResult> GetDetail(string MaID)
        {
            var result = await _teacherService.TeacherGetByIDAsync(MaID);
            return StatusCode(result.statusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeacherCreateResquest createRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _teacherService.TeacherCreateAsync(createRequest);
            return StatusCode(result.statusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TeacherUpdateResquest updateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _teacherService.TeacherUpdateAsync(updateRequest);
            return StatusCode(result.statusCode, result);
        }

        [HttpDelete("{MaID}")]
        public async Task<IActionResult> Delete(string MaID)
        {
            var result = await _teacherService.TeacherDeleteAsync(MaID);
            return StatusCode(result.statusCode, result);
        }
    }
}