using System.Reflection;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TopicsService : ITopicsService
    {
        private readonly ITopicsRepository _topicRepository;
        public TopicsService(ITopicsRepository topicsRepository)
        {
            _topicRepository = topicsRepository;
        }

        public async Task<bool> CreateTopic(ChuyendeDto_CU chuyendeDto)
        {   
            
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

