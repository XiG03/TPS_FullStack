using System.Diagnostics.Eventing.Reader;
using Microsoft.Identity.Client;
using Microsoft.JSInterop.Infrastructure;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ITopicsRepository
    {
        public Task<List<TopicGetAllDto>> GetTopicsAsync();
        public Task<TopicDetailDto> GetTopicByID(string MaID);
        public Task<bool> CreateTopicAsync (Chuyende_ChitietDto chuyendeDto);
        public Task<bool> DeleteTopicByIDAsync (string MaID);
        public Task<bool> UpdateTopicAsync(TopicUpdateDto chuyendeDto);
        
    }

}

