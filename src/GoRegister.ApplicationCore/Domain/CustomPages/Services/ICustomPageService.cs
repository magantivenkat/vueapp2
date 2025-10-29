using AutoMapper;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.CustomPages.Models;
using GoRegister.ApplicationCore.Framework.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.CustomPages.Services
{
    public interface ICustomPageService
    {
        Task<Result<int>> CreateAsync(CustomPageCreateEditModel model);
        Task<IEnumerable<CustomPageListItemModel>> GetList();
        Task<CustomPageCreateEditModel> FindAsync(int id);
        Task<CustomPageCreateEditModel> GetCreateEditModelAsync();
        Task<CustomPageCreateEditModel> GetCreateEditModelAsync(CustomPageCreateEditModel model);
        Task<Result<int>> EditAsync(CustomPageCreateEditModel model);
        Task DeleteAsync(int id);
        Task CreateAuditAsync(CustomPageCreateEditModel model);
        Task<IEnumerable<CustomPageAuditModel>> GetAuditAsync(int id);

        Task CreateVersionAsync(CustomPageVersionModel model);

        Task<CustomPageVersionModel> GetCurrentVersionAsync(int id);

        Task<CustomPageVersionModel> GetCustomPageVersionAsync(int id);

        Task<IEnumerable<CustomPageVersionModel>> GetVersionListAsync(int id);
        Task<IEnumerable<CustomPage>> GetNavBar(int id);
        string GetPathForCustomPage(string slug, PageType pageType, string fragment = null);
    }

    public class CustomPageService : ICustomPageService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;

        public CustomPageService(ApplicationDbContext context, IMapper mapper, IUrlHelper urlHelper)
        {
            _context = context;
            _mapper = mapper;
            _urlHelper = urlHelper;
        }

        public async Task<IEnumerable<CustomPageListItemModel>> GetList()
        {
            var items = await _context.CustomPages.AsNoTracking().ToListAsync();
            var models = _mapper.Map<IEnumerable<CustomPageListItemModel>>(items);

            var registrationTypes = await _context.RegistrationTypes.ToListAsync();
            foreach (var model in models)
            {
                model.HasMultipleRegistrationTypes = registrationTypes.Count > 1;

                // set up all the dropdowns
                if (model.HasMultipleRegistrationTypes)
                {
                    model.RegistrationTypeSelectList = new SelectList(registrationTypes, "Id", "Name");
                }
                else
                {
                    model.RegistrationTypeId[0] = registrationTypes.FirstOrDefault().Id;
                }
            }

            return models;
        }

        public async Task<CustomPageCreateEditModel> FindAsync(int id)
        {
            var item = await _context.CustomPages
                .Include(c => c.CustomPageRegistrationTypes)
                .Include(c => c.CustomPageRegistrationStatuses)
                .FirstOrDefaultAsync(c => c.Id == id);

            var model = _mapper.Map<CustomPageCreateEditModel>(item);

            return await GetCreateEditModelAsync(model);
        }

        public async Task<CustomPageCreateEditModel> GetCreateEditModelAsync()
        {
            return await GetCreateEditModelAsync(new CustomPageCreateEditModel());
        }

        public async Task<CustomPageCreateEditModel> GetCreateEditModelAsync(CustomPageCreateEditModel model)
        {
            var registrationTypes = await _context.RegistrationTypes.ToListAsync();

            model.HasMultipleRegistrationTypes = registrationTypes.Count > 1;

            if (model.HasMultipleRegistrationTypes)
            {
                model.RegistrationTypeSelectList = new SelectList(registrationTypes, "Id", "Name");

                foreach (var item in model.RegistrationTypeSelectList)
                {
                    foreach (var customPageRegistrationType in model.CustomPageRegistrationTypes
                        .Where(customPageRegistrationType => item.Value == customPageRegistrationType.RegistrationTypeId.ToString()))
                    {
                        item.Selected = true;
                    }
                }
            }
            else
            {
                model.RegistrationTypeId = new int[] { registrationTypes.FirstOrDefault().Id };
            }

            var registrationStatuses = await _context.RegistrationStatuses.ToListAsync();

            model.RegistrationStatusSelectList = new SelectList(registrationStatuses, "Id", "Description");

            foreach (var item in model.RegistrationStatusSelectList)
            {
                foreach (var customPageRegistrationStatus in model.CustomPageRegistrationStatuses
                    .Where(customPageRegistrationStatus => item.Value == customPageRegistrationStatus.RegistrationStatusId.ToString()))
                {
                    item.Selected = true;
                }
            }

            return model;
        }

        public async Task<Result<int>> CreateAsync(CustomPageCreateEditModel model)
        {
            var customPage = _mapper.Map<CustomPage>(model);

            await _context.CustomPages.AddAsync(customPage);

            foreach (var id in model.RegistrationTypeId)
            {
                var joinCustomPageRegistrationType = new CustomPageRegistrationType
                {
                    CustomPage = customPage,
                    RegistrationTypeId = id
                };

                await _context.AddAsync(joinCustomPageRegistrationType);
            }

            if (model.RegistrationStatusId != null)
            {
                foreach (var id in model.RegistrationStatusId)
                {
                    var joinCustomPageRegistrationStatus = new CustomPageRegistrationStatus
                    {
                        CustomPage = customPage,
                        RegistrationStatusId = id
                    };

                    await _context.AddAsync(joinCustomPageRegistrationStatus);
                }
            }

            await _context.SaveChangesAsync();

            return Result.Ok(customPage.Id);
        }

        public async Task<Result<int>> EditAsync(CustomPageCreateEditModel model)
        {
            //var customPage = await _context.CustomPages.FindAsync(model.Id);

            var customPage = await _context.CustomPages
                .Include(c => c.CustomPageRegistrationTypes)
                .Include(c => c.CustomPageRegistrationStatuses)
                .FirstOrDefaultAsync(c => c.Id == model.Id);

            // Change Tracker removes the old values
            _mapper.Map(model, customPage);
            await _context.SaveChangesAsync();

            if (model.RegistrationTypeId != null)
            {
                foreach (var id in model.RegistrationTypeId)
                {
                    var joinCustomPageRegistrationType = new CustomPageRegistrationType
                    {
                        CustomPageId = customPage.Id,
                        RegistrationTypeId = id
                    };

                    await _context.AddAsync(joinCustomPageRegistrationType);
                }
            }

            if (model.RegistrationStatusId != null)
            {
                foreach (var id in model.RegistrationStatusId)
                {
                    var joinCustomPageRegistrationStatus = new CustomPageRegistrationStatus
                    {
                        CustomPageId = customPage.Id,
                        RegistrationStatusId = id
                    };

                    await _context.AddAsync(joinCustomPageRegistrationStatus);
                }
            }

            await _context.SaveChangesAsync();

            return Result.Ok(customPage.Id);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.CustomPages.FindAsync(id);

            _context.CustomPages.Remove(entity);

            await _context.SaveChangesAsync();
        }

        public async Task CreateAuditAsync(CustomPageCreateEditModel model)
        {
            var customPageAudit = new CustomPageAudit
            {
                CustomPageId = model.Id,
                PageStatus = model.IsVisible,
                TimeStamp = DateTime.UtcNow
            };

            await _context.CustomPageAudits.AddAsync(customPageAudit);

            await _context.SaveChangesAsync();

            foreach (var id in model.RegistrationTypeId)
            {
                var customPageAuditRegistrationType = new CustomPageAuditRegistrationType
                {
                    CustomPageAuditId = customPageAudit.Id,
                    Name = _context.RegistrationTypes.FindAsync(id).Result.Name
                };

                await _context.CustomPageAuditRegistrationTypes.AddAsync(customPageAuditRegistrationType);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CustomPageAuditModel>> GetAuditAsync(int id)
        {
            var items = await _context.CustomPageAudits.Where(c => c.CustomPageId == id)
                .OrderByDescending(c => c.TimeStamp)
                .Include(c => c.CustomPage)
                .Include(c => c.CustomPageAuditRegistrationTypes)
                .ToListAsync();

            var models = _mapper.Map<IEnumerable<CustomPageAuditModel>>(items);

            return models;
        }

        public async Task CreateVersionAsync(CustomPageVersionModel model)
        {
            var customPageVersion = _mapper.Map<CustomPageVersion>(model);

            customPageVersion.TimeStamp = DateTime.UtcNow;

            await _context.CustomPageVersions.AddAsync(customPageVersion);
        }

        /// <summary>
        /// Get the current Custom Page's data and map it to model
        /// </summary>
        /// <param name="id">CustomPageId</param>
        /// <returns></returns>
        public async Task<CustomPageVersionModel> GetCurrentVersionAsync(int id)
        {
            var customPageVersion = await _context.CustomPages.FindAsync(id);

            var model = _mapper.Map<CustomPageVersionModel>(customPageVersion);

            return model;
        }

        public async Task<CustomPageVersionModel> GetCustomPageVersionAsync(int id)
        {
            var customPageVersion = await _context.CustomPageVersions.FindAsync(id);

            var model = _mapper.Map<CustomPageVersionModel>(customPageVersion);

            return model;
        }

        public async Task<IEnumerable<CustomPageVersionModel>> GetVersionListAsync(int id)
        {
            var items = await _context.CustomPageVersions.Where(c => c.CustomPageId == id)
                .OrderByDescending(c => c.TimeStamp)
                .Include(c => c.CustomPage)
                .ToListAsync();

            var models = _mapper.Map<IEnumerable<CustomPageVersionModel>>(items);

            return models;
        }

        public async Task<IEnumerable<CustomPage>> GetNavBar(int id)
        {
            var delegateUser = await _context.Delegates.FirstOrDefaultAsync(e => e.Id == Convert.ToInt32(id));

            var custPages = await _context.CustomPages.Include(e => e.CustomPageRegistrationTypes).Include(e => e.CustomPageRegistrationStatuses).Where(e => e.IsVisible).ToListAsync();

            return custPages.Where(e => (!e.CustomPageRegistrationTypes.Any() || e.CustomPageRegistrationTypes.Select(r => r.RegistrationTypeId).Contains(delegateUser.RegistrationTypeId) &&
                                   (!e.CustomPageRegistrationStatuses.Any() || e.CustomPageRegistrationStatuses.Select(s => s.RegistrationStatusId).Contains(delegateUser.RegistrationStatusId)))).OrderBy(e => e.Position);

        }

        public string GetPathForCustomPage(string slug, PageType pageType, string fragment = null)
        {
            var fragmentString = string.IsNullOrWhiteSpace(fragment) ? null : fragment;

            if (pageType == PageType.CustomPage)
            {
                return _urlHelper.ActionLink("Page", "Content", new { slug = slug }, fragment: fragmentString);
            }
            else if (pageType == PageType.HomePage)
            {
                return _urlHelper.ActionLink("Index", "Content", fragment: fragmentString);
            }

            throw new ArgumentOutOfRangeException(nameof(pageType), pageType, "Have not handled PageType enum value");
        }
    }
}
