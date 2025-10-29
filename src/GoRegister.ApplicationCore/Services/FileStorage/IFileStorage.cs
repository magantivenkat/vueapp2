using GoRegister.ApplicationCore.Domain.ProjectThemes.Models;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace GoRegister.ApplicationCore.Services.FileStorage
{
    public interface IFileStorage
    {
        Task<FileStorageUploadResult> UploadFile(string path, Stream stream, UploadFileStorageSettings settings = null);

        Task<Stream> ReadFile(string key);
        Task<List<MediaLibraryS3DirectoryContent>> GetFilesList(string path);

        FileStreamResult GetFileFromAws(string path, string filename, string filextn);
        Task<FileStorageDeleteResult> DeleteFile(string path, string filename, string filextn);
    }
}
