using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TPS_FullStack.Server.Modules.Teacher
{
    [Route("api/teacher/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet("information/{teacherId}")]
        public async Task<IActionResult> GetTeacherInformation(string teacherId)
        {
            var result = await _teacherService.GetTeacherInformationAsync(teacherId);
            return StatusCode(result.statusCode, result);
        }

        [HttpGet("schedules/{teacherId}")]
        public async Task<IActionResult> GetTeacherSchedules(string teacherId)
        {
            var result = await _teacherService.GetTeacherSchedulesAsync(teacherId);
            return StatusCode(result.statusCode, result);
        }

        [HttpPost("attendance")]
        public async Task<IActionResult> TeacherAttendance(teacherAttendance attendance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _teacherService.TeacherAttendanceAsync(attendance);
            return StatusCode(result.statusCode, result);
        }
    }
}
