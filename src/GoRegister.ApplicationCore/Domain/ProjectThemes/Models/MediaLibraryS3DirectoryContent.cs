using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace GoRegister.ApplicationCore.Domain.ProjectThemes.Models
{
    public class MediaLibraryS3DirectoryContent
    {
        public MediaLibraryS3DirectoryContent[] Data { get; set; }
        public bool ShowHiddenItems { get; set; }
        public string SearchString { get; set; }
        public bool CaseSensitive { get; set; }
        public IList<IFormFile> UploadFiles { get; set; }
        public string[] RenameFiles { get; set; }
        public string TargetPath { get; set; }
        public string ParentId { get; set; }
        public string FilterId { get; set; }
        public string FilterPath { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public bool IsFile { get; set; }
        public bool IsDirectory { get; set; }
        public bool HasChild { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string PreviousName { get; set; }
        public long Size { get; set; }
        public string Name { get; set; }
        public string[] Names { get; set; }
        public string NewName { get; set; }
        public string Action { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }

        public string AbsoluteUrl { get; set; }
        public MediaLibraryS3DirectoryContent TargetData { get; set; }

    }
}
