using GoRegister.ApplicationCore.Domain.ProjectThemes.Commands;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Queries;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Models;
using GoRegister.ApplicationCore.Framework.Notifications;
using GoRegister.ApplicationCore.Framework;
using Microsoft.AspNetCore.Hosting;
using GoRegister.ApplicationCore.Services.FileStorage;

namespace GoRegister.Areas.Admin.Controllers
{
    public class ProjectThemeController : ApiProjectAdminControllerBase
    {
        private readonly IProjectThemeService _projectThemeService;
        protected readonly IHostingEnvironment HostingEnvironment;
        private readonly FileContentBrowser directoryBrowser;

        private readonly ProjectTenant _projectTenant;
        private readonly INotifier _notifier;

        public ProjectThemeController(INotifier notifier, IHostingEnvironment hostingEnvironment, IProjectThemeService projectThemeService, ProjectTenant projectTenant)
        {
            _projectThemeService = projectThemeService;

            _projectTenant = projectTenant;
           
            _notifier = notifier;
            HostingEnvironment = hostingEnvironment;
            directoryBrowser = new FileContentBrowser();
        }

        public async Task<IActionResult> Get() => Ok(await Mediator.Send(new ProjectThemeEditQuery()));

        [HttpPost]
        public async Task<IActionResult> UploadLogo(IFormFile file)
        {
            var result = await _projectThemeService.ProjectThemeUpload(file, Guid.Empty);
            return Ok(new { LogoUrl = result });
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProjectThemeEditCommand model)
        {
            await Mediator.Send(model);
            return Ok();
        }

        //[HttpPost]
        //public async Task<IActionResult> DeleteLogo(IFormFile file)
        //{
        //    var result = await _projectThemeService.ProjectThemeUpload(file, Guid.Empty);
        //    return Ok(new { LogoUrl = result });
        //}

        [HttpPost]
        public async Task<IActionResult> DeleteLogo([FromBody] ProjectThemeModel model)
        {
            try
            {
                var url = new Uri(model.LogoUrl);
                string filename = System.IO.Path.GetFileName(url.LocalPath);
                string filextn = System.IO.Path.GetExtension(url.LocalPath);
               
                var result = await _projectThemeService.DeleteFile(filename, filextn);
                
                //return Json(Url.Action("Index", "ProjectAssets"));
                return Ok(new { LogoUrl = result });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //public async Task<IActionResult> Index(string flag)
        //{
        //    try
        //    {
        //        if (flag == "delete")
        //        {
        //            _notifier.Success("File deleted successfully");
        //        }
        //        else if (flag == "upload")
        //        {
        //            _notifier.Success("File uploaded successfully");
        //        }

        //        var s3filedata = await _projectThemeService.GetFilesList();

        //        var files = directoryBrowser.GetAwsFiles(s3filedata);

        //        return View(files);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }

        //}
    }

    public class FileContentBrowser
    {

        public IEnumerable<FileManagerEntryModel> GetAwsFiles(IEnumerable<MediaLibraryS3DirectoryContent> filesdata)
        {

            return filesdata.Select(file => new FileManagerEntryModel
            {
                Name = file.Name,
                Size = file.Size,
                Path = file.Path,
                Extension = file.Extension,
                IsDirectory = file.IsDirectory,
                HasDirectories = file.HasChild,
                Created = file.DateCreated,
                CreatedUtc = file.DateCreated,
                Modified = file.DateModified,
                ModifiedUtc = file.DateModified
            });

        }

        public IEnumerable<FileManagerEntryModel> GetAwsDirectories(IEnumerable<ProjectAssetS3DirectoryContent> dirsdata)
        {
            return dirsdata.Select(subDirectory => new FileManagerEntryModel
            {
                Name = subDirectory.Name,
                Path = subDirectory.Path,
                Extension = subDirectory.Type,
                IsDirectory = true,
                HasDirectories = subDirectory.HasChild,
                Created = subDirectory.DateCreated,
                CreatedUtc = subDirectory.DateCreated,
                Modified = subDirectory.DateModified,
                ModifiedUtc = subDirectory.DateModified
            });
        }




    }
}
