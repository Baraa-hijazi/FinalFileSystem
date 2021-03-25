using System.Threading.Tasks;
using MyFileSystem.Core.DTOs;
using MyFileSystem.Core.DTOs.FileManager.FileDtos;

namespace MyFileSystem.Services.Interfaces.FileManager.File
{
    public interface IFileService
    {
        Task<PagedResultDto<FileDto>> GetFiles(int? pageIndex, int? pageSize);
        Task<FileDto> GetFile(int id);
        Task<FileDto> UploadFile(CreateFileDto createFileDto);
        Task<string> UpdateFiles(int id, UpdateFileDto createFileDto);
        Task<string> DeleteFiles(int id);
    }
}
