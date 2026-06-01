using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TPS_FullStack.Server.Modules.Admin
{
    [Route("api/v1/admin/exams")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ExamController : ControllerBase
    {
        private readonly IFinalExamServices _finalExamServices;
        public ExamController(IFinalExamServices finalExamServices)
        {
            _finalExamServices = finalExamServices;
        }
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateFinalExam(List<string> HocvienIDs, string KhoahocID, decimal Socauhoi)
        {
            var result = await _finalExamServices.GenerateFinalExamAsync(HocvienIDs, KhoahocID, Socauhoi);
            return StatusCode(result.statusCode, result);
        }
        [HttpGet("lists")]
        public async Task<IActionResult> GetFinalExamLists()
        {
            var result = await _finalExamServices.GetFinalExamListsAsync();
            return StatusCode(result.statusCode, result);
        }
        [HttpGet("lists/{KhoahocID}")]
        public async Task<IActionResult> GetFinalExamLists(string KhoahocID)
        {
            var result = await _finalExamServices.GetFinalExamListsAsync(KhoahocID);
            return StatusCode(result.statusCode, result);
        }
        [HttpGet("detail/{MaID}")]
        public async Task<IActionResult> GetFinalExamDetail(string MaID)
        {
            var result = await _finalExamServices.GetFinalExamDetailAsync(MaID);
            return StatusCode(result.statusCode, result);
        }
        [HttpPut("update-score")]
        public async Task<IActionResult> UpdateFinalExamScore(finalExamUpdate finalExamUpdate)
        {
            var result = await _finalExamServices.UpdateFinalExamScoreAsync(finalExamUpdate);
            return StatusCode(result.statusCode, result);
        }

    }
}
