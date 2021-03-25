using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFileSystem.Core.DTOs;
using MyFileSystem.Core.DTOs.FileManager.FolderDtos;

namespace MyFileSystem.Services.Interfaces.FileManager.Folder
{
    public interface IFolderService
    {
        Task<CreateFolderDto> CreateFolders(CreateFolderDto cFolderDto);
        Task<FolderDto> GetFolder(int id); 
        Task<string> DeleteFolders(int id);
        Task<string> UpdateFolders(int id, [FromForm] string path2);
        Task<PagedResultDto<FolderDto>> GetPagedFolders(int? pageIndex, int? pageSize);
    }
}
