using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TPS_FullStack.Server.Modules.Student
{
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
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
        [HttpGet("courseinfo/{HocvienID}/{KhoahocID}")]
        
        public async Task<IActionResult> GetStudentCourseInfo(string HocvienID, string KhoahocID)
        {
            var result = await _studentService.GetStudentCourseInfoAsync(HocvienID, KhoahocID);
            return StatusCode(result.statusCode, result);
            // Implementation for getting student course info
        }


        // Bai thu hoach
        [HttpGet("finalexaminfo/{HocvienID}/{KhoahocID}")]
        public async Task<IActionResult> GetFinalExamInfo(string HocvienID, string KhoahocID)
        {
            var result = await _studentService.GetFinalExamInfoAsync(HocvienID, KhoahocID);
            return StatusCode(result.statusCode, result);
            // Implementation for getting final exam info
        }
        [HttpPost("submitfinalexam")]
        public async Task<IActionResult> SubmitFinalExam(finalExam finalRecord)
        {
            var result = await _studentService.SubmitFinalExamAsync(finalRecord);
            return StatusCode(result.statusCode, result);
            // Implementation for submitting final exam
        }
    }
}
