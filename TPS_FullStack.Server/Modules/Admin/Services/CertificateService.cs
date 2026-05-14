using System.Diagnostics;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class CertificateService : ICertificateService
    {
        private readonly ICertificateRepository _certificateRepository;
        public CertificateService (ICertificateRepository certificateRepository)
        {
            _certificateRepository = certificateRepository;
        }
        public async Task<ServiceDefault<bool>> CertificateDeleteAsync(string MaID)
        {
            var result = await _certificateRepository.CertificateDeleteAsync(MaID);
            if (!result)
            {
                return new ServiceDefault<bool>
                {
                    statusCode = StatusCodes.Status500InternalServerError,
                    Message = "Server chua xoa duoc",
                    Data = false
                };
            }
            if (result)
            {
                return new ServiceDefault<bool>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Server da xoa",
                    Data = true
                };
            }
            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<CertificateDetailDto>> CertificateFindByIdAsync(string MaID)
        {
            var result = await _certificateRepository.CertificateFindByIdAsync(MaID);
            if(result == null)
            {
                return new ServiceDefault<CertificateDetailDto>
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    Message = "Server khong tim thay chung chi",
                    Data = null
                };
            }
            if(result != null)
            {
                return new ServiceDefault<CertificateDetailDto>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Server tim thay chung chi " + MaID,
                    Data = result
                };
            }
            
            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<List<CertificateGetAllDto>>> CertificateGetAllAsync()
        {
            var result = await _certificateRepository.CertificateGetAllAsync();
            if(result == null)
            {
                return new ServiceDefault<List<CertificateGetAllDto>>
                {
                    statusCode = StatusCodes.Status500InternalServerError,
                    Message = "Server khong tim thay danh sach chung chi",
                    Data = null
                };
            }
            if(result != null)
            {
                return new ServiceDefault<List<CertificateGetAllDto>>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Tim thay danh sach chung chi",
                    Data = result
                };
            }
            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<CertificateCreateDto>> CertificateInsertAsync(CertificateCreateDto createDTO)
        {
            var result = await _certificateRepository.CertificateInsertAsync(createDTO);
            if (!result)
            {
                return new ServiceDefault<CertificateCreateDto>
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    Message = "Server khong tao duoc chung chi",
                    Data = null
                };
            }
            if (result)
            {
                return new ServiceDefault<CertificateCreateDto>
                {
                    statusCode = StatusCodes.Status201Created,
                    Message = "Tao chung chi thanh cong",
                    Data = createDTO
                };
            }
            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<CertificateUpdateDto>> CertificateUpdateAsync(CertificateUpdateDto updateDTO)
        {
            var result = await _certificateRepository.CertificateUpdateAsync(updateDTO);
            if (result)
            {
                return new ServiceDefault<CertificateUpdateDto>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Server da cap nhat cau hinh chung chi",
                    Data = updateDTO
                };
            }
            if (!result)
            {
                return new ServiceDefault<CertificateUpdateDto>
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    Message = "Server khong update duoc",
                    Data = null
                };
            }
            throw new NotImplementedException();
        }


        public async Task<ServiceDefault<Cert_StudentCreateDto>> CertificateStuAcceptAsync(Cert_StudentCreateDto createDto)
        {
            var result = await _certificateRepository.CertificateStuAcceptAsync(createDto);

            if (result)
            {
                return new ServiceDefault<Cert_StudentCreateDto>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Server cap chung chi thanh cong",
                    Data = createDto
                };
            }
            if (!result)
            {
                return new ServiceDefault<Cert_StudentCreateDto>
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    Message = "Server khong the cap chung chi hien tai",
                    Data = null
                };
            }

            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<bool>> CertificateStuRevokeAsync(string MaID)
        {
            var result = await _certificateRepository.CertificateStuRevokeAsync(MaID);
            if (!result)
            {
                return new ServiceDefault<bool>
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    Message = "Server khong update chung chi",
                    Data = false
                };
            }
            if (result)
            {
                return new ServiceDefault<bool>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Server update thanh cong",
                    Data = true
                };
            }
            throw new NotImplementedException();
        }
    }

}
