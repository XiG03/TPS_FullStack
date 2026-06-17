using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TPS_FullStack.Server.Modules.Admin
{
    [Route("api/v1/exams")]
    [ApiController]

    public class ExamController : ControllerBase
    {
        private readonly IFinalExamServices _finalExamServices;
        private readonly IExamService _examService;
        public ExamController(IExamService examService)
        {
            _examService = examService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllExams()
        {
            var result = await _examService.ExamGetAllAsync();
            return StatusCode(result.statusCode, result);
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost("{KhoahocID}/{HocvienID}")]
        public async Task<IActionResult> CreateExam(string KhoahocID, string HocvienID)
        {
            var createRequest = new ExamCreateRequest();
            createRequest.KhoahocID = KhoahocID;
            createRequest.HocvienID = HocvienID;
            var result = await _examService.ExamCreateAsync(createRequest);
            return StatusCode(result.statusCode, result);
        }
        [Authorize(Roles = "Admin, Hocvien")]
        [HttpGet("student/{HocvienID}")]
        public async Task<IActionResult> GetExamsByStudentID(string? HocvienID)
        {
            var result = await _examService.ExamGetAllByStudentIDAsync(HocvienID);
            return StatusCode(result.statusCode, result);
        }
        [Authorize(Roles = "Admin, Hocvien")]
        [HttpGet("{HocvienID}/{BaithuhoachID}")]
        public async Task<IActionResult> GetExamDetailByStudentID(string? HocvienID, string? BaithuhoachID)
        {
            var result = await _examService.ExamGetDetailByStudentIDAsync(HocvienID, BaithuhoachID);
            return StatusCode(result.statusCode, result);
        }
        [Authorize(Roles = "Admin, Hocvien")]
        [HttpPut("{HocvienID}/{BaithuhoachID}/submit")]
        public async Task<IActionResult> SubmitExam(string? HocvienID, string? BaithuhoachID, ExamSubmitResquest submitResquest)
        {
            var result = await _examService.ExamSubmitAsync(submitResquest);
            return StatusCode(result.statusCode, result);
        }
        [Authorize(Roles = "Admin, Hocvien")]
        [HttpGet("{HocvienID}/{BaithuhoachID}/get")]
        public async Task<IActionResult> GetSubmitExam(string? HocvienID, string? BaithuhoachID)
        {
            var result = await _examService.ExamGetSubmitAsync(HocvienID, BaithuhoachID);
            return StatusCode(result.statusCode, result);
        }
    }
}
