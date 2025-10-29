using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using GoRegister.ApplicationCore.Framework.Domain;
using GoRegister.ApplicationCore.Services.Email;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Emails
{
    public static class GenerateEmailPreview
    {
        public class Command : IRequest<Result<Guid>>
        {
            public int[] UserIds { get; set; }
            public int EmailId { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Result<Guid>>
        {
            private readonly ApplicationDbContext _db;
            private readonly IEmailService _emailService;

            public CommandHandler(ApplicationDbContext db, IEmailService emailService)
            {
                _db = db;
                _emailService = emailService;
            }

            public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                var users = await _db.Delegates
                    .Where(e => request.UserIds.Contains(e.Id))
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
                        RegistrationStatus = (GoRegister.ApplicationCore.Data.Enums.RegistrationStatus)e.RegistrationStatusId
                    })
                    .ToListAsync();

                var email = await _db.Emails
                    .Include(e => e.EmailTemplates)
                        .ThenInclude(e => e.RegistrationTypes)
                    .Include(e => e.EmailLayout)
                    .Where(e => e.Id == request.EmailId)
                    .FirstOrDefaultAsync();

                var batchId = await _emailService.GenerateEmails(email, users);
                return Result.Ok(batchId);
            }
        }
    }
}
