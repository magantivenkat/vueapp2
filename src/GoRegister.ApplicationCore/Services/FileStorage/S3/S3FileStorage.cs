using GoRegister.ApplicationCore.Domain.ProjectThemes.Models;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Logging;


namespace GoRegister.ApplicationCore.Services.FileStorage.S3
{
    public class S3FileStorage : IFileStorage
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonS3 _s3Client;
        protected static string bucketName;
        public string RootName;
        private readonly ILogger<S3FileStorage> _logger;

        public S3FileStorage(IConfiguration configuration, IAmazonS3 s3Client, ILogger<S3FileStorage> logger)
        {
            _configuration = configuration;
            _s3Client = s3Client;
            _logger = logger;
        }

        public async Task<FileStorageUploadResult> UploadFile(string path, Stream stream, UploadFileStorageSettings settings = null)
        {
            if (settings == null)
            {
                settings = new UploadFileStorageSettings();
            }

            var s3Settings = _configuration.GetSection("S3Settings").Get<S3Settings>();

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = path,
                BucketName = s3Settings.BucketName,
                CannedACL = settings.IsPrivate ? S3CannedACL.Private : S3CannedACL.PublicReadWrite
            };

            using (var fileTransferUtility = new TransferUtility(_s3Client))
            {
                try
                {
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }
                catch (Exception ex)
                {
                }
            }

            //var url = $"https://{s3Settings.BucketName}.{s3Settings.RegionDetail}/{path}";

            var url = $"https://{s3Settings.BucketMapURL}/{path}";

            return new FileStorageUploadResult
            {
                Path = path,
                AbsoluteUrl = url
            };
        }

        public async Task<Stream> ReadFile(string key)
        {
            var s3Settings = _configuration.GetSection("S3Settings").Get<S3Settings>();

            var objectRequest = new GetObjectRequest
            {
                Key = key,
                BucketName = s3Settings.BucketName,
            };

            GetObjectResponse response = await _s3Client.GetObjectAsync(objectRequest);

            return response.ResponseStream;
        }

        //public async Task<FileStorageDeleteResult> DeleteFile(string path, string filename, string filextn)
        //{
        //    try
        //    {
        //        var s3Settings = _configuration.GetSection("S3Settings").Get<S3Settings>();
        //        bucketName = s3Settings.BucketName;
        //        var awsFile = filename + filextn;
        //        DeleteObjectsRequest deleteObjectsRequest = new DeleteObjectsRequest() { BucketName = bucketName };

        //        ListObjectsRequest listObjectsRequest = new ListObjectsRequest { BucketName = bucketName, Prefix = path + "/" + awsFile };
        //        ListObjectsResponse listObjectsResponse = await _s3Client.ListObjectsAsync(listObjectsRequest);
        //        foreach (S3Object s3Object in listObjectsResponse.S3Objects) { deleteObjectsRequest.AddKey(s3Object.Key); }

        //        await _s3Client.DeleteObjectsAsync(deleteObjectsRequest);

        //        string tempfile = Path.Combine(Path.GetTempPath(), awsFile);
        //        if (System.IO.File.Exists(tempfile)) System.IO.File.Delete(tempfile); else if (Directory.Exists(tempfile)) Directory.Delete(tempfile, true);

        //        var url = $"https://{s3Settings.BucketName}.{s3Settings.RegionDetail}/{path}";

        //        return new FileStorageDeleteResult
        //        {
        //            Path = path,
        //            AbsoluteUrl = url
        //        };

        //    }
        //    catch (AmazonS3Exception amazonS3Exception) { throw amazonS3Exception; }
        //}


        public async Task<FileStorageDeleteResult> DeleteFile(string path, string filename, string filextn)
        {
            try
            {
                //var s3Settings = _configuration.GetSection("S3Settings").Get<S3Settings>();
                //bucketName = s3Settings.BucketName;
                var awsFile = filename;


                var s3Settings = _configuration.GetSection("S3Settings").Get<S3Settings>();
                var s3Path = path;
                bucketName = s3Settings.BucketName;
                var credentials = new BasicAWSCredentials(s3Settings.AccessKey, s3Settings.SecretKey);
                var config = new AmazonS3Config
                {
                    RegionEndpoint = Amazon.RegionEndpoint.EUWest1
                };

                //var client = new AmazonS3Client(credentials, config);
                var client = new AmazonS3Client(config);


                DeleteObjectsRequest deleteObjectsRequest = new DeleteObjectsRequest() { BucketName = bucketName };

                ListObjectsRequest listObjectsRequest = new ListObjectsRequest { BucketName = bucketName, Prefix = path + "/" + awsFile };
                ListObjectsResponse listObjectsResponse = await client.ListObjectsAsync(listObjectsRequest);
                foreach (S3Object s3Object in listObjectsResponse.S3Objects)
                {
                    deleteObjectsRequest.AddKey(s3Object.Key);
                }

                await client.DeleteObjectsAsync(deleteObjectsRequest);
                _logger.LogError("Deletelogo Successfully1: " + deleteObjectsRequest, deleteObjectsRequest);

                string tempfile = Path.Combine(Path.GetTempPath(), awsFile);
                if (System.IO.File.Exists(tempfile)) System.IO.File.Delete(tempfile); else if (Directory.Exists(tempfile)) Directory.Delete(tempfile, true);

                var url = $"https://{s3Settings.BucketName}.{s3Settings.RegionDetail}/{path}";

                return new FileStorageDeleteResult
                {
                    Path = path,
                    AbsoluteUrl = url
                };

            }
            catch (AmazonS3Exception amazonS3Exception) {
                _logger.LogError("Deletelogo InnerException: " + amazonS3Exception.InnerException, amazonS3Exception.InnerException);
                _logger.LogError("Deletelogo ExceptionMessgae: " + amazonS3Exception.Message, amazonS3Exception.Message);
                throw amazonS3Exception; 
            }
        }


        public async Task<List<MediaLibraryS3DirectoryContent>> GetFilesList(string path)
        {
            try
            {

                List<MediaLibraryS3DirectoryContent> filesS3 = new List<MediaLibraryS3DirectoryContent>();
                string fileName = string.Empty;
                string urlPath = string.Empty;

                var s3Path = path;
                var s3Settings = _configuration.GetSection("S3Settings").Get<S3Settings>();

                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = s3Settings.BucketName,
                    Prefix = s3Path
                };

                ListObjectsV2Response response;
                do
                {
                    response = await _s3Client.ListObjectsV2Async(request);
                    RootName = response.S3Objects.First().Key;
                    urlPath = $"https://{s3Settings.BucketName}.{s3Settings.RegionDetail}/{s3Path}/";

                    try
                    {
                        if (response.S3Objects.Count > 0)
                            filesS3 = response.S3Objects.Where(x => x.Key != RootName.Replace("/", "") + path).Select(y => CreateDirectoryContentInstance(y.Key.ToString().Replace(path + "/", ""), false, Path.GetExtension(y.Key.ToString()), y.Size, y.LastModified, y.LastModified, false, getFilterPath(y.Key, path), urlPath + y.Key.ToString().Replace(path + "/", ""))).ToList();
                    }
                    catch (Exception ex) { throw ex; }

                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated);

                return filesS3;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private MediaLibraryS3DirectoryContent CreateDirectoryContentInstance(string name, bool value, string type, long size, DateTime createddate, DateTime modifieddate, bool child, string filterpath, string itemurl = "")
        {
            MediaLibraryS3DirectoryContent tempFile = new MediaLibraryS3DirectoryContent();

            if (Path.GetExtension(name) != "")
            {
                tempFile.Name = Path.GetFileNameWithoutExtension(name);
                tempFile.IsDirectory = false;
                tempFile.Extension = Path.GetExtension(name);
            }
            else
            {
                tempFile.Name = name;
                tempFile.IsDirectory = true;
                tempFile.Extension = "";
            }

            tempFile.Type = type;
            tempFile.Size = size;
            tempFile.DateCreated = createddate;
            tempFile.DateModified = modifieddate;
            tempFile.HasChild = child;
            tempFile.FilterPath = filterpath;
            tempFile.AbsoluteUrl = itemurl;
            tempFile.Path = itemurl;

            return tempFile;
        }

        public virtual FileStreamResult GetFileFromAws(string path, string filename, string filextn)
        {
            try
            {
                var s3Settings = _configuration.GetSection("S3Settings").Get<S3Settings>();
                var s3Path = path;
                string fileName = path.ToString().Split("/").Last();

                var awsfile = filename + filextn;
                var fileTransferUtility = new TransferUtility(_s3Client);
                var url = $"https://{s3Settings.BucketName}.{s3Settings.RegionDetail}/{path}/{awsfile}";
                Stream stream = fileTransferUtility.OpenStream(s3Settings.BucketName, path + "/" + awsfile);

                //return stream;
                return new FileStreamResult(stream, new MediaTypeHeaderValue("APPLICATION/octet-stream"))
                {
                    FileDownloadName = awsfile

                };

            }
            catch (Exception ex) { throw ex; }
        }

        public string getFilterPath(string fullPath, string path)
        {

            string name = fullPath.ToString();
            int nameIndex = fullPath.LastIndexOf(name);
            if (nameIndex > 0)
            {
                fullPath = fullPath.Substring(0, nameIndex);
                int rootIndex = fullPath.IndexOf(RootName.Substring(0, RootName.Length - 1));
                fullPath = fullPath.Substring(rootIndex + RootName.Length - 1);
            }
            return fullPath;
        }
    }
}
