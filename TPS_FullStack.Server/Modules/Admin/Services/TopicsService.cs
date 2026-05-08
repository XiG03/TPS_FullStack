using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Identity.Client;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TopicsService : ITopicsService
    {
        private readonly ITopicsRepository _topicRepository;
        public TopicsService(ITopicsRepository topicsRepository)
        {
            _topicRepository = topicsRepository;
        }

        public async Task<Chuyende_ChitietDto> CreateTopic(Chuyende_ChitietDto chuyendeDto)
        {   
            var result = await _topicRepository.CreateTopicAsync(chuyendeDto);
            if (!result)
            {
                return null;
            }

            return chuyendeDto;


            // return new ServiceDefault<bool>
            // {
            //     Success = true,
            //     Message = "Complete create topic"
            // };

            throw new NotImplementedException();
        }

        public async Task<bool> DeleteTopicAsync(string MaID)
        {
            var result = await _topicRepository.DeleteTopicByIDAsync(MaID);

            if(result == false)
            {
                return false;
            }

            if(result == true)
            {
                return true;
            }

            throw new NotImplementedException();
        }
        
        public async Task<List<TopicGetAllDto>> GetAllTopicAsync() // Done
        {
            var data = await _topicRepository.GetTopicsAsync();

            if(data != null)
            {
                return data;
            }
            return null;
            throw new NotImplementedException();
        }

        public async Task<TopicDetailDto> GetTopicByID(string MaID)
        {
            var data = await _topicRepository.GetTopicByID(MaID);

            if(data != null)
            {
                return data;
            }
            return null;
            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<Chuyende_ChitietDto>> GetTopicDetail(string MaID)
        {
            var result = await _topicRepository.GetTopicByID(MaID);

            if(result == null)
            {
                return new ServiceDefault<Chuyende_ChitietDto>
                {
                    Success = false,
                    Message = "Can not found topic detail"
                };
            }

            return new ServiceDefault<Chuyende_ChitietDto>
            {
                Success = true,
                Message = "Complete read topic detail of " + MaID,
            };

            throw new NotImplementedException();
        }

        public async Task<TopicUpdateDto> UpdateTopicAsync(TopicUpdateDto chuyendeDto)
        {
            var result = await _topicRepository.UpdateTopicAsync(chuyendeDto);
            if (!result)
            {
                return null;
            }
            return chuyendeDto;
            throw new NotImplementedException();
        }
    }

}

