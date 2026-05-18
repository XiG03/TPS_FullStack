using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace TPS_FullStack.Server.Modules.Admin
{
    [Route("api/admin/certificate")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificateService _certificateService;
        public CertificateController (ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        [HttpGet]
        public async Task<IActionResult> CertificateGetAll()
        {
            var result = await _certificateService.CertificateGetAllAsync();
            
            return StatusCode(result.statusCode, result);
        }

        [HttpGet("{MaID}")]
        public async Task<IActionResult> CertificateGetById(string MaID)
        {
            var result = await _certificateService.CertificateFindByIdAsync(MaID);
            return StatusCode(result.statusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> CertificateInsert(CertificateCreateDto createDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _certificateService.CertificateInsertAsync(createDTO);
            return StatusCode(result.statusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> CertificateUpdate(CertificateUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _certificateService.CertificateUpdateAsync(updateDto);
            return StatusCode(result.statusCode, result);
        }

        [HttpDelete("{MaID}")]
        public async Task<IActionResult> CertificateDelete (string MaID)
        {
            var result = await _certificateService.CertificateDeleteAsync(MaID);
            return StatusCode(result.statusCode, result);
        }

        [HttpPost("accept")]
        public async Task<IActionResult> CertificateStuAccept(Cert_StudentCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _certificateService.CertificateStuAcceptAsync(createDto);
            return StatusCode(result.statusCode, result);
        }

        [HttpPut("revoke/{MaID}")]
        public async Task<IActionResult> CertificateStuRevoke(string MaID)
        {
            var result = await _certificateService.CertificateStuRevokeAsync(MaID);
            return StatusCode(result.statusCode, result);
        }
    }
}
