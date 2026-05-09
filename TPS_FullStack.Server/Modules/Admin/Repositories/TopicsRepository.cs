using System.Data;
using System.IO.Pipelines;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Identity.Client;
using TPS_FullStack.Server.AppDbContext;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TopicsRepository : ITopicsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AppUser> _logger;
        private readonly ApplicationDbContext _context;
        public TopicsRepository(IConfiguration configuration,
                                ILogger<AppUser> logger,
                                ApplicationDbContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
        }

        public async Task<bool> CreateTopicAsync(Chuyende_ChitietDto chuyendeDto)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            var queryTopicCreate = @"INSERT INTO Chuyende(MaID, Ten, Mota, Khongsudung, UpdatedAt, UpdatedBy, CreatedAt, CreatedBy)
                                    VALUES (@MaID,@Ten, @Mota,0,SYSDATETIME(), NEWID(),SYSDATETIME(),NEWID())";

            var queryDocumentCreate = @"INSERT INTO Chuyende_Tailieu(MaID, ChuyendeID, Tieude, Ngaytao, Loaitailieu, Kichthuoc, Khongsudung)
                                        VALUES (@MaID,@ChuyendeID,@Tieude,SYSDATETIME(), @Loaitailieu, @Kichthuoc, 0)";

            var queryQuestionCreate = @"INSERT INTO Chuyende_Cauhoi(MaID, ChuyendeID, Ten, Diem)
                                        VALUES (@MaID, @ChuyendeID, @Ten, @Diem)";

            var queryAnswerCreate = @"INSERT INTO Chuyende_Dapan(MaId, Chuyende_CauhoiID, Ten, Dung)
                                    VALUES (@MaID, @Chuyende_CauhoiID, @Ten, @Dung)";


            chuyendeDto.MaID = Guid.NewGuid();
            #region 
            // foreach (var tailieu in chuyendeDto.Chuyende_TailieuDtos)
            // {
            //     tailieu.MaID = Guid.NewGuid();
            //     tailieu.ChuyendeID = chuyendeDto.MaID;
            // }
            // foreach (var cauhoi in chuyendeDto.Chuyende_CauhoiDtos)
            // {
            //     cauhoi.MaID = Guid.NewGuid();
            //     cauhoi.ChuyendeID = chuyendeDto.MaID;

            //     foreach (var dapan in cauhoi.Chuyende_DapanDtos)
            //     {
            //         dapan.MaID = Guid.NewGuid();
            //         dapan.Chuyende_CauhoiID = cauhoi.MaID;
            //     }
            // }
            #endregion

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    //using (var transaction = conn.BeginTransaction())
                    //{
                    try
                    {
                        // Execute 1: Them chuyen de moi
                        using (var cmd = new SqlCommand(queryTopicCreate, conn))
                        {
                            //warning
                            cmd.Parameters.AddWithValue("@MaID", chuyendeDto.MaID);
                            cmd.Parameters.AddWithValue("@Ten", chuyendeDto.Ten);
                            cmd.Parameters.AddWithValue("@Mota", chuyendeDto.Mota);

                            if (await cmd.ExecuteNonQueryAsync() <= 0)
                            {
                                return false;
                            }
                        }
                        if (chuyendeDto.Chuyende_TailieuDtos != null)
                        {
                            foreach (var tailieu in chuyendeDto.Chuyende_TailieuDtos)
                            {
                                tailieu.MaID = Guid.NewGuid();
                                tailieu.ChuyendeID = chuyendeDto.MaID;

                                using (var cmd = new SqlCommand(queryDocumentCreate, conn))
                                {
                                    cmd.Parameters.AddWithValue("@MaID", tailieu.MaID);
                                    cmd.Parameters.AddWithValue("@ChuyendeID", tailieu.ChuyendeID);
                                    cmd.Parameters.AddWithValue("@Tieude", tailieu.Tieude);
                                    cmd.Parameters.AddWithValue("@Loaitailieu", tailieu.Loaitailieu);
                                    cmd.Parameters.AddWithValue("@Kichthuoc", tailieu.Kichthuoc);

                                    if (await cmd.ExecuteNonQueryAsync() <= 0)
                                    {
                                        return false;
                                    }
                                }

                            }
                        }

                        // Execute 2: Them tai lieu cho chuyen de 
                        // Warning forweach: Neu khong them duoc thi lam sao


                        // Execute 3: Them cau hoi va dap an cho chuyen de
                        // Warning foreach
                        if (chuyendeDto.Chuyende_CauhoiDtos != null)
                        {
                            foreach (var cauhoi in chuyendeDto.Chuyende_CauhoiDtos)
                            {

                                cauhoi.MaID = Guid.NewGuid();
                                cauhoi.ChuyendeID = chuyendeDto.MaID;

                                using (var cmd = new SqlCommand(queryQuestionCreate, conn))
                                {
                                    cmd.Parameters.AddWithValue("@MaID", cauhoi.MaID);
                                    cmd.Parameters.AddWithValue("@ChuyendeID", cauhoi.ChuyendeID);
                                    cmd.Parameters.AddWithValue("@Ten", cauhoi.Ten);
                                    cmd.Parameters.AddWithValue("@Diem", cauhoi.Diem);

                                    if (await cmd.ExecuteNonQueryAsync() <= 0)
                                    {
                                        return false;
                                    }
                                }

                                // 3.1: Them dapan 
                                if (cauhoi.Chuyende_DapanDtos != null)
                                {
                                    foreach (var dapan in cauhoi.Chuyende_DapanDtos)
                                    {
                                        dapan.MaID = Guid.NewGuid();
                                        dapan.Chuyende_CauhoiID = cauhoi.MaID;
                                        using (var cmd = new SqlCommand(queryAnswerCreate, conn))
                                        {
                                            cmd.Parameters.AddWithValue("@MaID", dapan.MaID);
                                            cmd.Parameters.AddWithValue("@Chuyende_CauhoiID", dapan.Chuyende_CauhoiID);
                                            cmd.Parameters.AddWithValue("@Ten", dapan.Ten);
                                            cmd.Parameters.AddWithValue("@Dung", dapan.Dung);

                                            if (await cmd.ExecuteNonQueryAsync() <= 0)
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        return false;
                    }
                    //}

                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return false;
        }

        public async Task<bool> DeleteTopicByIDAsync(string MaID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var query = @"UPDATE dbo.Chuyende
                            SET 
                            	Khongsudung = 1,
                            	DeletedAt = SYSDATETIME(),
                            	DeletedBy = NEWID()
                            WHERE MaID = @MaID";
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaID", MaID);

                    var affectedRows = await cmd.ExecuteNonQueryAsync();
                    if (affectedRows <= 0)
                    {
                        return false;
                    }
                    return true;
                }
            }
            throw new NotImplementedException();
        }

        public async Task<List<TopicGetAllDto>> GetTopicsAsync() //Them cau lenh query lay thong tin chi tiet
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var list = new List<TopicGetAllDto>();

            using var conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            var query = @"SELECT * FROM Chuyende WHERE Khongsudung = 0";

            using var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new TopicGetAllDto
                {
                    MaID = reader.GetString(0),
                    Ten = reader.GetString(1),
                    Mota = reader.GetString(2)
                });
            }

            return list;
            throw new NotImplementedException();
        }

        public async Task<TopicDetailDto> GetTopicByID(string MaID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var query = @"SELECT cd.MaID, cd.Ten, cd.Mota,
                        cd_tl.MaID AS TailieuID, cd_tl.Tieude, cd_tl.Loaitailieu,
                        cd_ch.MaID AS CauhoiID,cd_ch.Ten AS CauhoiTen, cd_ch.Diem AS Diem,
                        cd_da.MaID AS DapanID,cd_da.Ten AS DapanTen, cd_da.Dung,
                        gv.MaID AS GiangvienID, gv.Hoten
                        FROM dbo.Chuyende cd
                        LEFT JOIN dbo.Chuyende_Giangvien cd_gv ON cd.MaID = cd_gv.ChuyendeID
                        LEFT JOIN dbo.Giangvien gv ON cd_gv.GiangvienID = gv.MaID
                        LEFT JOIN dbo.Chuyende_Tailieu cd_tl ON cd.MaID = cd_tl.ChuyendeID
                        LEFT JOIN dbo.Chuyende_Cauhoi cd_ch ON cd.MaID = cd_ch.ChuyendeID
                        LEFT JOIN dbo.Chuyende_Dapan cd_da ON cd_ch.MaID = cd_da.Chuyende_CauhoiID
                        WHERE cd.MaID = @MaID AND cd.Khongsudung = 0";
            TopicDetailDto topic = null;

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaID", MaID);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (topic == null)
                            {
                                topic = new TopicDetailDto
                                {
                                    MaID = reader["MaID"].ToString(), // reader.GetString("MaID")
                                    Ten = reader["Ten"].ToString(),
                                    Mota = reader["Mota"].ToString(),

                                    Teachers = new List<TeacherDto>(),
                                    Documents = new List<DocumentDto>(),
                                    Questions = new List<QuestionDto>()
                                };
                            }
                            var tailieuId = reader["TailieuID"].ToString(); //reader.GetString(reader.GetOrdinal("TailieuID"));
                            bool tailieuExists = topic.Documents.Any(x => x.MaID == tailieuId);

                            if (!tailieuExists)
                            {
                                topic.Documents.Add(new DocumentDto
                                {
                                    MaID = tailieuId,
                                    Tieude = reader["Tieude"].ToString(),
                                    Loaitailieu = reader["Loaitailieu"].ToString()
                                });
                            }

                            var giangvienId = reader["GiangvienID"].ToString();//reader.GetString(reader.GetOrdinal("GiangvienID"));
                            bool giangvienExists = topic.Teachers.Any(x => x.MaID == giangvienId);

                            if (!giangvienExists)
                            {
                                topic.Teachers.Add(new TeacherDto
                                {
                                    MaID = giangvienId,
                                    Hoten = reader["Hoten"].ToString()
                                });
                            }

                            var cauhoiId = reader["CauhoiID"].ToString();   //reader.GetString(reader.GetOrdinal("CauhoiID"));
                            var question = topic.Questions.FirstOrDefault(x => x.MaID == cauhoiId);

                            if (question == null)
                            {
                                question = new QuestionDto
                                {
                                    MaID = cauhoiId,
                                    Ten = reader["CauhoiTen"].ToString(),
                                    // Diem = Convert.ToDecimal(reader["Diem"]),
                                    Diem = reader.IsDBNull(reader.GetOrdinal("Diem")) ? 0 : Convert.ToDecimal(reader["Diem"]),
                                    Answers = new List<AnswerDto>()
                                };
                                topic.Questions.Add(question);
                            }
                            var dapanId = reader["DapanID"].ToString(); //reader.GetString(reader.GetOrdinal("DapanID"));
                            bool dapanExists = question.Answers.Any(x => x.MaID == dapanId);

                            if (!dapanExists)
                            {
                                question.Answers.Add(new AnswerDto
                                {
                                    MaID = dapanId,
                                    Ten = reader["DapanTen"].ToString(),
                                    //Dung = Convert.ToBoolean(reader["Dung"])
                                    Dung = reader.IsDBNull(reader.GetOrdinal("Dung")) ? false : Convert.ToBoolean(reader["Dung"])
                                });
                            }

                        }
                    }
                }
            }

            if (topic == null)
            {
                return null;
            }
            return topic;

            throw new NotImplementedException();
        }

        public async Task<bool> UpdateTopicAsync(TopicUpdateDto chuyendeDto)
        {
            // Topic

            // Lay object topic - doc - question - answers
            var topic = await _context.Chuyende
                .Include(x => x.Chuyende_Tailieus)
                .Include(x => x.Chuyende_Cauhois)
                    .ThenInclude(x => x.Chuyende_Dapans)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.MaID == chuyendeDto.MaID);


            if (topic == null)
            {
                return false;
            }


            topic.Ten = chuyendeDto.Ten;
            topic.Mota = chuyendeDto.Mota;

            //
            foreach (var documentDto in chuyendeDto.DocumentsDto)
            {
                // Neu MaId cua document == null => Tao moi
                if (string.IsNullOrEmpty(documentDto.MaID))
                {
                    topic.Chuyende_Tailieus.Add(new Chuyende_Tailieu
                    {
                        MaID = Guid.NewGuid().ToString(),
                        Tieude = documentDto.Tieude,
                        Loaitailieu = documentDto.Loaitailieu,
                        Kichthuoc = documentDto.Kichthuoc
                    });
                    continue;
                }

                //Tra ve document khong, neu khong co -> loi
                var existingDocument = topic.Chuyende_Tailieus.FirstOrDefault(x => x.MaID.ToString() == documentDto.MaID);
                if (existingDocument == null)
                {
                    return false;
                }
                // Neu co => Thay doi
                existingDocument.Tieude = documentDto.Tieude;
                existingDocument.Loaitailieu = documentDto.Loaitailieu;
                existingDocument.Kichthuoc = documentDto.Kichthuoc;
            }

            var requestDocumentIds = chuyendeDto.DocumentsDto
                                    .Where(x => !string.IsNullOrEmpty(x.MaID))
                                    .Select(x => x.MaID)
                                    .ToList();
            var deletedDocuments = topic.Chuyende_Tailieus.Where(x => !requestDocumentIds.Contains(x.MaID.ToString()));

            _context.Chuyende_Tailieu.RemoveRange(deletedDocuments);

            // Question
            foreach (var questionDto in chuyendeDto.QuestionsDtos)
            {
                // Neu question khong co ma ID -> Tao Question moi
                if (string.IsNullOrEmpty(questionDto.MaID))
                {
                    var newQuestion = new Chuyende_Cauhoi
                    {
                        MaID = Guid.NewGuid().ToString(),
                        Ten = questionDto.Ten,
                        Diem = questionDto.Diem,
                        Chuyende_Dapans = questionDto.AnswersDtos.Select(a => new Chuyende_Dapan
                        {
                            MaID = Guid.NewGuid().ToString(),
                            Ten = a.Ten,
                            Dung = a.Dung
                        }).ToList()
                    };
                    topic.Chuyende_Cauhois.Add(newQuestion);
                    continue;
                }

                // Doi chieu question, neu null thi tra ve false, neu != null thi update
                var exsitingquestion = topic.Chuyende_Cauhois.FirstOrDefault(x => x.MaID.ToString() == questionDto.MaID);

                if (exsitingquestion == null)
                {
                    return false;
                }

                exsitingquestion.Ten = questionDto.Ten;
                exsitingquestion.Diem = questionDto.Diem;

                foreach (var answers in questionDto.AnswersDtos)
                {
                    if (string.IsNullOrEmpty(answers.MaID))
                    {
                        exsitingquestion.Chuyende_Dapans.Add(new Chuyende_Dapan
                        {
                            MaID = Guid.NewGuid().ToString(),
                            Ten = answers.Ten,
                            Dung = answers.Dung
                        });
                        continue;
                    }

                    var existingAnswer = exsitingquestion.Chuyende_Dapans.FirstOrDefault(x => x.MaID.ToString() == answers.MaID);

                    if (existingAnswer == null)
                    {
                        return false;
                    }
                    existingAnswer.Ten = answers.Ten;
                    existingAnswer.Dung = answers.Dung;
                }
                var requestAnswerIds = questionDto.AnswersDtos
                                        .Where(x => !string.IsNullOrEmpty(x.MaID))
                                        .Select(x => x.MaID)
                                        .ToList();
                var deletedAnswers = exsitingquestion.Chuyende_Dapans.Where(x => !requestAnswerIds.Contains(x.MaID.ToString())).ToList();
                _context.Chuyende_Dapan.RemoveRange(deletedAnswers);
            }

            var requestquestionIds = chuyendeDto.QuestionsDtos
                                        .Where(x => !string.IsNullOrEmpty(x.MaID))
                                        .Select(x => x.MaID)
                                        .ToList();
            var deletequestions = topic.Chuyende_Cauhois.Where(x => requestquestionIds.Contains(x.MaID.ToString())).ToList();
            _context.Chuyende_Cauhoi.RemoveRange(deletequestions);

            await _context.SaveChangesAsync();
            return true;
        }
        #region 
        //Helper
        //     private Status UpdateCheckHelper(List<string> ObjectCheck, string MaID)
        //     {
        //         if(MaID == null)
        //         {
        //             return Status.Create;
        //         }
        //         foreach(var check in ObjectCheck)
        //         {
        //             if(MaID == check)
        //             {
        //                 return Status.Update;
        //             }
        //         }
        //         return Status.Error;

        //     }
        //     private void UpdateHelper(List<string>)
        //     {

        //     }
        //     private async Task<CheckTopicDto> GetTopicItemIds(SqlConnection cnn)
        //     {
        //         CheckTopicDto dto = new CheckTopicDto();
        //         var queryDocumentsList = @"SELECT MaID FROM Chuyende_Tailieu";
        //         var queryQuestionsList = @"SELECT MaID FROM Chuyende_Cauhoi";
        //         var queryAnswersList = @"SELECT MaID FROM Chuyende_Dapan";

        //         using (var cmd = new SqlCommand(queryDocumentsList, cnn))
        //         {
        //             using var reader = await cmd.ExecuteReaderAsync();

        //             while (await reader.ReadAsync())
        //             {
        //                 dto.DocumentsID.Add(reader["MaID"].ToString());
        //             }
        //         }
        //         using (var cmd = new SqlCommand(queryQuestionsList, cnn))
        //         {
        //             using var reader = await cmd.ExecuteReaderAsync();

        //             while (await reader.ReadAsync())
        //             {
        //                 dto.QuestionsID.Add(reader["MaID"].ToString());
        //             }
        //         }
        //         using (var cmd = new SqlCommand(queryAnswersList, cnn))
        //         {
        //             using var reader = await cmd.ExecuteReaderAsync();

        //             while (await reader.ReadAsync())
        //             {
        //                 dto.AnswersID.Add(reader["MaID"].ToString());
        //             }
        //         }

        //         if(dto == null)
        //         {
        //             return null;
        //         }
        //         return dto;

        //     }

        //     public enum Status
        //     {
        //         Create,
        //         Delete,
        //         Update,
        //         Error
        //     }
        // }
        #endregion
    }
}


