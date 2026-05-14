using TPS_FullStack.Server.Modules.Admin;

namespace TPS_FullStack.Server
{
    public interface ICertificateRepository
    {
        public Task<List<CertificateGetAllDto>> CertificateGetAllAsync();
        public Task<CertificateDetailDto> CertificateFindById(string MaID);
    }
}


