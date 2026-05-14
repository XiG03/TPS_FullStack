using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TPS_FullStack.Server.Modules.Admin
{
    [Route("api/admin/schedule")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public async Task<IActionResult> ScheduleGetAll()
        {
            var result = await _scheduleService.ScheduleGetAllAsync();
            return StatusCode(result.statusCode, result);
        }
        [HttpPost]
        public async Task<IActionResult> ScheduleInsert(ScheduleCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _scheduleService.ScheduleInsertAsync(createDto);
            return StatusCode(result.statusCode,result);
        }
        [HttpPut]
        public async Task<IActionResult> ScheduleUpdate(ScheduleUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _scheduleService.ScheduleUpdateAsync(updateDto);
            return StatusCode(result.statusCode, result);
        }
        [HttpDelete("{MaID}")]
        public async Task<IActionResult> ScheduleDelete(string MaID)
        {
            var result = await _scheduleService.ScheduleDeleteAsync(MaID);
            return StatusCode(result.statusCode, result);
        }
        [HttpPost]
        public async Task<IActionResult> TeacherAttendance(ScheduleTeacherAttendanceDto attendanceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _scheduleService.ScheduleTeacherAttendanceAsync(attendanceDto);
            return StatusCode(result.statusCode, result);
        }
        [HttpPost]
        public async Task<IActionResult> StudentAttendance(ScheduleStudentAttendanceDto attendanceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _scheduleService.ScheduleStudentAttendanceAsync(attendanceDto);
            return StatusCode(result.statusCode, result);
        }
    }
}
