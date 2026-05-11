using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace TPS_FullStack.Server.Modules.Admin
{
    [Route("api/admin/student")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentsService _studentService;
        public StudentsController (IStudentsService studentsService)
        {
            _studentService = studentsService;
        }
        [HttpGet("students")]
        public async Task<IActionResult> StudentsGetAll()
        {
            var result = await _studentService.StudentGetAllsAsync();
            if(result == null)
            {
                return BadRequest("Can not get students");
            }
            return Ok(result);
        }
        [HttpGet("{MaId}")]
        public async Task<IActionResult> StudentGetById(string MaID)
        {
            var result = await _studentService.GetStudentByIDAsync(MaID);
            if(result == null)
            {
                return BadRequest("Can not find student");
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> StudentInsert(StudentCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Can not insert student");
            }
            var result = await _studentService.StudentInsertAsync(createDto);
            if(result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> StudentUpdate(StudentUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Can not update student");
            }
            var result = await _studentService.StudentUpdateAsync(updateDto);
            if(result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        
        [HttpDelete("{MaId}")]
        public async Task<IActionResult> StudentDeleteByID(string MaID)
        {
            var result = await _studentService.StudentDeleteAsync(MaID);
            if (result)
            {
                return Ok("Deleted Student");
            }
            return BadRequest();
        }
    }
}
