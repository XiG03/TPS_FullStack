using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ITopicsRepository
    {
        public Task<List<Chuyende>> GetChuyendesAsync();
    }

}

