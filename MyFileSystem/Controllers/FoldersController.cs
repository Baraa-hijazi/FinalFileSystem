using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFileSystem.Core.DTOs;
using MyFileSystem.Services.Interfaces.Folder;
using System.Threading.Tasks;

namespace MyFileSystem.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class FoldersController : BaseController
    {
        private readonly IFolderService _folderService;

        public FoldersController(IFolderService folderService)
        { 
            _folderService = folderService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateFolders([FromBody] CreateFolderDto cFolderDto) => Ok(await _folderService.CreateFolders(cFolderDto));

        [HttpGet("Get-By-Id")]
        public async Task<IActionResult> GetFolders(int id) => Ok(await _folderService.GetFolder(id));

        //[HttpGet]
        //public async Task<IActionResult> GetFolders() => Ok(await _folderService.GetFolders());

        [HttpGet("Get-All-Paged")]
        public async Task<IActionResult> GetPagedFolders([FromQuery] int? pageIndex, [FromQuery] int? pageSize) => Ok(await _folderService.GetPagedFolders(pageIndex, pageSize));
    
        [HttpPut("Edit")]
        public async Task<IActionResult> UpdateFolders(int id, [FromForm] string path2) => Ok(await _folderService.UpdateFolders(id, path2));

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteFolders(int id) => Ok(await _folderService.DeleteFolders(id));
    } 
}