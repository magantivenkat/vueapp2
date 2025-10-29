using AutoMapper;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Sessions.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Sessions.Services
{
    public interface ISessionBookingService
    {
        void AddDelegate(Registration.Fields.Session.SessionModel session, DelegateUser delegateUser);
        string ReserveSessionForDelegate(int sessionId, int delegateId, bool forceAdd);
        string ReserveSessionForDelegate(int sessionId, string anonGuid);
    }

    public class SessionBookingService : ISessionBookingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        private readonly string SESSION_ADDED = "added";
        private readonly string SESSION_REMOVED = "removed";
        private readonly string SESSION_FULL = "full"; 
        private readonly string SESSION_TIMECLASH = "timeclash"; 

        public SessionBookingService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public void AddDelegate(Registration.Fields.Session.SessionModel session, DelegateUser delegateUser)
        {
            // Add to db if not exists
            var bookingExists = _context.DelegateSessionBookings.SingleOrDefault(b => b.DelegateUserId == delegateUser.Id && b.SessionId == session.Id);

            if (bookingExists == null)
            {
                var sessionBooking = new DelegateSessionBooking
                {
                    DelegateUser = delegateUser,
                    SessionId = session.Id,
                    DateActionedUtc = SystemTime.UtcNow
                };

                _context.Add(sessionBooking);
            }

        }


        public string ReserveSessionForDelegate(int sessionId, int delegateId, bool forceAdd)
        {
            var sessionToJoin = _context.Sessions
                .Include(s => s.DelegateSessionBookings).ThenInclude(b => b.DelegateUser)
                .Include(s => s.AnonSessionBookings)
                .Include(s => s.SessionCategory)
                .SingleOrDefault(s => s.Id == sessionId);

            var currentDelegateBookings = _context.DelegateSessionBookings.Include(b => b.Session)
                .Where(b => b.DelegateUserId == delegateId).ToList();

            var booking = currentDelegateBookings.SingleOrDefault(b => b.SessionId == sessionId && b.DelegateUserId == delegateId);
            bool isFull = forceAdd ? false : (sessionToJoin.DelegateSessionBookings.Where(b => !b.DelegateUser.IsTest).ToList().Count + sessionToJoin.AnonSessionBookings?.Count) >= sessionToJoin.Capacity + sessionToJoin.CapacityReserved;

            if (sessionToJoin.SessionCategory.IsSingleSession)
            {
                var sessions = 
                    _context.Sessions.Include(s => s.SessionCategory)
                    .Where(s => s.SessionCategory.Id == sessionToJoin.SessionCategory.Id).ToList();

                if(currentDelegateBookings.Any(b => b.SessionId == sessionId && b.DelegateUserId == delegateId))
                {
                    _context.DelegateSessionBookings.Remove(booking);
                    _context.SaveChanges();

                    return SESSION_REMOVED;
                }

                // remove delegate from all sessions with submitted categoryId
                foreach (var sessionSingle in sessions)
                {
                    var sessionBooking = _context.DelegateSessionBookings.SingleOrDefault(b => b.DelegateUserId == delegateId && b.SessionId == sessionSingle.Id);
                    if (sessionBooking != null)
                    {
                        _context.DelegateSessionBookings.Remove(sessionBooking);
                    }
                }

                if (SessionTimeClash(currentDelegateBookings, sessionToJoin)) return SESSION_TIMECLASH;

                if (isFull)
                {
                    _context.SaveChanges();

                    return SESSION_FULL;
                }

                // add reservation with a time to reserve time limit. (15 mins)
                booking = new DelegateSessionBooking
                {
                    DelegateUserId = delegateId,
                    SessionId = sessionId,
                    DateReleasedUtc = SystemTime.UtcNow.AddMinutes(15)
                };

                _context.DelegateSessionBookings.Add(booking);
                _context.SaveChanges();

                return SESSION_ADDED;
            }
            else
            {
                if (booking != null)
                {
                    // free up reservation - remove  from database
                    _context.DelegateSessionBookings.Remove(booking);
                    _context.SaveChanges();
                    return SESSION_REMOVED;
                }
                else if (SessionTimeClash(currentDelegateBookings, sessionToJoin))
                {
                    return SESSION_TIMECLASH;
                }
                else if (isFull)
                {
                    return SESSION_FULL;
                }
                else
                {
                    // add reservation with a time to reserve time limit. (15 mins)
                    booking = new DelegateSessionBooking
                    {
                        DelegateUserId = delegateId,
                        SessionId = sessionId,
                        DateReleasedUtc = SystemTime.UtcNow.AddMinutes(15)
                    };

                    _context.DelegateSessionBookings.Add(booking);
                    _context.SaveChanges();

                    return SESSION_ADDED;
                }
            }
        }
        public string ReserveSessionForDelegate(int sessionId, string anonGuid)
        {
            var sessionToJoin = _context.Sessions.Include(s => s.DelegateSessionBookings).Include(s => s.AnonSessionBookings).Include(s => s.SessionCategory).SingleOrDefault(s => s.Id == sessionId);
            var currentAnonBookings = _context.AnonSessionBookings.Include(b => b.Session).Where(b => b.AnonUserId == anonGuid).ToList();

            var booking = currentAnonBookings.SingleOrDefault(b => b.SessionId == sessionId && b.AnonUserId == anonGuid);
            bool isFull = sessionToJoin.DelegateSessionBookings.Count + sessionToJoin.AnonSessionBookings.Count >= sessionToJoin.Capacity + sessionToJoin.CapacityReserved;

            if (sessionToJoin.SessionCategory.IsSingleSession)
            {
                var sessions = _context.Sessions.Include(s => s.SessionCategory).Where(s => s.SessionCategory.Id == sessionToJoin.SessionCategory.Id).ToList();
                if (currentAnonBookings.Any(b => b.SessionId == sessionId && b.AnonUserId == anonGuid))
                {
                    _context.AnonSessionBookings.Remove(booking);
                    _context.SaveChanges();

                    return SESSION_REMOVED;
                }

                // remove anonUser from all sessions with submitted categoryId
                foreach (var sessionSingle in sessions)
                {
                    var sessionBooking = _context.AnonSessionBookings.SingleOrDefault(b => b.AnonUserId == anonGuid && b.SessionId == sessionSingle.Id);
                    if (sessionBooking != null)
                    {
                        _context.AnonSessionBookings.Remove(sessionBooking);
                    }
                }

                if (SessionTimeClash(currentAnonBookings, sessionToJoin)) return SESSION_TIMECLASH;

                if (isFull)
                {
                    _context.SaveChanges();

                    return SESSION_FULL;
                }

                // add reservation with a time to reserve time limit. (15 mins)
                booking = new AnonSessionBooking
                {
                    AnonUserId = anonGuid,
                    SessionId = sessionId,
                    DateReleasedUtc = SystemTime.UtcNow.AddMinutes(15)
                };

                _context.AnonSessionBookings.Add(booking);
                _context.SaveChanges();

                return SESSION_ADDED;
            }
            else
            {
                if (booking != null)
                {
                    // free up reservation - remove  from database
                    _context.AnonSessionBookings.Remove(booking);
                    _context.SaveChanges();
                    return SESSION_REMOVED;
                }
                else if (SessionTimeClash(currentAnonBookings, sessionToJoin))
                {
                    return SESSION_TIMECLASH;
                }
                else if (isFull)
                {
                    return SESSION_FULL;
                }
                else
                {
                    // add reservation with a time to reserve time limit. (15 mins)
                    booking = new AnonSessionBooking
                    {
                        AnonUserId = anonGuid,
                        SessionId = sessionId,
                        DateReleasedUtc = SystemTime.UtcNow.AddMinutes(15)
                    };

                    _context.AnonSessionBookings.Add(booking);
                    _context.SaveChanges();

                    return SESSION_ADDED;
                }
            }

        }

        private bool SessionTimeClash(List<DelegateSessionBooking> currentBookings, Session sessionToJoin)
        {
            foreach (var booking in currentBookings)
            {
                if (sessionToJoin.DateStartUtc < booking.Session.DateEndUtc &&
                    sessionToJoin.DateEndUtc > booking.Session.DateStartUtc) return true;
            }

            return false;
        }

        private bool SessionTimeClash(List<AnonSessionBooking> currentBookings, Session sessionToJoin)
        {
            foreach (var booking in currentBookings)
            {
                if (sessionToJoin.DateStartUtc < booking.Session.DateEndUtc &&
                    sessionToJoin.DateEndUtc > booking.Session.DateStartUtc) return true;
            }

            return false;
        }

    }
}
