using System.Collections.Generic;
using MyFileSystem.Core.DTOs.FileManager.FileDtos;

namespace MyFileSystem.Core.DTOs.FileManager.FolderDtos
{
    public class FolderParentDto 
    {
        public int FolderId { get; set; }
        public int? FolderParentId { get; set; }
        public string FolderName { get; set; }
        public ICollection<FileDto> Files { get; set; }
        public FolderParentDto FolderParent { get; set; }
    }
}
