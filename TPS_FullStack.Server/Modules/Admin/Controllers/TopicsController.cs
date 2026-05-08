using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    // [Route("api/[controller]")]
    [Route("api/admin/topic")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicsService _topicService;

        public TopicsController(ITopicsService topicService)
        {
            _topicService = topicService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllTopics()
        {
            var data = await _topicService.GetAllTopicAsync();
            if(data == null)
            {
                return BadRequest();
            }
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopic(Chuyende_ChitietDto chuyendedto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _topicService.CreateTopic(chuyendedto);
            // if (!result.Success)
            // {
            //     return BadRequest(result);
            // }
            // return Created(result.Message,result);
            if(result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpDelete("{MaID}")]
        public async Task<IActionResult> DeleteTopicAsync(string MaID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _topicService.DeleteTopicAsync(MaID);

            if (!result)
            {
                return BadRequest("Can not delete this topic ID" + MaID);
            }

            return Ok("Deleted topic" + MaID);
        }
        [HttpGet("{MaID}")]
        public async Task<IActionResult> GetTopicByID(string MaID)
        {
            var result = await _topicService.GetTopicByID(MaID);

            if(result == null)
            {
                return BadRequest("Can not find the topic detail");
            }
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateTopic(TopicUpdateDto chuyendeDto)
        {
            var result = await _topicService.UpdateTopicAsync(chuyendeDto);

            if(result == null)
            {
                return BadRequest("Can not update topic");
            }
            return Ok(result);
        }

    }
}
