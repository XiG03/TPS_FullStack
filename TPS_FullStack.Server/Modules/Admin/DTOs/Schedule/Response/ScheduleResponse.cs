using System;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class ScheduleResponse
    {
        public string? LichhocID { get; set; }
        public string? KhoahocID { get; set; }
        public string? TenKhoahoc { get; set; }
        public string? ChuyendeID { get; set; }
        public string? TenChuyende { get; set; }
        public string? GiangvienID { get; set; }
        public string? TenGiangvien { get; set; }
        public DateTime? Ngaydukien { get; set; }
        public DateTime? Batdaudukien { get; set; }
        public DateTime? Ketthucdukien { get; set; }
        public DateTime? Ngaythucte { get; set; }
        public DateTime? Batdauthucte { get; set; }
        public DateTime? Ketthucthucte { get; set; }
        public bool? Trangthai { get; set; }
    }
}