namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ITopicService
    {
        public Task<ServiceDefault<List<TopicResponse>>> GetTopicsAsync();
        public Task<ServiceDefault<TopicDetailResponse>> GetTopicDetailAsync(string MaID);
        public Task<ServiceDefault<TopicCreateResponse>> CreateTopicAsync(TopicCreateRequest createRequest);
        public Task<ServiceDefault<TopicUpdateResponse>> UpdateTopicAsync(TopicUpdateRequest updateRequest);
        public Task<ServiceDefault<string>> DeletedTopicAsync(string MaID);
    }

}

