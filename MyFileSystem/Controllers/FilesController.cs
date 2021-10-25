using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MyFileSystem.Core.DTOs.FileManager.FileDtos;
using MyFileSystem.Services.Interfaces.FileManager.File;
using IActionResult = Microsoft.AspNetCore.Mvc.IActionResult;

namespace MyFileSystem.Controllers
{
    [Route("API/[Controller]")]
    [ApiController]
    public class FilesController : BaseController
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile([FromForm] CreateFileDto createFileDto) => 
            Ok(await _fileService.UploadFile(createFileDto));

        [HttpGet("GetFiles")]
        public async Task<IActionResult> GetFiles(int? pageIndex, int? pageSize) => 
            Ok(await _fileService.GetFiles(pageIndex, pageSize));

        [HttpGet("Get-GetFile-Id")]
        public async Task<IActionResult> GetFile(int id) => 
            Ok(await _fileService.GetFile(id));

        [HttpPut("UpdateFile")]
        public async Task<IActionResult> UpdateFiles(int id, [FromBody] UpdateFileDto updateFileDto) => 
            Ok(await _fileService.UpdateFile(id, updateFileDto));

        [HttpDelete("DeleteFile")]
        public async Task<IActionResult> DeleteFiles(int id) => 
            Ok(await _fileService.DeleteFile(id));
    }
}