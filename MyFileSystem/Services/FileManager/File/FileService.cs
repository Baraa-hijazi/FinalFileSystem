using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyFileSystem.Core.DTOs;
using MyFileSystem.Core.DTOs.FileManager.FileDtos;
using MyFileSystem.Persistence.Interfaces;
using MyFileSystem.Services.Interfaces.FileManager;
using MyFileSystem.Services.Interfaces.FileManager.File;
using MyFileSystem.Services.Validators;

namespace MyFileSystem.Services.FileManager.File
{
    public class FileService : IFileService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileManager _fileManager;

        public FileService(IMapper mapper, IUnitOfWork unitOfWork, IFileManager fileManager)
        {
            _unitOfWork = unitOfWork;
            _fileManager = fileManager;
            _mapper = mapper;
        }

        public async Task<PagedResultDto<FileDto>> GetFiles(int? pageIndex, int? pageSize)
        {
            var files = await _unitOfWork.FileRepository
                .GetAllIncludedPagination(f => f.FileName != null, pageIndex, pageSize);

            return _mapper.Map<PagedResultDto<Core.Entities.File>, PagedResultDto<FileDto>>(files);
        }

        public async Task<FileDto> GetFile(int id)
        {
            var file = await _unitOfWork.FileRepository.GetById(id);
            if (file == null) throw new Exception("Not Found... ");

            return _mapper.Map<Core.Entities.File, FileDto>(file);
        }

        public async Task<FileDto> UploadFile([FromForm] CreateFileDto createFileDto)
        {
            var createFileValidator = new CreateFileValidator();
            if (!(await createFileValidator.ValidateAsync(createFileDto.PhFile)).IsValid)
                throw new Exception("Name Not Valid... ");

            //--------------- Upload Physical File ------------------//
            string path;
            if (createFileDto.FolderId > 0)
            {
                var rootFolder =
                    (await _unitOfWork.FoldersRepository.GetAllIncluded(f => f.FolderId == createFileDto.FolderId))
                    .SingleOrDefault();
                if (rootFolder == null)
                {
                    throw new Exception("Folder not found");
                }

                path = $"{rootFolder.FolderPath}\\{createFileDto.PhFile.FileName}";
            }
            else
            {
                path = _fileManager.GetRootPath() + createFileDto.PhFile.FileName;
            }

            _fileManager.UploadFile(createFileDto.PhFile, path);

            //----------------------- Save to Db --------------------//
            var entityFile = new Core.Entities.File
            {
                FileName = Path.GetFileNameWithoutExtension(path),
                FileExtension = Path.GetExtension(createFileDto.PhFile.FileName),
                FileSize = int.Parse(createFileDto.PhFile.Length.ToString()),
                FilePath = Path.GetFullPath(path),
                FolderId = createFileDto.FolderId > 0 ? createFileDto.FolderId : default(int?)
            };

            _unitOfWork.FileRepository.Add(entityFile);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<Core.Entities.File, FileDto>(entityFile);
        }

        public async Task<string> UpdateFile(int id, [FromBody] UpdateFileDto updateFileDto)
        {
            var file = await _unitOfWork.FileRepository.GetById(id);
            if (file == null) throw new Exception("Not Found... ");

            file = _mapper.Map(updateFileDto, file);

            await _unitOfWork.CompleteAsync();

            _mapper.Map<Core.Entities.File, UpdateFileDto>(file);

            return "File was updated... ";
        }

        public async Task<string> DeleteFile(int id)
        {
            var file = await _unitOfWork.FileRepository.GetById(id);
            if (file == null) throw new Exception("Not Found... ");

            var path = file.FilePath;

            _fileManager.DeleteFile(path);

            _unitOfWork.FileRepository.Delete(file);
            await _unitOfWork.CompleteAsync();

            return "File was deleted... ";
        }
    }
}