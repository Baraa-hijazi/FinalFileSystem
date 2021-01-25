using Microsoft.AspNetCore.Mvc;
using MyFileSystem.Core.DTOs.Account;
using MyFileSystem.Services.Interfaces.Account;
using System.Threading.Tasks;

namespace MyFileSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminstrationController : ControllerBase
    {
        public IAdminstrationServices AdminstrationServices { get; }

        public AdminstrationController(IAdminstrationServices adminstrationServices)
        {
            AdminstrationServices = adminstrationServices;
        }

        [HttpGet("Get-All")]
        public IActionResult Get() => Ok(AdminstrationServices.Get());

        [HttpGet("Get-By-Id")]
        public async Task<IActionResult> Get(string id) => Ok(await AdminstrationServices.Get(id));

        [HttpPost("Create-Role")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto) => Ok(await AdminstrationServices.CreateRole(createRoleDto));

        [HttpPut("Edit-Role")]
        public async Task<IActionResult> Put(string id, [FromBody] CreateRoleDto createRoleDto) => Ok(await AdminstrationServices.Put(id, createRoleDto));

        [HttpDelete("Delete-Role")]
        public async Task<IActionResult> Delete(string id) => Ok(await AdminstrationServices.Delete(id));
    }
}
