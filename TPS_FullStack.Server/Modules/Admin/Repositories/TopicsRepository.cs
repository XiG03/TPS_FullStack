using System.Data;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Data.SqlClient;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TopicsRepository : ITopicsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AppUser> _logger;
        public TopicsRepository(IConfiguration configuration,
                                ILogger<AppUser> logger)
        {
            _configuration = configuration;
            _logger = logger;
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


            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

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

                    // Execute 2: Them tai lieu cho chuyen de 
                    // Warning forweach: Neu khong them duoc thi lam sao
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

                    // Execute 3: Them cau hoi va dap an cho chuyen de
                    // Warning foreach
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
                return true;
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return false;
        }

        public async Task<List<Chuyende>> GetChuyendesAsync() //Them cau lenh query lay thong tin chi tiet
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var list = new List<Chuyende>();

            using var conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            var query = @"SELECT * FROM Chuyende";

            using var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Chuyende
                {
                    MaID = reader.GetGuid(0),
                    Ten = reader.GetString(1),
                    Mota = reader.GetString(2)
                });
            }

            return list;
            throw new NotImplementedException();
        }

        public Task<Chuyende_ChitietDto> GetTopicByID(string MaID)
        {
            throw new NotImplementedException();
        }
    }

}

