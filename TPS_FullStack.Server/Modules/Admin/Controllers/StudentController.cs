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

        private string? ResolveStudentId(string? HocvienID = null)
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")
                ?? HocvienID;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _studentService.StudentGetAllAsync();
            return StatusCode(result.statusCode, result);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe(string? HocvienID)
        {
            var studentId = ResolveStudentId(HocvienID);
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

        [HttpGet("{HocvienID}")]
        public async Task<IActionResult> GetDetail(string HocvienID)
        {
            var result = await _studentService.StudentGetByIDAsync(HocvienID);
            return StatusCode(result.statusCode, result);
        }

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

        [HttpDelete("{HocvienID}")]
        public async Task<IActionResult> Delete(string HocvienID)
        {
            var result = await _studentService.StudentDeleteAsync(HocvienID);
            return StatusCode(result.statusCode, result);
        }
        [HttpGet("me/schedules")]
        public async Task<IActionResult> GetSchedules(string? HocvienID)
        {
            var result = await _studentService.StudentGetScheduleByIDAsync(ResolveStudentId(HocvienID));
            return StatusCode(result.statusCode, result);
        }
        [HttpGet("me/schedule/{LichhocID}")]
        public async Task<IActionResult> GetScheduleDetail(string? HocvienID, string? LichhocID)
        {
            var result = await _studentService.StudentGetScheduleDetailAsync(ResolveStudentId(HocvienID), LichhocID);
            return StatusCode(result.statusCode, result);
        }
        [HttpPost("me/schedule/{LichhocID}/checkin")]
        public async Task<IActionResult> Checkin(string? HocvienID, string? LichhocID)
        {
            var result = await _studentService.StudentCheckinAsync(ResolveStudentId(HocvienID), LichhocID);
            return StatusCode(result.statusCode, result);
        }
    }
}
