using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPS_FullStack.Server.Modules.Admin;

namespace MyApp.Namespace
{
    [Route("api/admin/teacher")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ITeachersService _teacherService;
        public TeachersController(ITeachersService teachersService)
        {
            _teacherService = teachersService;
        }
        [HttpGet]
        public async Task<IActionResult> TeacherGetAll()
        {
            var result = await _teacherService.TeacherGetAllAsync();

            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateTeacher(TeacherCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _teacherService.CreateTeacherAsync(createDto);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }


        [HttpDelete("{Maid}")]
        public async Task<IActionResult> DeleteTeacherId(string MaID)
        {
            var result = await _teacherService.DeleteTeacherAsync(MaID);

            if (result)
            {
                return Ok("Deleted teacher id");
            }

            return BadRequest();
        }
    }
}
