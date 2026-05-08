
using Microsoft.AspNetCore.Mvc;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ITopicsService
    {
        public Task<List<TopicGetAllDto>> GetAllTopicAsync();
        public Task<ServiceDefault<Chuyende_ChitietDto>> GetTopicDetail(string MaID);
        public Task<Chuyende_ChitietDto> CreateTopic (Chuyende_ChitietDto chuyendeDto);
        public Task<bool> DeleteTopicAsync(string MaID);
        public Task<TopicDetailDto> GetTopicByID (string MaID);
        public Task<TopicUpdateDto> UpdateTopicAsync (TopicUpdateDto chuyendeDto);

    }
}


