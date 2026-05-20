using Microsoft.Data.SqlClient;

namespace TPS_FullStack.Server.Modules.Student
{
    public class StudentExamRepository : IStudentExamRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<StudentExamRepository> _logger;
        public StudentExamRepository(IConfiguration configuration,
                                    ILogger<StudentExamRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<BaithuhoachInfo> GetFinalExamInfoAsync(string HocvienID, string KhoahocID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryexamInfo = @"SELECT Top 1 bth.MaID, bth.KhoahocID, bth.HocvinienID,
                                    FROM dbo.Baithuhoach bth
                                    WHERE bth.HocvienID = @HocvienID AND bth.KhoahocID = @KhoahocID"
                                + @"SELECT bthch.MaID, bthch.BaithuhoachID, bthch.KhoahocID, bthch.ChuyendeID, bthch.CauhoiID, ch.Tencauhoi,
                                    FROM dbo.Baithuhoach_Cauhoi bthch
                                    JOIN dbo.Cauhoi ch ON bthch.CauhoiID = ch.MaID
                                    WHERE bthch.BaithuhoachID = @BaithuhoachID";
            var queryQuesAns = @"SELECT cdcd.MaID, cdcd.CauhoiID, cdcd.Ten AS Ten
                                FROM dbo.Chuyende_Dapan cdcd
                                WHERE cdcd.CauhoiID = @CauhoiID";

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(queryexamInfo, conn))
                {
                    cmd.Parameters.AddWithValue("@HocvienID", HocvienID);
                    cmd.Parameters.AddWithValue("@KhoahocID", KhoahocID);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var examInfo = new BaithuhoachInfo
                            {
                                MaID = reader.GetString(0),
                                KhoahocID = reader.GetString(1),
                                HocvienID = reader.GetString(2)
                            };
                            reader.NextResult();
                            var cauhoiList = new List<BaithuhoachCauhoiInfo>();
                            while (await reader.ReadAsync())
                            {
                                var cauhoiInfo = new BaithuhoachCauhoiInfo
                                {
                                    MaID = reader.GetString(0),
                                    BaithuhoachID = reader.GetString(1),
                                    KhoahocID = reader.GetString(2),
                                    ChuyendeID = reader.GetString(3),
                                    CauhoiID = reader.GetString(4),
                                    Tencauhoi = reader.GetString(5),
                                    Dapan = new List<BaithuhoachCauhoiDapanInfo>()
                                };
                                using (var cmdQuesAns = new SqlCommand(queryQuesAns, conn))
                                {
                                    cmdQuesAns.Parameters.AddWithValue("@CauhoiID", cauhoiInfo.CauhoiID);
                                    using (var readerQuesAns = await cmdQuesAns.ExecuteReaderAsync())
                                    {
                                        while (await readerQuesAns.ReadAsync())
                                        {
                                            cauhoiInfo.Dapan.Add(new BaithuhoachCauhoiDapanInfo
                                            {
                                                MaID = readerQuesAns.GetString(0),
                                                CauhoiID = readerQuesAns.GetString(1),
                                                Ten = readerQuesAns.GetString(2)
                                            });
                                        }
                                    }
                                }
                                cauhoiList.Add(cauhoiInfo);
                            }
                            examInfo.Danhsachcauhoi = cauhoiList;
                            return examInfo;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            throw new NotImplementedException();
        }

        public async Task<decimal> SubmitFinalExamAsync(finalExam finalRecord)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryUpdateExam = @"UPDATE dbo.Baithuhoach bth
                                    SET
                                            bth.Batdauthi = @Batdauthi,
                                            bth.Ketthucthi = @Ketthucthi,
                                    WHERE bth.MaID = @BaithuhoachID AND bth.HocvienID = @HocvienID";
            var queryUpdateQues = @"UPDATE dbo.Baithuhoach_Cauhoi
                                    SET
                                        TraloiID = @TraloiID,
                                        NdTraloi = @NdTraloi,
                                        Dung = CASE 
                                                   WHEN @TraloiID = DapanID THEN 1 
                                                   ELSE 0 
                                               END
                                    WHERE 
                                        BaithuhoachID = @BaithuhoachID 
                                        AND CauhoiID = @CauhoiID;";
            var queryGetScore = @"SELECT dbo.fn_Baithuhoach_Tinhdiem(@BaithuhoachID) AS Diem
                                    FROM dbo.Baithuhoach bth
                                    WHERE bth.MaID = @BaithuhoachID";

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (var cmdExam = new SqlCommand(queryUpdateExam, conn, transaction))
                        {
                            cmdExam.Parameters.AddWithValue("@Batdauthi", finalRecord.Batdauthi);
                            cmdExam.Parameters.AddWithValue("@Ketthucthi", finalRecord.Ketthucthi);
                            cmdExam.Parameters.AddWithValue("@BaithuhoachID", finalRecord.MaID);
                            cmdExam.Parameters.AddWithValue("@HocvienID", finalRecord.HocvienID);
                            
                            var rowsAffectedExam = await cmdExam.ExecuteNonQueryAsync();
                            if (rowsAffectedExam == 0)
                            {
                                await transaction.RollbackAsync();
                                return -1; // Indicate failure to update exam info
                            }
                            foreach(var cauhoi in finalRecord.Danhsachcauhoi)
                            {
                                using (var cmdQues = new SqlCommand(queryUpdateQues, conn, transaction))
                                {
                                    cmdQues.Parameters.AddWithValue("@TraloiID", cauhoi.DapanID);
                                    cmdQues.Parameters.AddWithValue("@NdTraloi", cauhoi.Noidungtraloi);
                                    cmdQues.Parameters.AddWithValue("@BaithuhoachID", finalRecord.MaID);
                                    cmdQues.Parameters.AddWithValue("@CauhoiID", cauhoi.CauhoiID);
                                    
                                    var rowsAffectedQues = await cmdQues.ExecuteNonQueryAsync();
                                    if (rowsAffectedQues == 0)
                                    {
                                        await transaction.RollbackAsync();
                                        return -1;
                                    }
                                }
                            }
                        }
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error submitting final exam for BaithuhoachID: {BaithuhoachID}, HocvienID: {HocvienID}", finalRecord.MaID, finalRecord.HocvienID);
                        await transaction.RollbackAsync();
                        return -1;
                    }
                }
                using (var cmdGetScore = new SqlCommand(queryGetScore, conn))
                {
                    cmdGetScore.Parameters.AddWithValue("@BaithuhoachID", finalRecord.MaID);
                    var scoreObj = await cmdGetScore.ExecuteScalarAsync();
                    if (scoreObj != null && decimal.TryParse(scoreObj.ToString(), out decimal score))
                    {
                        return score;
                    }
                    else
                    {
                        _logger.LogWarning("Failed to retrieve score for BaithuhoachID: {BaithuhoachID}", finalRecord.MaID);
                        return -1; // Indicate failure to retrieve score
                    }
                }
            }

        }
    }

}

