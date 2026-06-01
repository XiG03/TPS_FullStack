using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace TPS_FullStack.Server.Modules.Admin
{
    [Route("api/v1/lab")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class LabController : ControllerBase
    {
        private readonly ILabService _labService;
        public LabController (ILabService labService)
        {
            _labService = labService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLab(LabInsert labInsert)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _labService.CreateLabAsync(labInsert);
            return StatusCode(result.statusCode, result);
        }
    }
}
