﻿namespace MyFileSystem.Core.Entities
{
    public class File
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public int? FileSize { get; set; }
        public int? FolderId { get; set; }
        public string FilePath { get; set; }
        public Folder Folder { get; set; }
    }
}
