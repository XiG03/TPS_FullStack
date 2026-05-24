using TPS_FullStack.Server.Modules.Admin;

namespace TPS_FullStack.Server
{
    public interface ICertificateRepository
    {
        public Task<List<CertificateGetAllDto>> CertificateGetAllAsync();
        public Task<CertificateDetailDto> CertificateFindByIdAsync(string MaID);
        public Task<bool> CertificateInsertAsync(CertificateCreateDto createDTO);
        public Task<bool> CertificateUpdateAsync(CertificateUpdateDto updateDto);
        public Task<bool> CertificateDeleteAsync(string MaID);

        // Giai thich tieng viet - 2 ham nay ho tro Admin trong viec cap / ngung chung chi cho hoc vien

        public Task<bool> CertificateStuAcceptAsync(Cert_StudentCreateDto createDto);
        public Task<bool> CertificateStuRevokeAsync(string MaID);
    }
}


