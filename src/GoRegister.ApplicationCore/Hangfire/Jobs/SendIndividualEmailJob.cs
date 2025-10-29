using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using GoRegister.ApplicationCore.Framework;
using GoRegister.ApplicationCore.Framework.Multitenancy;
using GoRegister.ApplicationCore.Services.Email;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Hangfire.Jobs
{
    public class SendIndividualEmailJob : Job<SendIndividualEmailOptions>
    {
        private readonly IRepository _repository;
        private readonly IEmailService _emailService;

        public SendIndividualEmailJob(ITenantAccessor tenantAccessor, IRepository repository, IEmailService emailService) : base(tenantAccessor)
        {
            _repository = repository;
            _emailService = emailService;
        }

        protected async override Task Handle(SendIndividualEmailOptions options)
        {
            Email email;
            if (options.EmailType != EmailType.Custom)
            {
                email = await _repository.DbContext.Emails
                    .Include(e => e.EmailTemplates)
                        .ThenInclude(e => e.RegistrationTypes)
                    .FirstOrDefaultAsync(e => e.EmailType == options.EmailType);
            }
            else
            {
                throw new NotImplementedException();
            }

            if (email == null) return;

            var user = await _repository.DbContext.Delegates
                    .Select(e => new DelegateLiquidQueryModel
                    {
                        Id = e.Id,
                        FirstName = e.ApplicationUser.FirstName,
                        LastName = e.ApplicationUser.LastName,
                        RegistrationType = e.RegistrationType.Name,
                        Email = e.ApplicationUser.Email,
                        RegistrationTypeId = e.RegistrationTypeId,
                        RegistrationDocument = e.RegistrationDocument,
                        UniqueIdentifier = e.UniqueIdentifier,
                        RegistrationStatus = (Data.Enums.RegistrationStatus)e.RegistrationStatusId
                    })
                    .FirstOrDefaultAsync(e => e.Id == options.UserId);

            if (user == null) return;

            await _emailService.GenerateAndSendEmail(email, user);
        }
    }

    public class SendIndividualEmailOptions : JobOptions
    {
        public SendIndividualEmailOptions(ProjectTenant projectTenant) : base(projectTenant) { }

        public EmailType EmailType { get; set; }
        public int? CustomEmailId { get; set; }
        public int UserId { get; set; }
    }
}
