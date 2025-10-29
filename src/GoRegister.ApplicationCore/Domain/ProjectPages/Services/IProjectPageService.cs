using AutoMapper;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.ProjectPages.Models;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using GoRegister.ApplicationCore.Extensions;
using GoRegister.ApplicationCore.Framework;
using GoRegister.ApplicationCore.Framework.Domain;
using GoRegister.ApplicationCore.Services.FileStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.ProjectPages.Services
{
    public interface IProjectPageService
    {
        Task<Result<int>> AddAsync(ProjectPageModel model);

        Task<IEnumerable<ProjectPageModel>> GetList();

        Task<ProjectPageModel> FindAsync(int id);

        Task EditAsync(ProjectPageModel model);

        Task EditRegistrationPageAsync(ProjectPageModel model);

        Task DeleteAsync(int id);

        Task<string> CkEditorFileUpload(IFormFile file);
    }

    public class ProjectPageService : IProjectPageService
    {
        private readonly ApplicationDbContext _context;
        private readonly ProjectTenant _project;
        private readonly IConfiguration _configuration;
        private readonly IProjectSettingsAccessor _projectSettingsAccessor;
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;

        public ProjectPageService(ApplicationDbContext context, IMapper mapper, ProjectTenant project, IConfiguration configuration, IProjectSettingsAccessor projectSettingsAccessor, IFileStorage fileStorage)
        {
            _context = context;
            _mapper = mapper;
            _project = project;
            _configuration = configuration;
            _projectSettingsAccessor = projectSettingsAccessor;
            _fileStorage = fileStorage;
        }

        public async Task<Result<int>> AddAsync(ProjectPageModel model)
        {
            var projectPage = _mapper.Map<ProjectPage>(model);

            await _context.ProjectPages.AddAsync(projectPage);

            await _context.SaveChangesAsync();

            return Result.Ok(projectPage.Id);
        }

        public async Task<IEnumerable<ProjectPageModel>> GetList()
        {

            //// TODO remove the below if statement later because the creation of ProjectPage and RegistrationPage should be done while creating a project
            //if (_context.ProjectPages.Any(p => p.ProjectId == _project.Id) == false)
            //{
            //    // Save registration page type in ProjectPage with menu position 1
            //    var projectPageModel = new ProjectPageModel
            //    {
            //        ProjectId = _project.Id,
            //        Type = ProjectPage.PageType.Registration,
            //        MenuPosition = 1
            //    };
            //    var projectPageId = await AddAsync(projectPageModel);

            //    // Save registration page
            //    var regPage = new RegistrationPage
            //    {
            //        Title = "Register",
            //        UniqueIdentifier = Guid.NewGuid(),
            //        ProjectPageId = projectPageId.Value,
            //        IsInternalOnly = false,
            //        IsVisible = true
            //    };
            //    await _context.RegistrationPages.AddAsync(regPage);
            //}

            //var registrationPage = await _context.ProjectPages
            //    .Include(p => p.RegistrationPage)
            //    .Select(x => new ProjectPageModel
            //    {
            //        Id = x.Id,
            //        ProjectId = x.ProjectId,
            //        PageId = x.RegistrationPage.Id,
            //        MenuPosition = x.MenuPosition,
            //        Type = x.Type,
            //        Title = x.RegistrationPage.Title,
            //        IsVisible = x.RegistrationPage.IsVisible
            //    })
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync();

            var customPages = await _context.ProjectPages.Where(p => p.Type != ProjectPage.PageType.Registration)
                .Include(p => p.CustomPage)
                .Select(x => new ProjectPageModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    PageId = x.CustomPage.Id,
                    MenuPosition = x.MenuPosition,
                    Type = x.Type,
                    Title = x.CustomPage.Title,
                    IsVisible = x.CustomPage.IsVisible
                })
                .AsNoTracking()
                .ToListAsync();


            //var projectPages = new List<ProjectPageModel> { registrationPage };
            var projectPages = new List<ProjectPageModel>();
            projectPages.AddRange(customPages);

            var models = _mapper.Map<IEnumerable<ProjectPageModel>>(projectPages);

            return models;
        }

        public async Task<ProjectPageModel> FindAsync(int id)
        {
            var item = await _context.ProjectPages
                .FirstOrDefaultAsync(c => c.Id == id);

            var model = _mapper.Map<ProjectPageModel>(item);

            return model;
        }

        public async Task EditAsync(ProjectPageModel model)
        {
            var projectPage = await _context.ProjectPages
                .FirstOrDefaultAsync(c => c.Id == model.Id);

            _mapper.Map(model, projectPage);

            await _context.SaveChangesAsync();
        }

        public async Task EditRegistrationPageAsync(ProjectPageModel model)
        {

            // TODO add RegistrationPageService
            var registrationPage = await _context.RegistrationPages
                .FirstOrDefaultAsync(c => c.Id == model.Id);

            registrationPage.IsVisible = model.IsVisible;
            registrationPage.Title = model.Title;


            //_mapper.Map(model, projectPage);

            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var entity = await _context.ProjectPages.FindAsync(id);

            _context.ProjectPages.Remove(entity);

            await _context.SaveChangesAsync();
        }

        public async Task<string> CkEditorFileUpload(IFormFile file)
        {
            var documentName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + $"{file.FileName}";
            var tinyMceSettings = _configuration.GetSection("AwsS3Upload").Get<TinyMceAwsS3Upload>();

            //Get project settings
            var proj = await _projectSettingsAccessor.GetAsync();

            var path = StringExtensions.CombineWithSlash(
                tinyMceSettings.ParentFolder, 
                proj.UniqueId.ToString(), 
                tinyMceSettings.SubFolder, 
                documentName);

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                var result = await _fileStorage.UploadFile(path, stream);
                return result.AbsoluteUrl;
            }
        }
    }
}
