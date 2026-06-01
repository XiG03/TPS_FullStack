using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _teacherService.TeacherGetAllAsync();
            return StatusCode(result.statusCode, result);
        }


        [Authorize(Roles = "Giangvien, Admin")]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(teacherId))
            {
                return Unauthorized(new ServiceDefault<TeacherDetailResponse>
                {
                    statusCode = StatusCodes.Status401Unauthorized,
                    Message = "Không xác định được giảng viên hiện tại",
                    Data = null
                });
            }

            var result = await _teacherService.TeacherGetByIDAsync(teacherId);
            return StatusCode(result.statusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{MaID}")]
        public async Task<IActionResult> GetDetail(string MaID)
        {
            var result = await _teacherService.TeacherGetByIDAsync(MaID);
            return StatusCode(result.statusCode, result);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
        
        [Authorize(Roles = "Admin")]
        [HttpDelete("{MaID}")]
        public async Task<IActionResult> Delete(string MaID)
        {
            var result = await _teacherService.TeacherDeleteAsync(MaID);
            return StatusCode(result.statusCode, result);
        }

        [Authorize(Roles = "Giangvien, Admin")]
        [HttpGet("me/schedules")]
        public async Task<IActionResult> GetSchedules(string? GiangvienID)
        {
            var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _teacherService.TeacherGetScheduleByIDAsync(teacherId);
            return StatusCode(result.statusCode, result);
        }
        [Authorize(Roles = "Giangvien, Admin")]
        [HttpGet("me/schedule/{LichhocID}")]
        public async Task<IActionResult> GetScheduleDetail(string? GiangvienID, string? LichhocID)
        {
            var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _teacherService.TeacherGetScheduleDetailAsync(teacherId, LichhocID);
            return StatusCode(result.statusCode, result);
        }
        [Authorize(Roles = "Giangvien, Admin")]
        [HttpPost("me/schedule/{LichhocID}/checkin")]
        public async Task<IActionResult> Checkin(string? GiangvienID, string? LichhocID)
        {
            var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _teacherService.TeacherCheckinAsync(teacherId, LichhocID);
            return StatusCode(result.statusCode, result);
        }
        [Authorize(Roles = "Giangvien, Admin")]
        [HttpPost("me/schedule/{LichhocID}/checkout")]
        public async Task<IActionResult> Checkout(string? GiangvienID, string? LichhocID)
        {
            var teacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _teacherService.TeacherCheckoutAsync(teacherId, LichhocID);
            return StatusCode(result.statusCode, result);
        }
    }
}
