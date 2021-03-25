namespace MyFileSystem.Core.DTOs.FileManager.FolderDtos
{
    public class FoldersDto
    {
        public int FolderId { get; set; }
        public int? FolderParentId { get; set; }
        public string FolderName { get; set; }
    }
}
