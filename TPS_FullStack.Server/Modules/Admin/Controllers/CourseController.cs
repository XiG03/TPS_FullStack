using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TPS_FullStack.Server.Modules.Admin
{
    [Route("api/v1/course")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _courseService.GetAllCoursesAsync();
            return StatusCode(result.statusCode, result);
        }

        [HttpGet("{KhoahocID}")]
        public async Task<IActionResult> GetDetail(string KhoahocID)
        {
            var result = await _courseService.GetCourseDetailAsync(KhoahocID);
            return StatusCode(result.statusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseCreateRequest createRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var result = await _courseService.CreateCourseAsync(createRequest);
            return StatusCode(result.statusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CourseUpdateRequest updateRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _courseService.UpdateCourseAsync(updateRequest);
            return StatusCode(result.statusCode, result);
        }

        [HttpDelete("{KhoahocID}")]
        public async Task<IActionResult> Delete(string KhoahocID)
        {
            var result = await _courseService.DeleteCourseAsync(KhoahocID);
            return StatusCode(result.statusCode, result);
        }
    }
}