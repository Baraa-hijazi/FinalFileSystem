using System.Collections.Generic;
using AutoMapper;
using MyFileSystem.Core.DTOs;
using MyFileSystem.Core.DTOs.Account;
using MyFileSystem.Core.DTOs.FileManager.FileDtos;
using MyFileSystem.Core.DTOs.FileManager.FolderDtos;
using MyFileSystem.Core.Entities;

namespace MyFileSystem.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Folder, FolderParentDto>().ReverseMap();
            CreateMap<Folder, FoldersDto>().ReverseMap();

            CreateMap(typeof(PagedResultDto<>), typeof(PagedResultDto<>));
            CreateMap<PagedResultDto<ApplicationUser>, PagedResultDto<LoginDto>>();

            CreateMap<Folder, FolderDto>().ReverseMap()
                .ForMember(f => f.FolderId, opt => opt.Ignore());

            CreateMap<Folder, List<FolderDto>>().ReverseMap()
                .ForMember(f => f.FolderId, opt => opt.Ignore());

            CreateMap<File, FileDto>().ReverseMap()
                .ForMember(f => f.FileId, opt => opt.Ignore());

            CreateMap<Folder, CreateFolderDto>().ReverseMap()
                .ForMember(f => f.FolderId, opt => opt.Ignore());

            CreateMap<File, UpdateFileDto>().ReverseMap()
                .ForMember(f => f.FileId, opt => opt.Ignore());

            CreateMap<ApplicationUser, LoginDto>();
            CreateMap<LoginDto, ApplicationUser>();
        }
    }
}
