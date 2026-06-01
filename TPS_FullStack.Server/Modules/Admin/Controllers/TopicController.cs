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
        public async Task<IActionResult> Create([FromBody] TopicCreateRequest createRequest)
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
        public async Task<IActionResult> Update([FromBody] TopicUpdateRequest updateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _topicService.UpdateTopicAsync(updateRequest);
            return StatusCode(result.statusCode, result);
        }

        // Xóa chuyên đề
        [HttpDelete("{MaID}")]
        public async Task<IActionResult> Delete(string MaID)
        {
            var result = await _topicService.DeletedTopicAsync(MaID);
            return StatusCode(result.statusCode, result);
        }
    }
}
