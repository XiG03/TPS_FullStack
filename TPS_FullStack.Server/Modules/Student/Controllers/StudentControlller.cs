using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TPS_FullStack.Server.Modules.Student
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentControlller : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentControlller(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpGet("info/{MaID}")]
        public async Task<IActionResult> GetStudentInfo(string MaID)
        {
            var result = await _studentService.GetStudentInfoAsync(MaID);
            return StatusCode(result.statusCode, result);
            // Implementation for getting student info
        }
        [HttpGet("schedule/{MaID}")]
        public async Task<IActionResult> GetStudentSchedule(string MaID)
        {
            var result = await _studentService.GetStudentScheduleAsync(MaID);
            return StatusCode(result.statusCode, result);
            // Implementation for getting student schedule
        }
        [HttpPost("checkin")]
        public async Task<IActionResult> StudentCheckIn(studentAttendance attendance)
        {
            var result = await _studentService.StudentCheckInAsync(attendance);
            return StatusCode(result.statusCode, result);
            // Implementation for student check-in
        }
        [HttpGet("courses/{MaID}")]
        public async Task<IActionResult> GetStudentCourses(string MaID)
        {
            var result = await _studentService.GetStudentCoursesAsync(MaID);
            return StatusCode(result.statusCode, result);
            // Implementation for getting student courses
        }
    }
}
