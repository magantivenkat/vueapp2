using AutoMapper;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using GoRegister.ApplicationCore.Domain.Sessions.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Sessions.Services
{
    public interface ISessionService
    {
        Task<SessionModel> CreateAsync(SessionCreateEditViewModel model);
        SessionModel GetSession(int sessionId);
        List<SessionModel> GetSessions();
        Task<SessionCreateEditViewModel> GetAsync(int sessionId);
        Task<SessionCategoryModel> CreateCategoryAsync(SessionCategoryModel model);
        IEnumerable<SelectListItem> GetCategorySelectList();
        Task<List<DelegateListItemModel>> GetDelegatesForSession(int sessionId);

        List<SessionModel> GetUserSessions(int? userId, int projectId);
    }

    public class SessionService : ISessionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;        

        public SessionService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;            
        }

        public async Task<SessionModel> CreateAsync(SessionCreateEditViewModel model)
        {
            if (model.Id == 0)
            {
                // create new
                var session = _mapper.Map<Session>(model);

                var project = await _context.Projects.SingleOrDefaultAsync(p => p.Id == session.ProjectId);
                session.Project = project;

                if (model.SessionCategoryId != null)
                {
                    session.SessionCategory = await _context.SessionCategories.SingleOrDefaultAsync(c => c.Id == model.SessionCategoryId);
                } else
                {
                    session.SessionCategory = null;
                }

                session.DateCreatedUtc = SystemTime.UtcNow;
                session.SessionRegistrationTypes = new List<SessionRegistrationType>();
                AddRemoveRegistrationTypes(session, model);

                _context.Sessions.Add(session);
                await _context.SaveChangesAsync();
                return _mapper.Map<SessionModel>(session);
            }
            else
            {
                // update session
                var session = await _context.Sessions
                    .Include(s => s.Project)
                    .Include(s => s.SessionRegistrationTypes)
                    .SingleOrDefaultAsync(s => s.Id == model.Id);
                AddRemoveRegistrationTypes(session, model);
                _context.Entry(session).CurrentValues.SetValues(model);
                await _context.SaveChangesAsync();

                return _mapper.Map<SessionModel>(session);
            }

        }

        private void AddRemoveRegistrationTypes(Session session, SessionCreateEditViewModel model)
        {
            var regTypes = _context.RegistrationTypes.AsNoTracking()
                .Where(rt => rt.ProjectId == session.Project.Id) // not needed ...
                .ToList();

            // Loop through all reg types for project 
            foreach (var regType in regTypes)
            {
                // work out wether to add or remove based on model.regtypeids
                if (model.RegTypeIds != null && model.RegTypeIds.Contains(regType.Id))
                {
                    // ADD / KEEP
                    var exists = session.SessionRegistrationTypes.SingleOrDefault(s => s.RegistrationTypeId == regType.Id);
                    if (exists == null)
                    {
                        session.SessionRegistrationTypes.Add(new SessionRegistrationType { RegistrationTypeId = regType.Id, SessionId = session.Id });
                    };
                }
                else
                {
                    // REMOVE
                    var exists = session.SessionRegistrationTypes.SingleOrDefault(s => s.RegistrationTypeId == regType.Id);
                    if (exists != null)
                    {
                        session.SessionRegistrationTypes.Remove(exists);
                    };

                }
            }
        }

        public async Task<SessionCategoryModel> CreateCategoryAsync(SessionCategoryModel model)
        {
            var project = await _context.Projects.SingleOrDefaultAsync(p => p.Id == model.ProjectId);
            var category = _mapper.Map<SessionCategory>(model);
            category.Project = project;

            _context.SessionCategories.Add(category);
            await _context.SaveChangesAsync();

            return _mapper.Map<SessionCategoryModel>(category);
        }

        public async Task<SessionCreateEditViewModel> GetAsync(int sessionId)
        {
            var session = await _context.Sessions
                .Include(s => s.SessionCategory)
                .Include(s => s.SessionRegistrationTypes)
                .Include(s => s.DelegateSessionBookings)
                .SingleOrDefaultAsync(s => s.Id == sessionId);
            return _mapper.Map<SessionCreateEditViewModel>(session);
        }

        public IEnumerable<SelectListItem> GetCategorySelectList()
        {
            var categories = _context.SessionCategories
                .ToList().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });

            return categories;
        }


        public SessionModel GetSession(int sessionId)
        {
            var session = _context.Sessions
                .Include(s => s.SessionCategory)
                .Include(s => s.DelegateSessionBookings).SingleOrDefault(s => s.Id == sessionId);

            return _mapper.Map<SessionModel>(session);
        }

        public List<SessionModel> GetSessions()
        {
            var sessions = _context.Sessions
                .Include(s => s.SessionCategory)
                .Include(s => s.DelegateSessionBookings).ToList();
            return _mapper.Map<List<SessionModel>>(sessions);
        }

        public async Task<List<DelegateListItemModel>> GetDelegatesForSession(int sessionId)
        {
            var delegates = await _context.DelegateSessionBookings
                .Include(b => b.DelegateUser)
                .ThenInclude(u => u.ApplicationUser)
                .Where(b => b.DateReleasedUtc == null && b.SessionId == sessionId)
                .Select(b =>
                    new DelegateListItemModel{
                        FirstName = b.DelegateUser.ApplicationUser.FirstName,
                        LastName = b.DelegateUser.ApplicationUser.LastName,
                    Email = b.DelegateUser.ApplicationUser.Email})
                .ToListAsync();
            return delegates;
        }

        public List<SessionModel> GetUserSessions(int? userId, int projectId)
        {            
            var result = _context.StoredProcedures.GetUserSession(userId, projectId);                     

            return _mapper.Map<List<SessionModel>>(result);            
        }
    }
}
