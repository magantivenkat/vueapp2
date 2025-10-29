using AutoMapper;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Models;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using GoRegister.ApplicationCore.Extensions;
using GoRegister.ApplicationCore.Services.FileStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.ProjectThemes.Services
{
    public interface IProjectThemeService
    {
        Task<int> SaveProjectThemeAsync(ProjectThemeModel model);
        Task<ProjectThemeModel> SaveGlobalProjectThemeAsync(ProjectThemeModel model);
        Task<ProjectThemeModel> GetProjectThemeByProjectIdAsync(int? projectId);
        ProjectThemeModel GetProjectThemeByGuid(Guid themeGuid);
        Task<IEnumerable<ProjectThemeListItemModel>> GetList();
        IEnumerable<ProjectThemeListItemModel> GetGlobalThemeList();
        ProjectThemeModel GetProjectThemeById(int id);
        Task<ProjectThemeModel> GetTheme(int id);
        IEnumerable<ProjectThemeListItemModel> GetProjectThemeByGuidAll(Guid themeGuid);
        IEnumerable<ProjectThemeListItemModel> GetProjectThemeByProjectIdAll(int projectId);
        void ArchiveGlobalTheme(Guid themeGuid);
        void UnarchiveGlobalTheme(Guid themeGuid);
        IEnumerable<ProjectThemeListItemModel> GetArchivedGlobalThemes();
        string GetThemeLayoutByProjectId(int id);
        List<SelectListItem> GetLayoutOptions();
        IEnumerable<ProjectThemeListItemModel> GetClientThemes(int clientId);
        Task<string> ProjectThemeUpload(IFormFile file, Guid themeGuid);
        Task<ProjectTheme> GetDefaultProjectTheme();
      

        Task<string> DeleteFile(string filename, string filextn);

        Task<IEnumerable<MediaLibraryS3DirectoryContent>> GetFilesList();

        Task<FileStreamResult> GetFile(string filename, string filextn);
    }

    public class ProjectThemeService : IProjectThemeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IFileStorage _fileStorage;
        private readonly IProjectSettingsAccessor _projectSettingsAccessor;

        public ProjectThemeService(ApplicationDbContext context, IMapper mapper, IConfiguration configuration, IFileStorage fileStorage, IProjectSettingsAccessor projectSettingsAccessor)
        {
            _context = context;
            this._mapper = mapper;
            _configuration = configuration;
            _fileStorage = fileStorage;
            _projectSettingsAccessor = projectSettingsAccessor;
        }

        public async Task<IEnumerable<ProjectThemeListItemModel>> GetList()
        {
            var items = await _context.ProjectThemes.ToListAsync();
            var models = _mapper.Map<IEnumerable<ProjectThemeListItemModel>>(items);

            return models;
        }

        public IEnumerable<ProjectThemeListItemModel> GetGlobalThemeList()
        {
            var items = _context.ProjectThemes.Where(t => t.ProjectId == null && t.IsArchived == false && t.ClientId == null).OrderByDescending(d => d.DateUpdated).ToList().GroupBy(n => n.Name).Select(s => s.First()).OrderBy(n => n.Name);
            var projectThemes = _mapper.Map<IEnumerable<ProjectThemeListItemModel>>(items);

            return projectThemes;
        }

        public async Task<int> SaveProjectThemeAsync(ProjectThemeModel model)
        {

            var projectTheme = new ProjectTheme
            {
                Name = model.Name,
                ThemeGuid = model.ThemeGuid,
                ProjectId = model.ProjectId,
                DateUpdated = DateTime.Now,
                LayoutName = model.LayoutName,
                HeaderHtml = model.HeaderHtml,
                FooterHtml = model.FooterHtml,
                ThemeVariables = model.ThemeVariables ?? _configuration.GetValue<string>("Themes:ThemeVariables"),
                ThemeVariableObject = model.ThemeVariableObject ?? _configuration.GetValue<string>("Themes:ThemeVariableObject"),
                ThemeCss = model.ThemeCss,
                OverrideCss = model.OverrideCss,
                HeadScripts = model.HeadScripts,
                FooterScripts = model.FooterScripts,
                ThemeUniqueId = model.ThemeUniqueId,
                LogoUrl = model.LogoUrl
            };

            _context.Add(projectTheme);
            await _context.SaveChangesAsync();

            return projectTheme.Id;
        }

        public async Task<ProjectThemeModel> GetProjectThemeByProjectIdAsync(int? projectId)
        {
            var item = await _context.ProjectThemes.Where(p => p.ProjectId == projectId).OrderByDescending(d => d.DateUpdated).FirstOrDefaultAsync();
            var model = _mapper.Map<ProjectThemeModel>(item);

            return model;
        }

        public async Task<ProjectThemeModel> GetTheme(int Id)
        {
            var item = await _context.ProjectThemes.FindAsync(Id);
            var model = _mapper.Map<ProjectThemeModel>(item);

            return model;
        }

        public ProjectThemeModel GetProjectThemeByGuid(Guid themeGuid)
        {
            var item = _context.ProjectThemes.Where(t => t.ProjectId == null && t.ThemeGuid == themeGuid).OrderByDescending(t => t.DateUpdated).FirstOrDefault();
            var model = _mapper.Map<ProjectThemeModel>(item);

            return model;
        }

        public IEnumerable<ProjectThemeListItemModel> GetProjectThemeByGuidAll(Guid themeGuid)
        {
            var items = _context.ProjectThemes.Where(t => t.ProjectId == null && t.ThemeGuid == themeGuid).OrderByDescending(t => t.DateUpdated);
            var model = _mapper.Map<IEnumerable<ProjectThemeListItemModel>>(items);

            return model;
        }

        public IEnumerable<ProjectThemeListItemModel> GetProjectThemeByProjectIdAll(int projectId)
        {
            var items = _context.ProjectThemes.Where(t => t.ProjectId == projectId).OrderByDescending(t => t.DateUpdated);
            var model = _mapper.Map<IEnumerable<ProjectThemeListItemModel>>(items);

            return model;
        }

        public void ArchiveGlobalTheme(Guid themeGuid)
        {
            var items = _context.ProjectThemes.Where(t => t.ProjectId == null && t.ThemeGuid == themeGuid).ToList();

            foreach (var item in items)
            {
                item.IsArchived = true;
                item.DateArchived = DateTime.UtcNow;
            }

            _context.SaveChanges();
        }

        public void UnarchiveGlobalTheme(Guid themeGuid)
        {
            var items = _context.ProjectThemes.Where(t => t.ProjectId == null && t.ThemeGuid == themeGuid).ToList();

            foreach (var item in items)
            {
                item.IsArchived = false;
                item.DateArchived = null;
            }

            _context.SaveChanges();
        }

        public IEnumerable<ProjectThemeListItemModel> GetArchivedGlobalThemes()
        {
            var items = _context.ProjectThemes.Where(t => t.ProjectId == null && t.IsArchived == true)
                // Todo: This was breaking the query, is this necessary?  .OrderByDescending(d => d.DateUpdated).GroupBy(n => n.Name).Select(s => s.First()).OrderBy(n => n.Name) 
                .ToList();
            var projectThemes = _mapper.Map<List<ProjectThemeListItemModel>>(items);

            return projectThemes;
        }

        public ProjectThemeModel GetProjectThemeById(int id)
        {
            var item = _context.ProjectThemes.Where(t => t.Id == id).OrderByDescending(d => d.DateUpdated).FirstOrDefault();
            var model = _mapper.Map<ProjectThemeModel>(item);

            return model;
        }


        //SaveGlobalTheme - no projectId
        public async Task<ProjectThemeModel> SaveGlobalProjectThemeAsync(ProjectThemeModel model)
        {
            var mappedTheme = _mapper.Map<ProjectTheme>(model);
            mappedTheme.DateUpdated = DateTime.Now;

            _context.Add(mappedTheme);
            await _context.SaveChangesAsync();

            _mapper.Map(mappedTheme, model);

            return model;
        }

        public string GetThemeLayoutByProjectId(int id)
        {
            if (id != 0)
            {
                return _context.ProjectThemes.Where(p => p.ProjectId == id).OrderByDescending(d => d.DateUpdated).FirstOrDefault().LayoutName;
            }
            else
            {
                //Could be used for Client portal views for them to see reports etc and change branding per client. Maybe?
                return "AdminLayout";
            }
        }

        public List<SelectListItem> GetLayoutOptions()
        {
            return ProjectThemeConfiguration.Layouts.Select(l => new SelectListItem(l.Name, l.View)).ToList();
        }

        public IEnumerable<ProjectThemeListItemModel> GetClientThemes(int clientId)
        {
            var items = _context.ProjectThemes.Where(t => t.ProjectId == null && t.IsArchived == false && t.ClientId == clientId).OrderByDescending(d => d.DateUpdated).ToList().GroupBy(n => n.Name).Select(s => s.First()).OrderBy(n => n.Name);
            var projectThemes = _mapper.Map<IEnumerable<ProjectThemeListItemModel>>(items);

            return projectThemes;
        }

        //This service is called when the project website theme is created/edited 
        public async Task<string> ProjectThemeUpload(IFormFile file, Guid themeGuid)
        {
            var documentName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + $"{file.FileName.Replace(" ", string.Empty)}";
            var logoSettings = _configuration.GetSection("AwsS3LogoUpload").Get<AwsSettings>();

            var projectSettings = await  _projectSettingsAccessor.GetAsync();


            var path = StringExtensions.CombineWithSlash(
               "Project",
               projectSettings.UniqueId.ToString(),
               "Theme",
               "Logo",
               documentName);

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                var result = await _fileStorage.UploadFile(path, stream);
                return result.AbsoluteUrl;
            }          
        }

        

        public async Task<string> DeleteFile(string filename, string filextn)
        {
            var awsS3Settings = _configuration.GetSection("AwsS3LogoUpload").Get<AwsS3MediaUpload>();
            //Get project settings
            var proj = await _projectSettingsAccessor.GetAsync();
            var filepath = StringExtensions.CombineWithSlash(
                "Project",
                proj.UniqueId.ToString(),
                awsS3Settings.ParentFolder,
                awsS3Settings.SubFolder);

            var result = await _fileStorage.DeleteFile(filepath, filename, filextn);
            return result.AbsoluteUrl;
        }

        public Task<ProjectTheme> GetDefaultProjectTheme()
        {
            return Task.FromResult(new ProjectTheme
            {
                LayoutName = ProjectThemeConfiguration.Layouts.First().View,
                ThemeUniqueId = Guid.NewGuid(),
                DateUpdated = SystemTime.UtcNow
            });
        }

        public async Task<IEnumerable<MediaLibraryS3DirectoryContent>> GetFilesList()
        {
            var awsS3Settings = _configuration.GetSection("AwsS3Upload").Get<AwsS3MediaUpload>();
            //Get project settings
            var proj = await _projectSettingsAccessor.GetAsync();
            var path = StringExtensions.CombineWithSlash(
                awsS3Settings.ParentFolder,
                proj.UniqueId.ToString(),
                awsS3Settings.SubFolder);

            var result = await _fileStorage.GetFilesList(path);
            return result;
        }

        public async Task<FileStreamResult> GetFile(string filename, string filextn)
        {
            var awsS3Settings = _configuration.GetSection("AwsS3Upload").Get<AwsS3MediaUpload>();
            //Get project settings
            var proj = await _projectSettingsAccessor.GetAsync();
            var filepath = StringExtensions.CombineWithSlash(
                awsS3Settings.ParentFolder,
                proj.UniqueId.ToString(),
                awsS3Settings.SubFolder);

            var result = _fileStorage.GetFileFromAws(filepath, filename, filextn);
            return result;
        }
    }
}
