using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Framework.Multitenancy;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Domain.Settings.Services
{
    public interface IProjectSettingsAccessor
    {
        Task<Project> GetAsync();
        Task<ProjectTheme> GetTheme();
        Task<List<LookupOptionDto>> GetLanguages();
    }

    public class ProjectSettingsAccessor : IProjectSettingsAccessor
    {
        private Project _projectSettings;
        private ProjectTheme _projectTheme;
        private readonly ITenantAccessor _tenantAccessor;
        private readonly ApplicationDbContext _context;

        public ProjectSettingsAccessor(ApplicationDbContext context, ITenantAccessor tenantAccessor)
        {
            _context = context;
            _tenantAccessor = tenantAccessor;
        }

        public async Task<Project> GetAsync()
        {
            return _projectSettings
                ?? (_projectSettings = await _context.Projects
                    .Include(p => p.CustomPages)
                        .ThenInclude(p => p.ProjectPage)
                    .Include(p => p.ProjectThemes)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == _tenantAccessor.Get.Id));
        }

        public async Task<ProjectTheme> GetTheme()
        {
            return _projectTheme
                ?? (_projectTheme = await _context.ProjectThemes.Include(p=>p.Fonts)
                    .OrderByDescending(e => e.DateUpdated)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.ProjectId == _tenantAccessor.Get.Id));
        }

        public async Task<List<LookupOptionDto>> GetLanguages()
        {
            return await _context.Language
                .Where(l => l.IsActive)
                .Join(_context.MRFLanguage,
                      language => language.Id,
                      mrfLanguage => mrfLanguage.LanguageId,
                      (language, mrfLanguage) => new { language, mrfLanguage })
                .Join(_context.Projects,
                      lm => lm.mrfLanguage.ProjectId,
                      project => project.Id,
                      (lm, project) => new { lm.language, project })
                .Where(p => p.project.Id == _tenantAccessor.Get.Id)
                .OrderBy(l => l.language.SortOrder)
                .Select(l => new LookupOptionDto
                {
                    Value = l.language.LanguageCode,
                    Text = l.language.LanguageName
                })
                .ToListAsync();
        }

        //public IReadOnlyList<LookupOptionDto> GetLanguages
        //{
        //    get
        //    {
        //        if (_languagesCache == null)
        //        {
        //            lock (_lock)
        //            {
        //                if (_languagesCache == null)
        //                {
        //                    _languagesCache = _context.Language
        //                        .Where(l => l.IsActive)
        //                        .OrderBy(l => l.SortOrder)
        //                        .Select(l => new LookupOptionDto
        //                        {
        //                            Value = l.LanguageCode,
        //                            Text = l.LanguageName
        //                        })
        //                        .ToList();
        //                }

        //            }
        //        }
        //        return _languagesCache;
        //    } 
        //}      
    }

    //public class LookupDataModel
    //{
    //    public List<LookupOptionDto> Languages { get; set; }

    //}
    public class LookupOptionDto
    {
        public string Value { get; set; }
        public string Text { get; set; }

    }
}
