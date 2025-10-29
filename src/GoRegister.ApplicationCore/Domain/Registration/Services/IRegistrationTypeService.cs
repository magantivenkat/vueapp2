using AutoMapper;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Services
{
    public interface IRegistrationTypeService
    {
        Task<IEnumerable<RegistrationTypeModel>> GetAll(int projectId);

        Task<RegistrationTypeModel> GetById(int id);

        int GetRegistrationPathId(int projectId);
        Task<int> GetRegistrationPathId(int projectId, int registrationPathId);

        Task<int> CreateAsync(int projectId, RegistrationTypeModel model, bool updateAllFieldsWithRegistrationType);

        Task<bool> Exists(int id);

        Task<bool> IsDuplicateName(int projectId, string name);

        Task UpdateAsync(RegistrationTypeModel model);

        Task DeleteAsync(int id);
        IEnumerable<SelectListItem> GetRegTypeSelectList();
        Task<IEnumerable<SelectListItem>> GetActiveRegPathSelectListAsync();
        Task<int> CreateRegPathAsync(RegistrationTypeModel registrationTypeModel);
    }

    public class RegistrationTypeService : IRegistrationTypeService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public RegistrationTypeService(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<RegistrationTypeModel>> GetAll(int projectId)
        {
            var registrationTypes = await _context.RegistrationTypes
                .AsNoTracking() // No need for AsNoTracking when using LINQs ProjectTo 
                .Include(d => d.DelegateUsers)
                //.Where(rt => rt.ProjectId == projectId && !rt.IsDeleted)
                //.ProjectTo<RegistrationTypeModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return _mapper.Map<IEnumerable<RegistrationTypeModel>>(registrationTypes);
        }


        public async Task<RegistrationTypeModel> GetById(int id)
        {
            return _mapper.Map<RegistrationTypeModel>(await _context.RegistrationTypes.FindAsync(id));
        }

        public int GetRegistrationPathId(int projectId)
        {
            return _context.RegistrationTypes.FirstOrDefaultAsync(e => e.ProjectId == projectId).Result.RegistrationPathId;
        }

        public async Task<int> GetRegistrationPathId(int projectId, int registrationPathId)
        {
            var regType = await _context.RegistrationTypes.FirstOrDefaultAsync(e => e.ProjectId == projectId && e.Id == registrationPathId);
            return regType.RegistrationPathId;
        }

        public async Task<int> CreateAsync(int projectId, RegistrationTypeModel model, bool updateAllFieldsWithRegistrationType)
        {
            var entity = _mapper.Map<RegistrationType>(model);
            entity.DateCreated = DateTime.UtcNow;

            await _context.RegistrationTypes.AddAsync(entity);

            if (updateAllFieldsWithRegistrationType)
            {
                throw new NotImplementedException();
            }

            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateAsync(RegistrationTypeModel model)
        {
            // validate

            var entity = await _context.RegistrationTypes.FindAsync(model.Id);
            _mapper.Map(model, entity);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.RegistrationTypes.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> IsDuplicateName(int projectId, string name)
        {
            return await _context.RegistrationTypes.AnyAsync(e => e.ProjectId == projectId
                                                                  && e.Name == name
                                                                  && !e.IsDeleted);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.RegistrationTypes.FindAsync(id);

            entity.IsDeleted = true;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetActiveRegPathSelectListAsync()
        {
            var regTypes = await _context.RegistrationTypes.Where(rt => !rt.IsDeleted).Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
            }).ToListAsync();

            return regTypes;
        }

        public IEnumerable<SelectListItem> GetRegTypeSelectList()
        {
            var regPaths = _context.RegistrationPaths.Where(p => p.IsActive).ToList().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
            });

            return regPaths;
        }

        public async Task<int> CreateRegPathAsync(RegistrationTypeModel registrationTypeModel)
        {
            var path = new RegistrationPath { Name = registrationTypeModel.Name };

            _context.RegistrationPaths.Add(path);
            await _context.SaveChangesAsync();

            return path.Id;
        }

    }
}
