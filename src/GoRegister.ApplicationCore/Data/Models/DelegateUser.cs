using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class DelegateUser : MustHaveProjectEntity, IDelegateUserCache
    {
        public int Id { get; set; }
        public int RegistrationTypeId { get; set; }
        public int? ParentDelegateUserId { get; set; }
        public int RegistrationStatusId { get; set; }
        public string RegistrationDocument { get; set; }

        public string MRFClientResponse { get; set; }

        public string MRFClientUserInvitationlink { get; set; }
        public string DeclineDocument { get; set; }
        public string CancellationDocument { get; set; }
        public Guid UniqueIdentifier { get; set; }

        public DateTime? InvitedUtc { get; set; }
        public DateTime? ConfirmedUtc { get; set; }
        public DateTime? CancelledUtc { get; set; }
        public DateTime? DeclinedUtc { get; set; }
        public DateTime? ModifiedUtc { get; set; }
        public bool IsTest { get; set; }

        [ForeignKey("Id")]
        public ApplicationUser ApplicationUser { get; set; }
        public virtual RegistrationType RegistrationType { get; set; }
        public virtual DelegateUser ParentDelegateUser { get; set; }
        public virtual RegistrationStatus RegistrationStatus { get; set; }
        public virtual ICollection<DelegateUserAudit> Audits { get; set; } = new HashSet<DelegateUserAudit>();
        public ICollection<UserFormResponse> UserFormResponses { get; set; } = new HashSet<UserFormResponse>();
        public List<DelegateSessionBooking> DelegateSessionBookings { get; set; }
        public bool AcceptedPrivacyPolicy { get; set; }
        public DateTime? AcceptedPrivacyPolicyDateUtc { get; set; }
        public string AttendeeNumber { get; set; }

        public static DelegateUser Create(int regTypeId)
        {
            var user = new DelegateUser();
            user.ApplicationUser = ApplicationUser.Create();
            user.RegistrationTypeId = regTypeId;
            user.UniqueIdentifier = Guid.NewGuid();
            user.ApplicationUser.DelegateUser = user;
            return user;
        }

        public void ChangeRegistrationStatus(Enums.RegistrationStatus newStatus)
        {
            // check that the value hasn't changed
            if ((int)newStatus == RegistrationStatusId) return;

            RegistrationStatusId = (int)newStatus;
            var time = SystemTime.UtcNow;

            // update actioned time
            switch (newStatus)
            {
                case Enums.RegistrationStatus.Confirmed: ConfirmedUtc = time; break;
                case Enums.RegistrationStatus.Invited: InvitedUtc = time; break;
                case Enums.RegistrationStatus.Declined: DeclinedUtc = time; break;
                case Enums.RegistrationStatus.Cancelled: CancelledUtc = time; break;
            }

            _audit.RegistrationStatusId = (int)newStatus;
        }

        public void HasBeenModified() => ModifiedUtc = SystemTime.UtcNow;

        public void UpdateFirstName(string firstName)
        {
            if (ApplicationUser == null) throw new NullReferenceException();

            if (firstName == ApplicationUser.FirstName) return;

            ApplicationUser.FirstName = firstName;
            _audit.FirstName = firstName;
        }

        public void UpdateLastName(string lastName)
        {
            if (ApplicationUser == null) throw new NullReferenceException();

            if (lastName == ApplicationUser.LastName) return;

            ApplicationUser.LastName = lastName;
            _audit.LastName = lastName;
        }

        public void UpdateEmail(string email)
        {
            if (ApplicationUser == null) throw new NullReferenceException();

            if (email == ApplicationUser.Email) return;

            ApplicationUser.Email = email;
            ApplicationUser.NormalizedEmail = email.ToUpperInvariant();
            _audit.Email = email;
        }

        public void UpdateRegistrationType(int registrationTypeId)
        {
            if (registrationTypeId == RegistrationTypeId) return;

            RegistrationTypeId = registrationTypeId;
            _audit.RegistrationTypeId = registrationTypeId;
        }

        // create delegate audit model
        // - id, firstname, lastname, email, registrationstatusid, createdutc, actionedbyid
        // - actionedfrom (AdminForm, Form, Upload)
        // - with list of userfieldresponseaudit
        //  - basically copy of userfieldresponse
        //TODO: make response values nullable

        private DelegateUserAudit _audit = new DelegateUserAudit();

        internal void SetAudit(DelegateUserAudit audit) => _audit = audit;

        public DelegateUserAudit GetAudit(ActionedFrom from)
        {
            return GetAudit(from, this.ApplicationUser);
        }

        public DelegateUserAudit GetAudit(ActionedFrom from, ApplicationUser actionedBy)
        {
            if (actionedBy == null) throw new ArgumentNullException("Actioned By ApplicationUser cannot be null when creating an audit");

            _audit.DelegateUser = this;
            _audit.ActionedFrom = from;
            _audit.ActionedBy = actionedBy;
            return _audit;

        }

        public DelegateUserAudit GetAudit(ActionedFrom from, int actionedById)
        {
            if (actionedById == 0) throw new ArgumentNullException("Actioned By ApplicationUser cannot be null when creating an audit");

            _audit.DelegateUser = this;
            _audit.ActionedFrom = from;
            _audit.ActionedById = actionedById;
            return _audit;
        }

        public void SetNote(string note) => _audit.Note = note;
    }

    public class DelegateUserMap : IEntityTypeConfiguration<DelegateUser>
    {
        public void Configure(EntityTypeBuilder<DelegateUser> builder)
        {
            builder
                .HasIndex(du => new { du.AttendeeNumber, du.ProjectId })
                .IsUnique();
        }
    }
}
