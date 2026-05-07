
namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ITopicsService
    {
        public Task<List<ChuyendeDto>> GetAllTopicAsync();
        public Task<ServiceDefault<Chuyende_ChitietDto>> GetTopicDetail(string MaID);
        public Task<ServiceDefault<bool>> CreateTopic (Chuyende_ChitietDto chuyendeDto);

    }
}


