using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TPS_FullStack.Server.Modules.Admin
{
    [Route("api/v1/exams")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ExamController : ControllerBase
    {
        private readonly IFinalExamServices _finalExamServices;
        private readonly IExamService _examService;
        public ExamController(IExamService examService)
        {
            _examService = examService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllExams()
        {
            var result = await _examService.ExamGetAllAsync();
            return StatusCode(result.statusCode, result);
        }

        [HttpGet("{MaID}")]
        public async Task<IActionResult> GetExamDetail(string MaID)
        {
            var result = await _examService.ExamGetDetailAsync(MaID);
            return StatusCode(result.statusCode, result);
        }

        // [HttpPut("{MaID}")]
        // public async Task<IActionResult> UpdateExam(string MaID, ExamUpdateRequest updateRequest)
        // {
        //     updateRequest.MaID = MaID;
        //     var result = await _examService.ExamUpdateAsync(updateRequest);
        //     return StatusCode(result.statusCode, result);
        // }

        [HttpPost("{KhoahocID}/{HocvienID}")]
        public async Task<IActionResult> CreateExam(string KhoahocID, string HocvienID)
        {
            var createRequest = new ExamCreateRequest();
            createRequest.KhoahocID = KhoahocID;
            createRequest.HocvienID = HocvienID;
            var result = await _examService.ExamCreateAsync(createRequest);
            return StatusCode(result.statusCode, result);
        }
    }
}
