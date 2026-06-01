using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    [Route("api/v1/student")]
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _studentService.StudentGetAllAsync();
            return StatusCode(result.statusCode, result);
        }

        [Authorize(Roles = "Hocvien, Admin")]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe(string? HocvienID)
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(studentId))
            {
                return Unauthorized(new ServiceDefault<StudentDetailResponse>
                {
                    statusCode = StatusCodes.Status401Unauthorized,
                    Message = "Không xác định được học viên hiện tại",
                    Data = null
                });
            }

            var result = await _studentService.StudentGetByIDAsync(studentId);
            return StatusCode(result.statusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{HocvienID}")]
        public async Task<IActionResult> GetDetail(string HocvienID)
        {
            var result = await _studentService.StudentGetByIDAsync(HocvienID);
            return StatusCode(result.statusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentCreateRequest createRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _studentService.StudentCreateAsync(createRequest);
            return StatusCode(result.statusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] StudentUpdateRequest updateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _studentService.StudentUpdateAsync(updateRequest);
            return StatusCode(result.statusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{HocvienID}")]
        public async Task<IActionResult> Delete(string HocvienID)
        {
            var result = await _studentService.StudentDeleteAsync(HocvienID);
            return StatusCode(result.statusCode, result);
        }

        [Authorize(Roles = "Hocvien, Admin")]
        [HttpGet("me/schedules")]
        public async Task<IActionResult> GetSchedules(string? HocvienID)
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _studentService.StudentGetScheduleByIDAsync(studentId);
            return StatusCode(result.statusCode, result);
        }

        [Authorize(Roles = "Hocvien, Admin")]
        [HttpGet("me/schedule/{LichhocID}")]
        public async Task<IActionResult> GetScheduleDetail(string? HocvienID, string? LichhocID)
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _studentService.StudentGetScheduleDetailAsync(studentId, LichhocID);
            return StatusCode(result.statusCode, result);
        }

        [Authorize(Roles = "Hocvien, Admin")]
        [HttpPost("me/schedule/{LichhocID}/checkin")]
        public async Task<IActionResult> Checkin(string? HocvienID, string? LichhocID)
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _studentService.StudentCheckinAsync(studentId, LichhocID);
            return StatusCode(result.statusCode, result);
        }
    }
}
