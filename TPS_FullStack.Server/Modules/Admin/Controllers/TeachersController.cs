using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPS_FullStack.Server.Modules.Admin;

namespace TPS_FullStack.Server.Modules.Admin
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
        [HttpGet("teachers")]
        public async Task<IActionResult> TeacherGetAll()
        {
            var result = await _teacherService.TeacherGetAllAsync();

            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpGet("{MaId}")]
        public async Task<IActionResult> TeacherDetail(string MaId)
        {
            var result = await _teacherService.TeacherDetailAsync(MaId);
            if(result != null)
            {
                return BadRequest();
            }

            return Ok(result);
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
                return Created();
            }
            return BadRequest();
        }


        [HttpDelete("{MaId}")]
        public async Task<IActionResult> DeleteTeacherId(string MaID)
        {
            var result = await _teacherService.DeleteTeacherAsync(MaID);

            if (result)
            {
                return Ok("Deleted teacher id");
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTeacherById(TeacherUpdateDto updateDto)
        {
            var result = await _teacherService.UpdateTeacherAsync(updateDto);

            if(result == null)
            {
                return BadRequest("Can not update teacher ");
            }
            return Ok(result);
        }
    }
}
