using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPS_FullStack.Server.Modules.Admin;

namespace TPS_FullStack.Server.Modules.Admin
{
    [Route("api/admin/course")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICoursesService _courseService;
        public CoursesController(ICoursesService coursesService)
        {
            _courseService = coursesService;
        }

        [HttpGet]
        public async Task<IActionResult> CourseGetAll()
        {
            var result = await _courseService.CourseGetAllAsync();

            if(result == null)
            {
                return BadRequest("Can not get Courses");
            }
            return Ok(result);
        }

        [HttpGet("{MaID}")]
        public async Task<IActionResult> CourseGetByID(string MaID)
        {
            var result = await _courseService.CourseGetByIdAsync(MaID);
            if(result == null)
            {
                return BadRequest("Can not find course");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CourseInsert(CourseCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _courseService.CourseInsertAsync(createDto);
            if(result == null)
            {
                return BadRequest("Can not insert");
            }
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> CourseUpdate(CourseUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _courseService.CourseUpdateAsync(updateDto);
            if(result == null)
            {
                return BadRequest("Can not update this course");
            }
            return Ok(updateDto);
        }

        [HttpDelete("{MaId}")]
        public async Task<IActionResult> CourseDeleteByID(string MaID)
        {
            var result = await _courseService.CourseDeleteByIdAsync(MaID);
            if (!result)
            {
                return BadRequest("Can not delete this course");
            }
            return Ok();
        }


    }
}
