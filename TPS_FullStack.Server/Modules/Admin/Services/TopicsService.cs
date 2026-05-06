namespace TPS_FullStack.Server.Modules.Admin
{
    public class TopicsService : ITopicsService
    {
        private readonly ITopicsRepository _topicRepository;
        public TopicsService(ITopicsRepository topicsRepository)
        {
            _topicRepository = topicsRepository;
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

        public Task<ServiceDefault<Chuyende_ChitietDto>> GetTopicDetail(string MaID)
        {
            throw new NotImplementedException();
        }
    }

}

