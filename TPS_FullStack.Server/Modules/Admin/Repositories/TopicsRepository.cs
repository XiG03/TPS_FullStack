using System.Data;
using System.IO.Pipelines;
using System.Net;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Transactions;
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

            var queryAnswerCreate = @"INSERT INTO Chuyende_Dapan(MaID, ChuyendeID, Chuyende_CauhoiID, Ten, Dung)
                                    VALUES (@MaID, @ChuyendeID, @Chuyende_CauhoiID, @Ten, @Dung)";


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
                    using var trans = conn.BeginTransaction();
                    try
                    {
                        // Execute 1: Them chuyen de moi
                        using (var cmd = new SqlCommand(queryTopicCreate, conn, trans))
                        {
                            //warning
                            cmd.Parameters.AddWithValue("@MaID", chuyendeDto.MaID);
                            cmd.Parameters.AddWithValue("@Ten", chuyendeDto.Ten);
                            cmd.Parameters.AddWithValue("@Mota", chuyendeDto.Mota);

                            if (await cmd.ExecuteNonQueryAsync() <= 0)
                            {
                                await trans.RollbackAsync();
                                return false;
                            }
                        }
                        if (chuyendeDto.Chuyende_TailieuDtos != null)
                        {
                            foreach (var tailieu in chuyendeDto.Chuyende_TailieuDtos)
                            {
                                tailieu.MaID = Guid.NewGuid();
                                tailieu.ChuyendeID = chuyendeDto.MaID;

                                using (var cmd = new SqlCommand(queryDocumentCreate, conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("@MaID", tailieu.MaID);
                                    cmd.Parameters.AddWithValue("@ChuyendeID", tailieu.ChuyendeID);
                                    cmd.Parameters.AddWithValue("@Tieude", tailieu.Tieude);
                                    cmd.Parameters.AddWithValue("@Loaitailieu", tailieu.Loaitailieu);
                                    cmd.Parameters.AddWithValue("@Kichthuoc", tailieu.Kichthuoc);

                                    if (await cmd.ExecuteNonQueryAsync() <= 0)
                                    {
                                        await trans.RollbackAsync();
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

                                using (var cmd = new SqlCommand(queryQuestionCreate, conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("@MaID", cauhoi.MaID);
                                    cmd.Parameters.AddWithValue("@ChuyendeID", cauhoi.ChuyendeID);
                                    cmd.Parameters.AddWithValue("@Ten", cauhoi.Ten);
                                    cmd.Parameters.AddWithValue("@Diem", cauhoi.Diem);

                                    if (await cmd.ExecuteNonQueryAsync() <= 0)
                                    {
                                        await trans.RollbackAsync();
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
                                        using (var cmd = new SqlCommand(queryAnswerCreate, conn, trans))
                                        {
                                            cmd.Parameters.AddWithValue("@MaID", dapan.MaID);
                                            cmd.Parameters.AddWithValue("@ChuyendeID", chuyendeDto.MaID);
                                            cmd.Parameters.AddWithValue("@Chuyende_CauhoiID", dapan.Chuyende_CauhoiID);
                                            cmd.Parameters.AddWithValue("@Ten", dapan.Ten);
                                            cmd.Parameters.AddWithValue("@Dung", dapan.Dung);

                                            if (await cmd.ExecuteNonQueryAsync() <= 0)
                                            {
                                                trans.RollbackAsync();
                                                return false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        await trans.CommitAsync();
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
                        cd_da.MaID AS DapanID,cd_da.Chuyende_CauhoiID AS CauhoiID,cd_da.Ten AS DapanTen, cd_da.Dung,
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
                                    CauhoiID = reader["CauhoiID"].ToString(),
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
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Gom Update bang chinh, xoa bang phu va create record moi 
            #region 
            var queryTopicUpdate = @"UPDATE dbo.Chuyende
                                    SET
                                    	Ten = @Ten,
                                    	Mota = @Mota,
                                    	UpdatedAt = SYSDATETIME(),
                                    	UpdatedBy = ''
                                    WHERE MaID = @MaID";
            var queryDocDelete = @"DELETE dbo.Chuyende_Tailieu
                                    WHERE ChuyendeID = @ChuyendeID";

            var queryDocInsert = @"INSERT INTO Chuyende_Tailieu(MaID, ChuyendeId, Tieude, Ngaytao, Loaitailieu, Kichthuoc)
                                    VALUES (@MaID, @ChuyendeID, @Tieude, @Ngaytao, @Loaitailieu, @Kichthuoc)";

            var queryAnsDelete = @"DELETE dbo.Chuyende_Dapan
                                    WHERE ChuyendeID = @ChuyendeID";

            var queryQuesDelete = @"DELETE dbo.Chuyende_Cauhoi
                                    WHERE ChuyendeID = @ChuyendeID";

            var queryQuesInsert = @"INSERT INTO Chuyende_Cauhoi(MaID, ChuyendeID, Ten, Diem)
                                    VALUES (@MaID ,@ChuyendeID, @Ten, @Diem)";

            var queryAnsInsert = @"INSERT INTO Chuyende_Dapan(MaID, Chuyende_CauhoiID, ChuyendeID, Ten, Dung)
                                    VALUES (@MaID, @CauhoiID, @ChuyendeID, @Ten,@Dung)";
            #endregion

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                //Update Topic information
                using var trans = conn.BeginTransaction();
                try
                {
                    using (var cmd = new SqlCommand(queryTopicUpdate, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@Ten", chuyendeDto.Ten);
                        cmd.Parameters.AddWithValue("@Mota", chuyendeDto.Mota);
                        // cmd.Parameters.AddWithValue("@Ten", chuyendeDto.Ten); -- Update updated by
                        cmd.Parameters.AddWithValue("@MaID", chuyendeDto.MaID);

                        if (await cmd.ExecuteNonQueryAsync() < 0)
                        {
                            await trans.RollbackAsync();
                            return false;
                        }
                    }

                    #region //Documents

                    //#1 Delete document if have topicID
                    using (var cmd = new SqlCommand(queryDocDelete, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@ChuyendeID", chuyendeDto.MaID);

                        await cmd.ExecuteNonQueryAsync();
                    }

                    foreach (var docs in chuyendeDto.DocumentsDto)
                    {
                        using (var cmd = new SqlCommand(queryDocInsert, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@MaID", docs.MaID == null ? Guid.NewGuid().ToString()
                                                                                    : docs.MaID);
                            cmd.Parameters.AddWithValue("@ChuyendeID", chuyendeDto.MaID);
                            cmd.Parameters.AddWithValue("@Tieude", docs.Tieude);
                            cmd.Parameters.AddWithValue("@Ngaytao", docs.Ngaytao);
                            cmd.Parameters.AddWithValue("@Loaitailieu", docs.Loaitailieu);
                            cmd.Parameters.AddWithValue("@Kichthuoc", docs.Kichthuoc);

                            if (await cmd.ExecuteNonQueryAsync() < 0)
                            {
                                await trans.RollbackAsync();
                                return false;
                            }
                        }
                    }

                    #endregion

                    #region //Questions

                    //#1 Delete Ans and Ques
                    using (var cmd = new SqlCommand(queryAnsDelete, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@ChuyendeID", chuyendeDto.MaID);
                        await cmd.ExecuteNonQueryAsync();
                    }
                    using (var cmd = new SqlCommand(queryQuesDelete, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@ChuyendeID", chuyendeDto.MaID);
                        await cmd.ExecuteNonQueryAsync();
                    }
                    //#2 Insert Ques
                    foreach (var ques in chuyendeDto.QuestionsDtos)
                    {
                        using (var cmd = new SqlCommand(queryQuesInsert, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@MaID", ques.MaID == null ? Guid.NewGuid().ToString()
                                                                                    : ques.MaID);
                            cmd.Parameters.AddWithValue("@ChuyendeID", chuyendeDto.MaID);
                            cmd.Parameters.AddWithValue("@Ten", ques.Ten);
                            cmd.Parameters.AddWithValue("@Diem", ques.Diem);

                            if (await cmd.ExecuteNonQueryAsync() < 0)
                            {
                                await trans.RollbackAsync();
                                return false;
                            }
                        }
                        //#3 Insert Ans - Done
                        foreach (var ans in ques.AnswersDtos)
                        {
                            using (var cmd = new SqlCommand(queryAnsInsert, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@MaID", ans.MaID == null ? Guid.NewGuid().ToString()
                                                                                        : ans.MaID);
                                cmd.Parameters.AddWithValue("@CauhoiID", ques.MaID);
                                cmd.Parameters.AddWithValue("@ChuyendeID", chuyendeDto.MaID);
                                cmd.Parameters.AddWithValue("@Ten", ans.Ten);
                                cmd.Parameters.AddWithValue("@Dung", ans.Dung);

                                if (await cmd.ExecuteNonQueryAsync() < 0)
                                {
                                    await trans.RollbackAsync();
                                    return false;
                                }
                            }
                        }
                    }
                    #endregion
                    await trans.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync();
                    _logger.LogError(ex.Message);
                    return false;
                }
            }
            return false;
        }
    }
}