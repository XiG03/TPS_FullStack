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

        public async Task<ServiceDefault<bool>> CreateTopic(Chuyende_ChitietDto chuyendeDto)
        {   
            var result = await _topicRepository.CreateTopicAsync(chuyendeDto);
            if (!result)
            {
                return new ServiceDefault<bool>
                {
                    Success = false,
                    Message = "Can not create new topic"
                };
            }


            return new ServiceDefault<bool>
            {
                Success = true,
                Message = "Complete create topic"
            };

            throw new NotImplementedException();
        }

        public async Task<List<ChuyendeDto>> GetAllTopicAsync()
        {
            var data = await _topicRepository.GetChuyendesAsync();

            return data.Select(x => new ChuyendeDto
            {
                MaID = x.MaID,
                Ten = x.Ten,
                Mota = x.Mota
            }).ToList();


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
                Data = result
            };

            throw new NotImplementedException();
        }
    }

}

