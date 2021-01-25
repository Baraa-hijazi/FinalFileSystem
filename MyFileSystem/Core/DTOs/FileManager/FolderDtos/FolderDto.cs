using System.Collections.Generic;

namespace MyFileSystem.Core.DTOs
{
    public class FolderDto
    {
        public int FolderId { get; set; }
        public int? FolderParentId { get; set; }
        public string FolderName { get; set; }
        public ICollection<FileDto> Files { get; set; }
        public ICollection<FolderDto> Folders { get; set; }
    }
}
