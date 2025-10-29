using AutoMapper;
using Microsoft.EntityFrameworkCore;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Framework;
using GoRegister.ApplicationCore.Framework.Domain;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Settings.Services
{
    public interface ISettingsService
    {
        Task<T> GetSettingsAsync<T>();
        Task<Result> UpdateSettings<T>(T model);
    }

    public class SettingsService : ISettingsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ProjectTenant _project;
        private readonly IMapper _mapper;

        public SettingsService(ApplicationDbContext context, ProjectTenant project, IMapper mapper)
        {
            _context = context;
            _project = project;
            _mapper = mapper;
        }

        public async Task<T> GetSettingsAsync<T>()
        {
            var settings = await _context.Projects
                .Include(p => p.Client)
                .FirstOrDefaultAsync(e => e.Id == _project.Id);

            return _mapper.Map<T>(settings);
        }

        public async Task<Result> UpdateSettings<T>(T model)
        {
            var settings = await _context
                .Projects
                .Include(p => p.Client)
                .FirstOrDefaultAsync(e => e.Id == _project.Id);

            _mapper.Map(model, settings);
            _context.Update(settings);
            _context.SaveChanges();
            return Result.Ok();
        }
    }
}
