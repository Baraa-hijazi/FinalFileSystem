using Microsoft.AspNetCore.Mvc;
using MyFileSystem.Core.DTOs.Account;
using MyFileSystem.Services.Interfaces.Account;
using System.Threading.Tasks;

namespace MyFileSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private IAdministrationServices _administrationService { get; }

        public AdministrationController(IAdministrationServices administrationService)
        {
            _administrationService = administrationService;
        }

        [HttpGet("Get-All")]
        public IActionResult Get() => 
            Ok(_administrationService.Get());

        [HttpGet("Get-By-Id")]
        public async Task<IActionResult> Get(string id) => 
            Ok(await _administrationService.Get(id));

        [HttpPost("Create-Role")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto) => 
            Ok(await _administrationService.CreateRole(createRoleDto));

        [HttpPut("Edit-Role")]
        public async Task<IActionResult> Put(string id, [FromBody] CreateRoleDto createRoleDto) => 
            Ok(await _administrationService.Put(id, createRoleDto));

        [HttpDelete("Delete-Role")]
        public async Task<IActionResult> Delete(string id) => 
            Ok(await _administrationService.Delete(id));
    }
}
