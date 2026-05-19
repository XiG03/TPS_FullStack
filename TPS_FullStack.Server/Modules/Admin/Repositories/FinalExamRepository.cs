using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Identity.Client;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class FinalExamRepository : IFinalExamRepository
    {
        private readonly IConfiguration _configuration;
        public FinalExamRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Lấy danh sách câu hỏi cho kỳ thi cuối khóa dựa trên ID khóa học
        public async Task<List<examQuestions>> GetExamQuestions(string KhoahocID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryGetQuestions = @" SELECT cdch.ChuyendeID, cdch.MaID AS CauhoiID, cdch.Ten AS Tencauhoi, cdcd.MaID AS DapanID, cdcd.Ten AS Nddapan 
                                        FROM dbo.Chuyende_Cauhoi cdch
                                        JOIN dbo.Chuyende_Dapan cdcd ON cdch.MaID = cdcd.Chuyende_CauhoiID
                                        WHERE cdch.ChuyendeID IN (SELECT ChuyendeID FROM dbo.Khoahoc_Chuyende WHERE KhoahocID = @KhoahocID) AND cdcd.Dung = 1
                                        ORDER BY cdch.ChuyendeID, cdch.MaID";
            using(var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand(queryGetQuestions, conn))
                {
                    cmd.Parameters.AddWithValue("@KhoahocID", KhoahocID);
                    await conn.OpenAsync();
                    var reader = cmd.ExecuteReader();
                    if(!reader.HasRows)
                    {
                        return null;
                    }
                    var examQuestionsList = new List<examQuestions>();
                    while(reader.Read())
                    {
                        var examQuestion = new examQuestions
                        {
                            ChuyendeID = reader["ChuyendeID"].ToString(),
                            CauhoiID = reader["CauhoiID"].ToString(),
                            Tencauhoi = reader["Tencauhoi"].ToString(),
                            DapanID = reader["DapanID"].ToString(),
                            Noidungdapan = reader["Nddapan"].ToString()
                        };
                        examQuestionsList.Add(examQuestion);
                    }
                    return await Task.FromResult(examQuestionsList);
                }
            }
            throw new NotImplementedException();
        }

        // Chèn danh sách câu hỏi của kỳ thi cuối khóa vào cơ sở dữ liệu
        public async Task<bool> FinalExamInsertAsync( List<finalExams> finalExamQuestionsList)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryFinalExamInsert = @"INSERT INTO dbo.Baithuhoach(MaID, KhoahocID, HocvienID, Thoigianlambai, CreatedAt, CreatedBy) 
                                        VALUES (@BaithuhoachID, @KhoahocID, @HocvienID, @Thoigianlambai, @CreatedAt, @CreatedBy)";
            var querFinalExamQuestionInsert = @"INSERT INTO dbo.Baithuhoach_Cauhoi(MaID, BaithuhoachID, KhoahocID, ChuyendeID, CauhoiID, Tencauhoi, DapanID, Nddapan)
                                                VALUES (@Baithuhoach_CauhoiID, @BaithuhoachID, @KhoahocID, @ChuyendeID, @CauhoiID, @Tencauhoi, @DapanID, @Nddapan)";


            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        foreach(var finalExamQuestion in finalExamQuestionsList)
                        {
                            using (var cmd = new SqlCommand(queryFinalExamInsert, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@BaithuhoachID", finalExamQuestion.MaID);
                                cmd.Parameters.AddWithValue("@KhoahocID", finalExamQuestion.KhoahocID);
                                cmd.Parameters.AddWithValue("@HocvienID", finalExamQuestion.HocvienID);
                                cmd.Parameters.AddWithValue("@Thoigianlambai", 60);
                                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);
                                cmd.Parameters.AddWithValue("@CreatedBy", "System");
                                await cmd.ExecuteNonQueryAsync();
                            }
                            foreach(var question in finalExamQuestion.Danhsachcauhoi)
                            {
                                using (var cmd = new SqlCommand(querFinalExamQuestionInsert, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@Baithuhoach_CauhoiID", question.MaID);
                                    cmd.Parameters.AddWithValue("@BaithuhoachID", finalExamQuestion.MaID);
                                    cmd.Parameters.AddWithValue("@KhoahocID", question.KhoahocID);
                                    cmd.Parameters.AddWithValue("@ChuyendeID", question.ChuyendeID);
                                    cmd.Parameters.AddWithValue("@CauhoiID", question.CauhoiID);
                                    cmd.Parameters.AddWithValue("@Tencauhoi", question.Tencauhoi);
                                    cmd.Parameters.AddWithValue("@DapanID", question.DapanID);
                                    cmd.Parameters.AddWithValue("@Nddapan", question.Noidungdapan);
                                    await cmd.ExecuteNonQueryAsync();
                                }
                            }
                        }
                        transaction.Commit();
                        return await Task.FromResult(true);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // Log the exception (ex) here as needed
                        return await Task.FromResult(false);
                    }
                }
            }
            throw new NotImplementedException();
        }

        // Lấy danh sách câu hỏi (trong mục quản lý bài thu hoạch)
        public async Task<List<finalExamList>> GetFinalExamListsAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryGetList = @"SELECT bth.MaID, bth.KhoahocID, bth.HocvienID, hv.Hoten AS Tenhocvien, bth.Thoigianlambai, dbo.fn_Baithuhoach_Tinhdiem(bth.MaID) AS Diem
                                FROM dbo.Baithuhoach bth
                                JOIN dbo.Hocvien hv ON bth.HocvienID = hv.MaID ";
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand(queryGetList, conn))
                {
                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        var list = new List<finalExamList>();
                        while(await reader.ReadAsync())
                        {
                            list.Add(new finalExamList
                            {
                                MaID = reader["MaID"].ToString(),
                                KhoahocID = reader["KhoahocID"].ToString(),
                                HocvienID = reader["HocvienID"].ToString(),
                                Tenhocvien = reader["Tenhocvien"].ToString(),
                                Thoigianlambai = Convert.ToDecimal(reader["Thoigianlambai"]),
                                Diem = Convert.ToDecimal(reader["Diem"])
                            });
                        }
                        return list;
                    }
                }
            }
            throw new NotImplementedException();
        }

        public async Task<List<finalExamList>> GetFinalExamListsAsync(string KhoahocID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryGetList = @"SELECT bth.MaID, bth.KhoahocID, bth.HocvienID, hv.Hoten AS Tenhocvien, bth.Thoigianlambai, dbo.fn_Baithuhoach_Tinhdiem(bth.MaID) AS Diem
                                FROM dbo.Baithuhoach bth
                                JOIN dbo.Hocvien hv ON bth.HocvienID = hv.
                                WHERE bth.KhoahocID = @KhoahocID ";
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand(queryGetList, conn))
                {
                    cmd.Parameters.AddWithValue("@KhoahocID", KhoahocID);
                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        var list = new List<finalExamList>();
                        while(await reader.ReadAsync())
                        {
                            list.Add(new finalExamList
                            {
                                MaID = reader["MaID"].ToString(),
                                KhoahocID = reader["KhoahocID"].ToString(),
                                HocvienID = reader["HocvienID"].ToString(),
                                Tenhocvien = reader["Tenhocvien"].ToString(),
                                Thoigianlambai = Convert.ToDecimal(reader["Thoigianlambai"]),
                                Diem = Convert.ToDecimal(reader["Diem"])
                            });
                        }
                        return list;
                    }
                }
            }
            throw new NotImplementedException();
        }

        public async Task<finalExamDetail> GetFinalExamDetailAsync(string MaID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryExam = @"SELECT bth.MaID, bth.KhoahocID, bth.HocvienID, bth.HocvienID, 
                                (SELECT Top 1 Hoten FROM dbo.Hocvien WHERE bth.HocvienID = MaID) AS Tenhocvien, 
                                bth.Thoigianlambai, bth.Batdauthi, bth.Ketthucthi, dbo.fn_Baithuhoach_Tinhdiem(bth.MaID) AS Diem
                                FROM dbo.Baithuhoach bth
                                WHERE bth.MaID = @MaID"
                            + @"SELECT btch.MaID, btch.BaithuhoachID, btch.KhoahocID, btch.ChuyendeID, btch.CauhoiID, btch.Tencauhoi, btch.TraloiID, btch.NdTraloi, btch.DapanID, btch.Nddapan, btch.Dung
                                FROM dbo.Baithuhoach_Cauhoi btch
                                WHERE btch.BaithuhoachID = @MaID";

            using(var conn = new SqlConnection(connectionString))
            {
                using(var cmd = new SqlCommand(queryExam, conn))
                {
                    cmd.Parameters.AddWithValue("@MaID", MaID);
                    await conn.OpenAsync();
                    using (var reader  = await cmd.ExecuteReaderAsync())
                    {
                        var examDetail = new finalExamDetail();
                        if(await reader.ReadAsync())
                        {
                            examDetail.MaID = reader["@MaID"].ToString();
                            examDetail.KhoahocID = reader["@KhoahocID"].ToString();
                            examDetail.HocvienID = reader["@HocvienID"].ToString();
                            examDetail.Tenhocvien = reader["@Tenhocvien"].ToString();
                            examDetail.Thoigianlambai = Convert.ToDecimal(reader["@Thoigianlambai"]);
                            examDetail.Batdauthi = Convert.ToDateTime(reader["@Batdauthi"]);
                            examDetail.Ketthucthi = Convert.ToDateTime(reader["@Ketthucthi"]);
                            examDetail.Diem = Convert.ToDecimal(reader["@Diem"]);
                            
                        }
                        if(await reader.NextResultAsync())
                        {
                            while(await reader.ReadAsync())
                            {
                                examDetail.Danhsachcauhoi.Add(new finalExamQuestionDetail
                                {
                                    MaID = reader["@MaID"].ToString(),
                                    BaithuhoachID = reader["@BaithuhoachID"].ToString(),
                                    KhoahocID = reader["@KhoahocID"].ToString(),
                                    ChuyendeID = reader["@ChuyendeID"].ToString(),
                                    CauhoiID = reader["@CauhoiID"].ToString(),
                                    Tencauhoi = reader["@Tencauhoi"].ToString(),
                                    TraloiID = reader["@TraloiID"].ToString(),
                                    Noidungtraloi = reader["@NdTraloi"].ToString(),
                                    DapanID = reader["@DapanID"].ToString(),
                                    Noidungdapan = reader["@Nddapan"].ToString(),
                                    Dung = Convert.ToBoolean(reader["@Dung"])
                                });
                            }
                        }
                        return examDetail;
                    }

                }
            }
            throw new NotImplementedException();
        }

        public Task<bool> UpdateFinalExamScoreAsync(finalExamUpdate finalExamUpdate)
        {
            throw new NotImplementedException();
        }
    }

}

