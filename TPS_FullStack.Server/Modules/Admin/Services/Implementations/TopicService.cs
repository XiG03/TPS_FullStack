using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TopicService : ITopicService
    {
        private readonly IConfiguration _configuration;
        private readonly ITopicRepository _topicRepository;
        private readonly ITopicDocumentRepository _documentRepository;
        private readonly ITopicQuestionRepository _questionRepository;
        private readonly ITopicAnswerRepository _answerRepository;
        public TopicService(IConfiguration configuration, ITopicRepository topicRepository,
                            ITopicDocumentRepository documentRepository, ITopicQuestionRepository questionRepository,
        ITopicAnswerRepository answerRepository)
        {
            _configuration = configuration;
            _topicRepository = topicRepository;
            _documentRepository = documentRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
        }
        // Done
        public async Task<ServiceDefault<TopicCreateResponse>> CreateTopicAsync(TopicCreateRequest createRequest)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        //Buoc 1: Tao chuyende
                        createRequest.ChuyendeID = Guid.NewGuid().ToString();
                        await _topicRepository.CreateAsync(conn, trans, createRequest.ChuyendeID, createRequest.Ten, createRequest.Mota,
                                                            false, DateTime.UtcNow, null, null, null, null, null);

                        // Buoc 2: Tao Tai lieu
                        foreach (var doc in createRequest.Documents)
                        {
                            doc.TailieuID = Guid.NewGuid().ToString();
                            await _documentRepository.CreateAsync(conn, trans, doc.TailieuID, createRequest.ChuyendeID,
                                                                doc.Tieude, doc.Ngaytao, doc.Loaitailieu, doc.Kichthuoc);
                        }

                        // Buoc 3: Tao cau hoi
                        // 3.1: Tao cau hoi
                        foreach (var ques in createRequest.Questions)
                        {
                            ques.CauhoiID = Guid.NewGuid().ToString();
                            await _questionRepository.CreateAsync(conn, trans, ques.CauhoiID, createRequest.ChuyendeID, ques.Ten);

                            // 3.2: Tao dap an
                            foreach (var ans in ques.Answers)
                            {
                                ans.DapanID = Guid.NewGuid().ToString();
                                await _answerRepository.CreateAsync(conn, trans, ans.DapanID, createRequest.ChuyendeID, ques.CauhoiID, ans.Ten, ans.Dung);
                            }
                        }
                        await trans.CommitAsync();
                        return new ServiceDefault<TopicCreateResponse>
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Tao chuyen de thanh cong",
                            Data = new TopicCreateResponse
                            {
                                ChuyendeID = createRequest.ChuyendeID
                            }
                        };
                    }
                    catch (Exception ex)
                    {
                        await trans.RollbackAsync();
                        return new ServiceDefault<TopicCreateResponse>
                        {
                            statusCode = StatusCodes.Status500InternalServerError,
                            Message = "Tao chuyen de khong thanh cong",
                            Data = null
                        };
                    }
                }
            }
            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<string>> DeletedTopicAsync(string MaID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        await _topicRepository.DeleteAsync(conn, trans, MaID, DateTime.UtcNow, null);
                        await trans.CommitAsync();
                        return new ServiceDefault<string>
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Xoa chuyende thanh cong",
                            Data = MaID
                        };
                    }
                    catch (Exception ex)
                    {
                        await trans.RollbackAsync();
                        return new ServiceDefault<string>
                        {
                            statusCode = StatusCodes.Status500InternalServerError,
                            Message = "Xoa chuyen de khong thanh cong",
                            Data = null
                        };
                    }
                }
            }
            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<TopicDetailResponse>> GetTopicDetailAsync(string MaID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                var topic = await _topicRepository.GetByIdAsync(conn, MaID);
                if (topic == null)
                {
                    return new ServiceDefault<TopicDetailResponse>
                    {
                        statusCode = StatusCodes.Status404NotFound,
                        Message = "Khong tim thay chuyen de",
                        Data = null
                    };
                }

                var result = new TopicDetailResponse
                {
                    ChuyendeID = topic.MaID,
                    Ten = topic.Ten,
                    Mota = topic.Mota,
                    Documents = new List<TopicDocumentDetailResponse>(),
                    Questions = new List<TopicQuestionDetailResponse>()
                };

                // Vi khong the dung LINQ, ta lay toan bo roi dung foreach va if de loc ra du lieu thuoc ve ChuyendeID
                var allDocs = await _documentRepository.GetAllAsync(conn);
                foreach (var doc in allDocs)
                {
                    if (doc.ChuyendeID == MaID)
                    {
                        var docRes = new TopicDocumentDetailResponse();
                        docRes.TailieuID = doc.MaID;
                        docRes.Tieude = doc.Tieude;
                        docRes.Ngaytao = doc.Ngaytao;
                        docRes.Loaitailieu = doc.Loaitailieu;
                        docRes.Kichthuoc = doc.Kichthuoc; // Ép kiểu từ decimal sang long tuỳ thiết kế ban đầu

                        result.Documents.Add(docRes);
                    }
                }

                var allQuestions = await _questionRepository.GetAllAsync(conn);
                var allAnswers = await _answerRepository.GetAllAsync(conn);

                foreach (var q in allQuestions)
                {
                    if (q.ChuyendeID == MaID)
                    {
                        var qRes = new TopicQuestionDetailResponse();
                        qRes.CauhoiID = q.MaID;
                        qRes.Ten = q.Ten;
                        qRes.Answers = new List<TopicAnswerDetailResponse>();

                        // Lap cac dap an xem cai nao thuoc ve CauHoiID hien tai
                        foreach (var a in allAnswers)
                        {
                            if (a.Chuyende_CauhoiID == q.MaID)
                            {
                                var aRes = new TopicAnswerDetailResponse();
                                aRes.DapanID = a.MaID;
                                aRes.Ten = a.Ten;
                                aRes.Dung = a.Dung;
                                qRes.Answers.Add(aRes);
                            }
                        }

                        result.Questions.Add(qRes);
                    }
                }

                return new ServiceDefault<TopicDetailResponse>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Thanh cong",
                    Data = result
                };
            }
        }

        public async Task<ServiceDefault<List<TopicResponse>>> GetTopicsAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var topics = await _topicRepository.GetAllAsync(conn);

                var resultList = new List<TopicResponse>();

                // Khong su dung LINQ, dung vong lap foreach de map data
                foreach (var topic in topics)
                {
                    var responseModel = new TopicResponse();
                    responseModel.ChuyendeID = topic.MaID;
                    responseModel.Ten = topic.Ten;
                    responseModel.Mota = topic.Mota;

                    resultList.Add(responseModel);
                }

                return new ServiceDefault<List<TopicResponse>>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Thanh cong",
                    Data = resultList
                };
            }
        }
        public async Task<ServiceDefault<TopicUpdateResponse>> UpdateTopicAsync(TopicUpdateRequest updateRequest)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                // 1. Kiem tra ton tai
                var existTopic = await _topicRepository.GetByIdAsync(conn, updateRequest.ChuyendeID);
                if (existTopic == null)
                {
                    return new ServiceDefault<TopicUpdateResponse>
                    {
                        statusCode = StatusCodes.Status404NotFound,
                        Message = "Khong tim thay chuyen de",
                        Data = null
                    };
                }

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 2. Cap nhat Topic
                        await _topicRepository.UpdateAsync(conn, trans, updateRequest.ChuyendeID, updateRequest.Ten, updateRequest.Mota, DateTime.UtcNow, "Admin");

                        // --- Xu ly Documents ---
                        var allDocs = await _documentRepository.GetAllAsync(conn);
                        var existingDocs = new List<TopicDocumentModel>();
                        foreach (var d in allDocs)
                        {
                            if (d.ChuyendeID == updateRequest.ChuyendeID) existingDocs.Add(d);
                        }

                        // Xoa cac Document bi mat (Co trong db nhung khong co trong request)
                        foreach (var oldDoc in existingDocs)
                        {
                            bool isFound = false;
                            foreach (var newDoc in updateRequest.Documents)
                            {
                                if (newDoc.TailieuID == oldDoc.MaID)
                                {
                                    isFound = true;
                                    break;
                                }
                            }
                            if (!isFound)
                            {
                                await _documentRepository.DeleteAsync(conn, trans, oldDoc.MaID, updateRequest.ChuyendeID);
                            }
                        }

                        // Them hoac Sua Document
                        foreach (var doc in updateRequest.Documents)
                        {
                            if (string.IsNullOrEmpty(doc.TailieuID)) // Them moi
                            {
                                doc.TailieuID = Guid.NewGuid().ToString();
                                await _documentRepository.CreateAsync(conn, trans, doc.TailieuID, updateRequest.ChuyendeID, doc.Tieude, doc.Ngaytao, doc.Loaitailieu, doc.Kichthuoc);
                            }
                            else // Sua
                            {
                                await _documentRepository.UpdateAsync(conn, trans, doc.TailieuID, updateRequest.ChuyendeID, doc.Tieude, doc.Ngaytao, doc.Loaitailieu, doc.Kichthuoc);
                            }
                        }


                        // --- Xu ly Questions & Answers ---
                        var allQuestions = await _questionRepository.GetAllAsync(conn);
                        var allAnswers = await _answerRepository.GetAllAsync(conn);

                        var existingQuestions = new List<QuestionTopicModel>();
                        foreach (var q in allQuestions) if (q.ChuyendeID == updateRequest.ChuyendeID) existingQuestions.Add(q);

                        var existingAnswers = new List<TopicAnswerModel>();
                        foreach (var a in allAnswers) if (a.ChuyendeID == updateRequest.ChuyendeID) existingAnswers.Add(a);

                        // Xoa cau hoi bi mat
                        foreach (var oldQ in existingQuestions)
                        {
                            bool isFound = false;
                            foreach (var newQ in updateRequest.Questions)
                            {
                                if (newQ.CauhoiID == oldQ.MaID)
                                {
                                    isFound = true;
                                    break;
                                }
                            }

                            if (!isFound)
                            {
                                await _questionRepository.DeleteAsync(conn, trans, oldQ.MaID, updateRequest.ChuyendeID);
                                // Xoa luon cac dap an thuoc ve cau hoi bi xoa do
                                foreach (var oldA in existingAnswers)
                                {
                                    if (oldA.Chuyende_CauhoiID == oldQ.MaID)
                                    {
                                        await _answerRepository.DeleteAsync(conn, trans, oldA.MaID, updateRequest.ChuyendeID, oldQ.MaID);
                                    }
                                }
                            }
                        }

                        // Them hoac Sua cau hoi va dap an
                        foreach (var q in updateRequest.Questions)
                        {
                            if (string.IsNullOrEmpty(q.CauhoiID))
                            {
                                q.CauhoiID = Guid.NewGuid().ToString();
                                await _questionRepository.CreateAsync(conn, trans, q.CauhoiID, updateRequest.ChuyendeID, q.Ten);
                            }
                            else
                            {
                                await _questionRepository.UpdateAsync(conn, trans, q.CauhoiID, updateRequest.ChuyendeID, q.Ten);
                            }

                            // Lay cac dap an cu cua cau hoi hien tai
                            var oldAnswersForThisQ = new List<TopicAnswerModel>();
                            foreach (var a in existingAnswers)
                            {
                                if (a.Chuyende_CauhoiID == q.CauhoiID) oldAnswersForThisQ.Add(a);
                            }

                            // Xoa dap an bi mat
                            foreach (var oldA in oldAnswersForThisQ)
                            {
                                bool isAFound = false;
                                foreach (var newA in q.Answers)
                                {
                                    if (newA.DapanID == oldA.MaID)
                                    {
                                        isAFound = true;
                                        break;
                                    }
                                }
                                if (!isAFound)
                                {
                                    await _answerRepository.DeleteAsync(conn, trans, oldA.MaID, updateRequest.ChuyendeID, q.CauhoiID);
                                }
                            }

                            // Them hoac Sua dap an
                            foreach (var a in q.Answers)
                            {
                                if (string.IsNullOrEmpty(a.DapanID))
                                {
                                    a.DapanID = Guid.NewGuid().ToString();
                                    await _answerRepository.CreateAsync(conn, trans, a.DapanID, updateRequest.ChuyendeID, q.CauhoiID, a.Ten, a.Dung);
                                }
                                else
                                {
                                    await _answerRepository.UpdateAsync(conn, trans, a.DapanID, updateRequest.ChuyendeID, q.CauhoiID, a.Ten, a.Dung);
                                }
                            }
                        }

                        await trans.CommitAsync();

                        return new ServiceDefault<TopicUpdateResponse>
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Cap nhat chuyen de thanh cong",
                            Data = new TopicUpdateResponse { ChuyendeID = updateRequest.ChuyendeID }
                        };
                    }
                    catch (Exception ex)
                    {
                        await trans.RollbackAsync();
                        return new ServiceDefault<TopicUpdateResponse>
                        {
                            statusCode = StatusCodes.Status500InternalServerError,
                            Message = "Cap nhat chuyen de khong thanh cong: " + ex.Message,
                            Data = null
                        };
                    }
                }
            }
        }
    }

}

