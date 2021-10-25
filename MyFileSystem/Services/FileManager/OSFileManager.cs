using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MyFileSystem.Services.Interfaces.FileManager;

namespace MyFileSystem.Services.FileManager
{
    public class OsFileManager : IFileManager
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OsFileManager( IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        
        public void CreateDirectory(string path)
        {
            if (Directory.Exists(path)) throw new Exception("Directory Already Exits... ");
            Directory.CreateDirectory(path);
        }
        
        public void DeleteDirectory(string path)
        {
            if (!Directory.Exists(path)) throw new Exception("Directory Doesn't Exits... ");
            Directory.Delete(path, true);
        }
        
        public async void UploadFile(IFormFile file, string path)
        {
            var stream = System.IO.File.Create(path);
            await file.CopyToAsync(stream);
            stream.Flush();
        }
        
        public void DeleteFile(string path)
        {
            System.IO.File.Delete(path);
        }
        
        public string GetRootPath()
        {
            return _webHostEnvironment.WebRootPath + "\\";
        }
    }
}
