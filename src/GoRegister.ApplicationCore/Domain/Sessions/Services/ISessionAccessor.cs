using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Fields.Session;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Sessions.Services
{
    public interface ISessionAccessor
    {
        Task<Session> GetSession(int id);
        Task<List<Session>> GetSessionsForCategory(int id);
        Task<List<Session>> GetSessionsForCategories(IEnumerable<int> ids);
        Task<List<SessionModel>> GetDelegateSessions(int delegateId);
    }

    public class SessionAccessor : ISessionAccessor
    {
        private readonly ApplicationDbContext _db;

        public SessionAccessor(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<SessionModel>> GetDelegateSessions(int delegateId)
        {
            var user = await _db.Delegates
                .Include(d => d.DelegateSessionBookings)
                    .ThenInclude(sb => sb.Session)
                        .ThenInclude(s => s.SessionCategory)
                .SingleOrDefaultAsync(d => d.Id == delegateId);

            var sessionList = user.DelegateSessionBookings.Select(s => new SessionModel { Id = s.SessionId, CategoryId = s.Session.SessionCategory.Id, Name = s.Session.Name }).ToList();
            return sessionList;
        }

        public Task<Session> GetSession(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Session>> GetSessionsForCategories(IEnumerable<int> ids)
        {
            var sessions = await _db.Sessions
                .Include(s => s.Project)
                .Include(s => s.SessionCategory)
                .Include(s => s.SessionRegistrationTypes)
                .Include(s => s.DelegateSessionBookings)
                .AsNoTracking()
                .ToListAsync();

            return sessions;
        }

        public Task<List<Session>> GetSessionsForCategory(int id)
        {
            throw new NotImplementedException();
        }
    }
}
