namespace TPS_FullStack.Server.Modules.Admin
{
    public interface ICertificateService
    {
        public Task<ServiceDefault<List<CertificateGetAllDto>>> CertificateGetAllAsync();
        public Task<ServiceDefault<CertificateDetailDto>> CertificateFindByIdAsync(string MaID);
        public Task<ServiceDefault<CertificateCreateDto>> CertificateInsertAsync(CertificateCreateDto createDTO);
        public Task<ServiceDefault<CertificateUpdateDto>> CertificateUpdateAsync(CertificateUpdateDto updateDTO);
        public Task<ServiceDefault<bool>> CertificateDeleteAsync(string MaID);

        // cap/ ngung chung chi cho hoc vien

        public Task<ServiceDefault<Cert_StudentCreateDto>> CertificateStuAcceptAsync(Cert_StudentCreateDto createDto);
        public Task<ServiceDefault<bool>> CertificateStuRevokeAsync(string MaID);

    }

}

