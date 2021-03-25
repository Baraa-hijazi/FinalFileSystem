using Microsoft.AspNetCore.Http;

namespace MyFileSystem.Core.DTOs.FileManager.FileDtos
{
    public class CreateFileDto
    {
        public int FolderId { get; set; }
        public IFormFile PhFile { get; set; }
    }
}
