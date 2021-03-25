using Microsoft.AspNetCore.Http;

namespace MyFileSystem.Services.Interfaces.FileManager
{
    public interface IFileManager
    {
        string GetRootPath();
        void CreateDirectory(string path);
        void DeleteDirectory(string path);
        void UploadFile(IFormFile file, string path);
        void DeleteFile(string path);
    }
}
