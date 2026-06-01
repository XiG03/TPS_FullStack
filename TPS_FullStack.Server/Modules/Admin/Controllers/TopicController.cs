using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TPS_FullStack.Server.Modules.Admin
{
    [Route("api/v1/topic")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;
        
        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        // Lấy danh sách tất cả chuyên đề
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _topicService.GetTopicsAsync();
            return StatusCode(result.statusCode, result);
        }

        // Lấy chi tiết một chuyên đề theo ID
        [HttpGet("{MaID}")]
        public async Task<IActionResult> GetDetail(string MaID)
        {
            var result = await _topicService.GetTopicDetailAsync(MaID);
            return StatusCode(result.statusCode, result);
        }

        // Tạo mới chuyên đề
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] TopicCreateRequest createRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _topicService.CreateTopicAsync(createRequest);
            return StatusCode(result.statusCode, result);
        }

        // Cập nhật chuyên đề
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] TopicUpdateRequest updateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _topicService.UpdateTopicAsync(updateRequest);
            return StatusCode(result.statusCode, result);
        }

        // Xóa chuyên đề
        [HttpGet("document/{tailieuID}/download")]
        public async Task<IActionResult> DownloadDocument(string tailieuID)
        {
            var result = await _topicService.GetDocumentDownloadAsync(tailieuID);
            if (result.Data == null)
            {
                return StatusCode(result.statusCode, result);
            }

            return PhysicalFile(result.Data.FilePath, result.Data.ContentType, result.Data.FileName);
        }

        [HttpDelete("{MaID}")]
        public async Task<IActionResult> Delete(string MaID)
        {
            var result = await _topicService.DeletedTopicAsync(MaID);
            return StatusCode(result.statusCode, result);
        }
    }
}
