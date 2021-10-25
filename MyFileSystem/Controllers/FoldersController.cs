using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MyFileSystem.Core.DTOs.FileManager.FolderDtos;
using MyFileSystem.Services.Interfaces.FileManager.Folder;

namespace MyFileSystem.Controllers
{
    [Route("API/[Controller]")]
    [ApiController]
    public class FoldersController : BaseController
    {
        private readonly IFolderService _folderService;

        public FoldersController(IFolderService folderService)
        { 
            _folderService = folderService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateFolders([FromBody] CreateFolderDto cFolderDto) =>
            Ok(await _folderService.CreateFolder(cFolderDto));

        [HttpGet("Get-By-Id")]
        public async Task<IActionResult> GetFolders(int id) => 
            Ok(await _folderService.GetFolder(id));

        [HttpGet("GetPagedFolders")]
        public async Task<IActionResult> GetPagedFolders([FromQuery] int? pageIndex, [FromQuery] int? pageSize) => 
            Ok(await _folderService.GetPagedFolders(pageIndex, pageSize));
    
        [HttpPut("Edit")]
        public async Task<IActionResult> UpdateFolders(int id, [FromForm] string path2) => 
            Ok(await _folderService.UpdateFolder(id, path2));

        [HttpDelete("DeleteFolder")]
        public async Task<IActionResult> DeleteFolder(int id) =>
            Ok(await _folderService.DeleteFolder(id));
    } 
}