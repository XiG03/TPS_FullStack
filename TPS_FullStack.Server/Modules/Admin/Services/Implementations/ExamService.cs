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
        private readonly ICourseStudentRepository _courseStudentRepository;
        private readonly IExamQuestionRepository _examQuestionRepository;
        private readonly IExamRepository _examRepository;
        private readonly IExamAnswersRepository _examAnswersRepository;
        public ExamService(IConfiguration configuration, ICourseTopicRepository courseTopicRepository,
                        ITopicQuestionRepository topicQuestionRepository, ITopicAnswerRepository topicAnswerRepository,
                        ICourseStudentRepository courseStudentRepository, IExamQuestionRepository examQuestionRepository, IExamRepository examRepository, IExamAnswersRepository examAnswersRepository)
        {
            _configuration = configuration;
            _courseTopicRepository = courseTopicRepository;
            _topicQuestionRepository = topicQuestionRepository;
            _topicAnswerRepository = topicAnswerRepository;
            _courseStudentRepository = courseStudentRepository;
            _examQuestionRepository = examQuestionRepository;
            _examRepository = examRepository;
            _examAnswersRepository = examAnswersRepository;
        }
        public async Task<ServiceDefault<ExamCreateResponse>> ExamCreateAsync(ExamCreateRequest createRequest)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using(var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                if(await _courseStudentRepository.CheckStudentByCourseIDAsync(conn,createRequest.KhoahocID,createRequest.HocvienID) == null)
                {
                    return new ServiceDefault<ExamCreateResponse>
                    {
                        statusCode = StatusCodes.Status400BadRequest,
                        Message = "Hoc vien khong co trong khoa hoc",
                        Data = null
                    };
                }
                var questions = await _examQuestionRepository.GetByCourseIDAsync(conn, createRequest.KhoahocID);

                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var examID = Guid.NewGuid().ToString();
                        await _examRepository.CreateAsync(conn, transaction, examID, createRequest.KhoahocID, createRequest.HocvienID, null, null, DateTime.UtcNow, "Admin", null, null, null, null);
                        foreach(var question in questions)
                        {
                            var quesID = Guid.NewGuid().ToString();
                            await _examQuestionRepository.CreateAsync(conn, transaction, quesID, examID, createRequest.KhoahocID, question.ChuyendeID, question.CauhoiID, question.Ten);
                            foreach(var ans in question.questionAnswers)
                            {
                                var answerID = Guid.NewGuid().ToString();
                                await _examAnswersRepository.CreateAsync(conn, transaction, answerID, examID, quesID, ans.Ten, ans.Dung, null);
                            }
                        }
                        transaction.Commit();
                        return new ServiceDefault<ExamCreateResponse>
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Tao de thi thanh cong",
                            Data = new ExamCreateResponse
                            {
                            }
                        };
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new ServiceDefault<ExamCreateResponse>
                        {
                            statusCode = StatusCodes.Status500InternalServerError,
                            Message = "Loi server: " + ex.Message,
                            Data = null
                        };
                    }
                }




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

