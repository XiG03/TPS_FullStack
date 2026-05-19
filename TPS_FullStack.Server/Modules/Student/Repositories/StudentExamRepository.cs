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

        public Task<courseQuestions> GetFinalExamInfo(string KhoahocID)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryGetAllQues = @"";
            throw new NotImplementedException();
        }

        public async Task<bool> SubmitFinalExam(finalExam finalRecord)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var queryExamInsert = @"INSERT INTO dbo.Baithuhoach (MaID, KhoahocID, HocvienID, Diem, Thoigianlambai, CreatedAt, CreatedBy)
                                VALUES (@BaithiID, @KhoahocID, @HocvienID, @Diem, @Thoigianlambai, SYSDATETIME(), @HocvienID)";
            var queryExamQuesInsert = @"INSERT INTO dbo.Baithuhoach_Cauhoi (MaID, BaithuhoachID, CauhoiID, TraloiID, DapanID, Dung)
                                        VALUES (@Baithi_CauhoiID, @BaithiID, @CauhoiID, @TraloiID, @DapanID, @Dung)";



            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        using (var cmd = new SqlCommand(queryExamInsert, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@BaithiID", finalRecord.examInfo.BaithiID = Guid.NewGuid().ToString());
                            cmd.Parameters.AddWithValue("@KhoahocID", finalRecord.examInfo.KhoahocID);
                            cmd.Parameters.AddWithValue("@HocvienID", finalRecord.examInfo.HocvienID);
                            cmd.Parameters.AddWithValue("@Diem", finalRecord.examInfo.Diem);
                            cmd.Parameters.AddWithValue("@Thoigianlambai", finalRecord.examInfo.Thoigianlambai);
                            await cmd.ExecuteNonQueryAsync();
                        }

                        foreach (var quesAns in finalRecord.examQuesAns)
                        {
                            using (var cmd = new SqlCommand(queryExamQuesInsert, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@Baithi_CauhoiID", quesAns.MaID = Guid.NewGuid().ToString());
                                cmd.Parameters.AddWithValue("@BaithiID", finalRecord.examInfo.BaithiID);
                                cmd.Parameters.AddWithValue("@CauhoiID", quesAns.CauhoiID);
                                cmd.Parameters.AddWithValue("@TraloiID", quesAns.TraloiID);
                                cmd.Parameters.AddWithValue("@DapanID", quesAns.DapanID);
                                cmd.Parameters.AddWithValue("@Dung", quesAns.Dung);
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                        await trans.CommitAsync();
                        return true;

                    }
                    catch (Exception ex)
                    {
                        await trans.RollbackAsync();
                        // Log the exception (ex) here as needed
                        _logger.LogError(ex, "Error occurred while submitting final exam for HocvienID: {HocvienID}, KhoahocID: {KhoahocID}", finalRecord.examInfo.HocvienID, finalRecord.examInfo.KhoahocID);
                        return false;

                    }
                }
                throw new NotImplementedException();
            }

        }
    }

}

