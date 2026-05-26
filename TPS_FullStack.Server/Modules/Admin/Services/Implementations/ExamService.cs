using System.Data;
using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class ExamService : IExamService
    {
        private readonly IConfiguration _configuration;
        private readonly ICourseTopicRepository _courseTopicRepository;
        private readonly ITopicQuestionRepository _topicQuestionRepository;
        private readonly ITopicAnswerRepository _topicAnswerRepository;
        public ExamService(IConfiguration configuration, ICourseTopicRepository courseTopicRepository,
                        ITopicQuestionRepository topicQuestionRepository, ITopicAnswerRepository topicAnswerRepository)
        {
            _configuration = configuration;
            _courseTopicRepository = courseTopicRepository;
            _topicQuestionRepository = topicQuestionRepository;
            _topicAnswerRepository = topicAnswerRepository;
        }
        public async Task<ServiceDefault<ExamCreateResponse>> ExamCreateAsync(ExamCreateRequest createRequest)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using(var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                // Lay danh sach topic
                var topics = await _courseTopicRepository.GetByCourseIDAsync(conn,createRequest.KhoahocID);
                if(topics.Count() == 0)
                {
                    return new ServiceDefault<ExamCreateResponse>
                    {
                        statusCode = StatusCodes.Status400BadRequest,
                        Message = "Khong co danh sach chuyen de",
                        Data = null
                    };
                }
                // Lay danh sach cau hoi va cau tra loi
                var list = new List<Question>();
                foreach(var topic in topics)
                {
                    var questions = await _topicQuestionRepository.GetByTopicIdAsync(conn, topic.ChuyendeID);
                    foreach(var ques in questions)
                    {
                        list.Add(new Question
                        {
                            ChuyendeID = ques.ChuyendeID,
                            CauhoiID = ques.MaID,
                            Ten = ques.Ten,
                            questionAnswers = new List<QuestionAnswer>()
                        });
                    }
                    var answers = await _topicAnswerRepository.GetByTopicIdAsync(conn,topic.ChuyendeID);
                    foreach(var ques in list)
                    {
                        foreach(var ans in answers)
                        {
                            if(ans.Chuyende_CauhoiID == ques.CauhoiID)
                            {
                                ques.questionAnswers.Add(new QuestionAnswer
                                {
                                    CauhoiID = ans.Chuyende_CauhoiID,
                                    Ten = ans.Ten,
                                    Dung = ans.Dung
                                });
                            }
                        }
                    }   
                }
                Random rnd = new Random();
            }
            throw new NotImplementedException();
        }

        public Task<ServiceDefault<bool>> ExamDeleteAsync(string? MaID)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceDefault<List<ExamResponse>>> ExamGetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceDefault<ExamDetailResponse>> ExamGetDetailAsync(string? MaID)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceDefault<ExamUpdateResponse>> ExamUpdateAsync(ExamUpdateRequest updateRequest)
        {
            throw new NotImplementedException();
        }
    }

}

