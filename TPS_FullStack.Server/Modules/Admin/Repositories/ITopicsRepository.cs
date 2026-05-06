using Microsoft.Identity.Client;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ITopicsRepository
    {
        public Task<List<Chuyende>> GetChuyendesAsync();
        public Task<Chuyende_ChitietDto> GetTopicByID(string MaID);
    }

}

