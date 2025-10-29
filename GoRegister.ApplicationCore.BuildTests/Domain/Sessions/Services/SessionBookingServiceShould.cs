using AutoMapper;
using FluentAssertions;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Sessions;
using GoRegister.ApplicationCore.Domain.Sessions.Services;
using GoRegister.TestingCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GoRegister.Tests.Domain.Sessions.Services
{
    public class SessionBookingServiceShould : DatabaseContextTest
    {
        private readonly IMapper _mapper;

        private int SINGLE_SESSION_ID = 1;
        private int MULTI_SESSION_ID = 10;
        private int FULL_SESSION_ID = 20;
        private int TIMECLASH_SESSION_ID = 101;

        private int DELEGATE_TO_REMOVE = 99;

        private Session SINGLE_SESSION = new Session { Id = 1, Capacity = 10, };
        private Session MULTI_SESSION = new Session { Id = 10, Name = "Multi Session 1", Capacity = 2, DateCreatedUtc = DateTime.UtcNow, DateStartUtc = new DateTime(2021, 1, 25, 9, 1, 0), DateEndUtc = new DateTime(2021, 1, 25, 10, 1, 0), DateCloseRegistrationUtc = DateTime.UtcNow };
        private Session FULL_SESSION = new Session { Id = 20, Capacity = 1, };

        private SessionCategory SINGLE_SESSION_CATEGORY = new SessionCategory { Id = 1, IsSingleSession = true };
        private SessionCategory MULTI_SESSION_CATEGORY = new SessionCategory { Id = 2, IsSingleSession = false };

        private DelegateUser DELEGATE_1 = new DelegateUser { Id = 1, IsTest = false };
        private DelegateUser DELEGATE_2 = new DelegateUser { Id = 2, IsTest = false };
        private DelegateUser DELEGATE_TEST = new DelegateUser { Id = 10, IsTest = true };
        private List<DelegateSessionBooking> TIME_CLASH_DELEGATE= new List<DelegateSessionBooking> { new DelegateSessionBooking { SessionId = 100, DelegateUserId = 15 } };

        public SessionBookingServiceShould()
        {
            SINGLE_SESSION.SessionCategory = SINGLE_SESSION_CATEGORY;
            MULTI_SESSION.SessionCategory = MULTI_SESSION_CATEGORY;
            FULL_SESSION.SessionCategory = MULTI_SESSION_CATEGORY;

            List<Session> sessions = new List<Session> { SINGLE_SESSION, MULTI_SESSION, FULL_SESSION };
            List<SessionCategory> sessionCategories = new List<SessionCategory> { SINGLE_SESSION_CATEGORY, MULTI_SESSION_CATEGORY };
            List<DelegateUser> delegates = new List<DelegateUser> { DELEGATE_1, DELEGATE_2, DELEGATE_TEST };
            var timeClashDelegate = new List<DelegateSessionBooking> { new DelegateSessionBooking { SessionId = 100, DelegateUserId = 15 } };

            using (var db = GetDatabase())
            {
                db.Sessions.AddRange(sessions);
                db.Sessions.Add(new Session { Id = 100, DateStartUtc = new DateTime(2021, 1, 1, 9, 0, 0, 0), DateEndUtc = new DateTime(2021, 1, 1, 12, 0, 0, 0), Capacity = 5, DelegateSessionBookings = TIME_CLASH_DELEGATE, SessionCategory = MULTI_SESSION_CATEGORY });
                db.Sessions.Add(new Session { Id = 101, DateStartUtc = new DateTime(2021, 1, 1, 11, 0, 0, 0), DateEndUtc = new DateTime(2021, 1, 1, 14, 0, 0, 0), Capacity = 5, SessionCategory = SINGLE_SESSION_CATEGORY });
                db.SessionCategories.AddRange(sessionCategories);
                db.Delegates.AddRange(delegates);
                db.SaveChanges();
            }
        }

        [Fact]
        public void ReserveSingleSessionForDelegate_If_Availible()
        {
            using (var db = GetDatabase())
            {
                var _sut = new SessionBookingService(db, _mapper);
                var sessionId = SINGLE_SESSION_ID;
                var delegateId = 1;

                // Act
                var result = _sut.ReserveSessionForDelegate(sessionId, delegateId, false);

                // Assert
                result.Should().Be("added");
            }
        }

        [Fact]
        public void LeaveSingleSessionForDelegate()
        {
            using (var db = GetDatabase())
            {
                var _sut = new SessionBookingService(db, _mapper);
                var sessionId = SINGLE_SESSION_ID;
                var delegateId = 1;

                // Act 
                _sut.ReserveSessionForDelegate(sessionId, delegateId, false);
                var result = _sut.ReserveSessionForDelegate(sessionId, delegateId, false);

                // Assert
                result.Should().Be("removed");

            }
            // Arrange
        }

        [Fact]
        public void ReserveMultiSessionForDelegate_If_Availible()
        {
            using (var db = GetDatabase())
            {
                var _sut = new SessionBookingService(db, _mapper);
                var sessionId = MULTI_SESSION_ID;
                var delegateId = 1;

                // Act
                var result = _sut.ReserveSessionForDelegate(sessionId, delegateId, false);

                // Assert
                result.Should().Be("added");
            }
        }

        [Fact]
        public void Session_Is_Full_Return_Full()
        {
            using (var db = GetDatabase())
            {
                // add attendee so session capacity is full
                var session = db.Sessions
                    .Include(s => s.DelegateSessionBookings)
                    .SingleOrDefault(s => s.Id == FULL_SESSION_ID);

                session.DelegateSessionBookings.Add(
                    new DelegateSessionBooking { DelegateUser = new DelegateUser { Id = 1011 } }
                    );
                db.SaveChanges();

                var _sut = new SessionBookingService(db, _mapper);
                var sessionId = FULL_SESSION_ID;
                _sut.ReserveSessionForDelegate(sessionId, 190, false);

                var delegateId = 1;
                // Act
                var result = _sut.ReserveSessionForDelegate(sessionId, delegateId, false);

                // Assert
                result.Should().Be("full");
            }
        }

        [Fact]
        public void RemoveDelegateFromSession()
        {
            using (var db = GetDatabase())
            {
                // add an attendee to the session DELEGATE_TO_REMOVE
                var session = db.Sessions
                    .Include(s => s.DelegateSessionBookings)
                    .SingleOrDefault(s => s.Id == FULL_SESSION_ID);
                session.DelegateSessionBookings.Add(
                    new DelegateSessionBooking { DelegateUser = new DelegateUser { Id = DELEGATE_TO_REMOVE } }
                    );
                db.SaveChanges();

                // remove that same attendee DELEGATE_TO_REMOVE
                var _sut = new SessionBookingService(db, _mapper);
                var sessionId = FULL_SESSION_ID;
                var delegateId = DELEGATE_TO_REMOVE;

                // Act
                var result = _sut.ReserveSessionForDelegate(sessionId, delegateId, false);

                // Assert
                result.Should().Be("removed");
            }
        }


        [Fact]
        public void ReserveSingleSessionForAnonDelegate_If_Availible()
        {
            using (var db = GetDatabase())
            {
                var _sut = new SessionBookingService(db, _mapper);
                var sessionId = SINGLE_SESSION_ID;
                var anonGuid = "1";

                // Act
                var result = _sut.ReserveSessionForDelegate(sessionId, anonGuid);

                // Assert
                result.Should().Be("added");
            }
        }

        [Fact]
        public void ReserveMultiSessionForAnonDelegate_If_Availible()
        {
            using (var db = GetDatabase())
            {
                var _sut = new SessionBookingService(db, _mapper);
                var sessionId = MULTI_SESSION_ID;
                var anonGuid = "1";

                // Act
                var result = _sut.ReserveSessionForDelegate(sessionId, anonGuid);

                // Assert
                result.Should().Be("added");
            }
        }

        [Fact]
        public void AnonSession_Is_Full_Return_Full()
        {
            using (var db = GetDatabase())
            {
                var session = db.Sessions.Include(s => s.AnonSessionBookings).SingleOrDefault(s => s.Id == FULL_SESSION_ID);
                session.AnonSessionBookings.Add(new AnonSessionBooking { AnonUserId = "99" });
                db.SaveChanges();

                var _sut = new SessionBookingService(db, _mapper);
                var sessionId = FULL_SESSION_ID;
                var anonGuid = "1";

                // Act
                var result = _sut.ReserveSessionForDelegate(sessionId, anonGuid);

                // Assert
                result.Should().Be("full");
            }
        }

        [Fact]
        public void RemoveAnonDelegateFromSession()
        {
            using (var db = GetDatabase())
            {
                // add anon booking
                var session = db.Sessions.Include(s => s.AnonSessionBookings).SingleOrDefault(s => s.Id == FULL_SESSION_ID);
                session.AnonSessionBookings.Add(new AnonSessionBooking { AnonUserId = "99" });
                db.SaveChanges();

                var _sut = new SessionBookingService(db, _mapper);
                var sessionId = FULL_SESSION_ID;
                var anonGuid = "99";

                // Act
                var result = _sut.ReserveSessionForDelegate(sessionId, anonGuid);

                // Assert
                result.Should().Be("removed");
            }
        }

        [Fact]
        public void SessionTimeClash_Returns_TimeClash()
        {
            using (var db = GetDatabase())
            {
                var _sut = new SessionBookingService(db, _mapper);
                var sessionId = TIMECLASH_SESSION_ID;
                var delegateId = 15;

                // Act
                var result = _sut.ReserveSessionForDelegate(sessionId, delegateId, false);

                // Assert
                result.Should().Be("timeclash");
            }
        }

        [Fact]
        public void SessionTimeOkay_Returns_Added()
        {
            using (var db = GetDatabase())
            {
                // create new multisession
                var category = db.SessionCategories.Find(MULTI_SESSION_CATEGORY.Id);
                db.Sessions.Add(new Session { Id = 11, Name = "Multi Session 1", Capacity = 2, SessionCategory = category, DateCreatedUtc = DateTime.UtcNow, DateStartUtc = new DateTime(2021, 1, 25, 11, 1, 0), DateEndUtc = new DateTime(2021, 1, 25, 14, 1, 0), DateCloseRegistrationUtc = DateTime.UtcNow });
                db.SaveChanges();

                var _sut = new SessionBookingService(db, _mapper);
                var delegateId = 1;
                var sessionAId = MULTI_SESSION_ID;
                var sessionBId = 11;

                // Act
                _sut.ReserveSessionForDelegate(sessionAId, delegateId, false);
                var result = _sut.ReserveSessionForDelegate(sessionBId, delegateId, false);

                // Assert
                result.Should().Be("added");
            }
        }

        [Fact]
        public void SessionTimeOkay_Returns_Added2()
        {
            using (var db = GetDatabase())
            {
                var category = db.SessionCategories.Find(MULTI_SESSION_CATEGORY.Id);
                db.Sessions.Add(new Session { Id = 11, Name = "Multi Session 1", Capacity = 2, SessionCategory = category, DateCreatedUtc = DateTime.UtcNow, DateStartUtc = new DateTime(2021, 1, 25, 11, 1, 0), DateEndUtc = new DateTime(2021, 1, 25, 14, 1, 0), DateCloseRegistrationUtc = DateTime.UtcNow });
                db.SaveChanges();

                var _sut = new SessionBookingService(db, _mapper);
                var delegateId = 1;
                var sessionAId = 10;
                var sessionBId = 11;

                // Act
                _sut.ReserveSessionForDelegate(sessionBId, delegateId, false);
                var result = _sut.ReserveSessionForDelegate(sessionAId, delegateId, false);

                // Assert
                result.Should().Be("added");
            }
        }

        [Fact]
        public void WhenAdmin_BypassChecks_AddDelegate()
        {
            using (var db = GetDatabase())
            {
                var _sut = new SessionBookingService(db, _mapper);
                var delegateId = 1;
                var sessionAId = FULL_SESSION_ID;
                //var sessionBId = 11;
                var forceAdd = true;

                //// Act
                var result = _sut.ReserveSessionForDelegate(sessionAId, delegateId, forceAdd);

                //// Assert
                result.Should().Be("added");
            }
        }

        [Fact]
        public void WhenGettingSessionCapacity_DontInclude_TestDelegates()
        {
            using (var db = GetDatabase())
            {
                db.DelegateSessionBookings.Add(new DelegateSessionBooking { SessionId = SINGLE_SESSION.Id, DelegateUserId = DELEGATE_TEST.Id });
                db.SaveChanges();

                var sut = new SessionBookingService(db, _mapper);
                var result = sut.ReserveSessionForDelegate(SINGLE_SESSION.Id, DELEGATE_1.Id, false);

                result.Should().Be("added");
            }


        }



    }
}
