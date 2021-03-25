using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyFileSystem.Core.DTOs;
using MyFileSystem.Core.DTOs.FileManager.FolderDtos;
using MyFileSystem.Persistence.Interfaces;
using MyFileSystem.Services.Interfaces.FileManager;
using MyFileSystem.Services.Interfaces.FileManager.Folder;
using MyFileSystem.Services.Validators;

namespace MyFileSystem.Services.FileManager.Folder
{
    public class FolderService : IFolderService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileManager  _fileManager;

        public FolderService(IMapper mapper, IUnitOfWork unitOfWork, IFileManager fileManager)
        {
            _unitOfWork = unitOfWork;
            _fileManager = fileManager;
            _mapper = mapper;
        }

        public async Task<CreateFolderDto> CreateFolders(CreateFolderDto cFolderDto)
        {
            var createFolderValidator = new CreateFolderValidator();
            if (!(await createFolderValidator.ValidateAsync(cFolderDto)).IsValid) throw new Exception("Not Valid... ");
            
            var path = _fileManager.GetRootPath() + cFolderDto.FolderName;
            
            if (cFolderDto.FolderParentId == null)
            {
                _fileManager.CreateDirectory(path);
               
                var folder = _mapper.Map<CreateFolderDto, Core.Entities.Folder>(cFolderDto);
                
                folder.FolderPath = path;
               
                _unitOfWork.FoldersRepository.Add(folder);
                await _unitOfWork.CompleteAsync();
               
                var result = _mapper.Map<Core.Entities.Folder, CreateFolderDto>(folder);
               
                return result;
            }
            else 
            {
                var pFolder = await _unitOfWork.FoldersRepository.GetById(cFolderDto.FolderParentId);
                path = pFolder.FolderPath + "\\" + cFolderDto.FolderName; 
                _fileManager.CreateDirectory(path);
               
                var folder = _mapper.Map<CreateFolderDto, Core.Entities.Folder>(cFolderDto);
                folder.FolderPath = path;
               
                _unitOfWork.FoldersRepository.Add(folder);
                await _unitOfWork.CompleteAsync();
                
                return _mapper.Map<Core.Entities.Folder, CreateFolderDto>(folder);
            }
        }

        public async Task<FolderDto> GetFolder(int id)
        {
            var folder = (await _unitOfWork.FoldersRepository
                .GetAllIncluded(f => f.FolderId == id, o => o.Files)).SingleOrDefault();
           
            var folders = await _unitOfWork.FoldersRepository.GetAll(f => f.FolderParentId == id);
            if (folder == null) throw new Exception("Not Found... ");
           
            var folderDto = _mapper.Map<Core.Entities.Folder, FolderDto>(folder);
            folderDto.Folders = _mapper.Map<List<Core.Entities.Folder>, List<FolderDto>>(folders.ToList());
           
            return folderDto;
        }

        public async Task<string> UpdateFolders(int id, [FromForm] string path2)
        {
            var folder = (await _unitOfWork.FoldersRepository.GetAllIncluded(f => f.FolderId == id, o => o.Files)).SingleOrDefault();
            if (folder == null)
                throw new Exception("Not Found... ");
            try
            {
                if (!Directory.Exists(path2))
                {
                    Directory.Move(folder.FolderPath, $"{folder.FolderPath}\\{path2}");
                    
                    await _unitOfWork.CompleteAsync();
                    
                    _mapper.Map<Core.Entities.Folder, CreateFolderDto>(folder);
                    
                    return ("Directory was Moved... ");
                }
                else throw new Exception("Not Found... ");
            }
            catch (Exception e) { throw new Exception(e.Message); }
        }

        public async Task<string> DeleteFolders(int id)
        {
            var folder = await _unitOfWork.FoldersRepository.GetById(id);
            if (folder == null)
                throw new Exception("Not Found... ");
           
            await DeleteTree(id);
            
            _unitOfWork.FoldersRepository.Delete(folder);
           
            var path = folder.FolderPath;
           
            _fileManager.DeleteDirectory(path);
            
            await _unitOfWork.CompleteAsync();
           
            return ("Folder and it's contents were deleted... ");
        }

        private async Task DeleteTree(int fId)
        {
            var folders = (await _unitOfWork.FoldersRepository.GetAllIncluded(f => f.FolderParentId == fId)).ToList();
            var firstLevelFiles = (await _unitOfWork.FileRepository.GetAllIncluded(fi => fi.FolderId == fId)).ToList();
            
            if (firstLevelFiles.Count > 0)
            {  
                await _unitOfWork.FileRepository.DeleteRange(firstLevelFiles); 
            }

            foreach (var folder in folders)
            {
                await DeleteTree(folder.FolderId);
                var files = (await _unitOfWork.FileRepository.GetAllIncluded(fi => fi.FolderId == fId)).ToList();
                
                if (files.Count > 0)
                {
                    await  _unitOfWork.FileRepository.DeleteRange(files);
                } 
                _unitOfWork.FoldersRepository.Delete(folder);
            }
        }

        public async Task<PagedResultDto<FolderDto>> GetPagedFolders(int? pageIndex, int? pageSize)
        {
            var folder = await _unitOfWork.FoldersRepository
                    .GetAllIncludedPagination(f => f.FolderName != null, pageIndex, pageSize);
           
            var result = _mapper.Map<PagedResultDto<Core.Entities.Folder>, PagedResultDto<FolderDto>>(folder);
           
            return result;
        }
    }
}
