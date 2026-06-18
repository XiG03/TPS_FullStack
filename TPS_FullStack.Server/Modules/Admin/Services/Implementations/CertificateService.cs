using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using TPS_FullStack.Server.Entities;
using TPS_FullStack.Server.Helpers;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class CertificateService : ICertificateService
    {
        private readonly ICertificateRepository _certificateRepository;
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<CertificateService> _logger;
        public CertificateService(ICertificateRepository certificateRepository, IEmailService emailService, UserManager<AppUser> userManager,
                                    ILogger<CertificateService> logger)
        {
            _certificateRepository = certificateRepository;
            _emailService = emailService;
            _userManager = userManager;
            _logger = logger;
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
            if (result == null)
            {
                return new ServiceDefault<CertificateDetailDto>
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    Message = "Server khong tim thay chung chi",
                    Data = null
                };
            }
            if (result != null)
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
            if (result == null)
            {
                return new ServiceDefault<List<CertificateGetAllDto>>
                {
                    statusCode = StatusCodes.Status500InternalServerError,
                    Message = "Server khong tim thay danh sach chung chi",
                    Data = null
                };
            }
            if (result != null)
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
            createDto.Ngaycap = DateTime.UtcNow;
            createDto.Ngayhethan = createDto.Ngaycap.Value.AddMonths((int)createDto.Thoigiansudung);
            var result = await _certificateRepository.CertificateStuAcceptAsync(createDto);

            if (result)
            {
                var body = await _emailService.RenderAsync("NotifyCertification", new Dictionary<string, string>
                {
                    ["FullName"] = createDto.HocvienID,
                    ["CertificateName"] = createDto.ChungchiID,
                    ["Duration"] = createDto.Thoigiansudung.ToString(),
                    ["StartDate"] = createDto.Ngaycap.ToString(),
                    ["EndDate"] = createDto.Ngayhethan.ToString(),
                    ["CenterName"] = createDto.Donvicap
                });

                var stuEmail = await _userManager.FindByIdAsync(createDto.HocvienID);
                if (stuEmail == null)
                {
                    _logger.LogError("Email is null");
                }
                await _emailService.SendEmailAsync(stuEmail.Email, "Thông báo đã được cấp chứng chỉ", body);

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
