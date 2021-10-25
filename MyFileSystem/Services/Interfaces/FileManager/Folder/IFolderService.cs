using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFileSystem.Core.DTOs;
using MyFileSystem.Core.DTOs.FileManager.FolderDtos;

namespace MyFileSystem.Services.Interfaces.FileManager.Folder
{
    public interface IFolderService
    {
        Task<CreateFolderDto> CreateFolder(CreateFolderDto cFolderDto);
        Task<FolderDto> GetFolder(int id); 
        Task<string> DeleteFolder(int id);
        Task<string> UpdateFolder(int id, [FromForm] string path2);
        Task<PagedResultDto<FolderDto>> GetPagedFolders(int? pageIndex, int? pageSize);
    }
}
