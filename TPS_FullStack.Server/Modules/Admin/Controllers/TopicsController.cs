using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicsService _topicService;

        public TopicsController(ITopicsService topicService)
        {
            _topicService = topicService;
        }

        [HttpGet(Name = "GetAllTopics")]
        public async Task<IActionResult> GetAllTopics()
        {
            var data = await _topicService.GetAllTopicAsync();
            return Ok(data);
        }

        [HttpPost(Name ="Topic")]
        public async Task<IActionResult> CreateTopic(Chuyende_ChitietDto chuyendedto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _topicService.CreateTopic(chuyendedto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Created(result.Message,result);
        }

    }
}
