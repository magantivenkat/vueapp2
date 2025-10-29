using AutoMapper;
using FluentValidation;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Framework.Domain;
using GoRegister.ApplicationCore.Framework.Domain.Mediatr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Emails
{
    public static class CreateEditEmail
    {
        public abstract class EmailModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string EmailTypeName => EmailType.ToString();
            public EmailType EmailType { get; set; }
            public bool IsEnabled { get; set; }
            public string Subject { get; set; }
            public string Cc { get; set; }
            public string Bcc { get; set; }
            public int DefaultTemplateIndex { get; set; }
            public List<EmailTemplateModel> Templates { get; set; } = new List<EmailTemplateModel>();
        }

        public class EmailTemplateModel
        {
            public int? Id { get; set; }
            public string BodyHtml { get; set; }
            public bool HasTextBody { get; set; }
            public string BodyText { get; set; }
            public IEnumerable<int> RegistrationTypes { get; set; } = new HashSet<int>();
        }

        public class EmailViewModel
        {
            public IEnumerable<NameValueModel> RegistrationTypes { get; set; }
            public IEnumerable<NameValueModel> Layouts { get; set; }
        }


        public class Response : IRequest<int>
        {
            public Command Model { get; set; }
            public EmailViewModel ViewModel { get; set; }
        }

        public abstract class QueryHandlerBase
        {
            private readonly ApplicationDbContext _db;

            public QueryHandlerBase(ApplicationDbContext db)
            {
                _db = db;
            }

            protected async Task<EmailViewModel> GetViewModel()
            {
                var vm = new EmailViewModel();
                vm.RegistrationTypes = (await _db.RegistrationTypes.ToListAsync()).Select(e => new NameValueModel(e.Id, e.Name));
                vm.Layouts = (await _db.EmailLayouts.Include(et => et.Project).Where(p => p.ProjectId == _db.Emails.FirstOrDefault().ProjectId).ToListAsync()).Select(et => new NameValueModel(et.Id, et.Name));
                return vm;
            }
        }

        public class CommandHandler : IRequestHandler<Command, Result<int>>
        {
            private readonly ApplicationDbContext _db;
            private readonly IMapper _mapper;

            public CommandHandler(ApplicationDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<Result<int>> Handle(Command request, CancellationToken cancellationToken)
            {
                // validate
                if (!request.Templates.Any())
                {
                    return Result.Invalid<int>("An email must have at least one template");
                }

                Email email;
                if (request.Id == 0)
                {
                    // check an existing template doesn't exist if emailtype is not custom
                    if (request.EmailType != EmailType.Custom)
                    {
                        var existing = await _db.Emails.FirstOrDefaultAsync(e => e.EmailType == request.EmailType);
                        if (existing != null)
                        {
                            return Result.Invalid<int>($"A {request.EmailType} email already exists for this project, please edit the existing one.");
                        }
                    }

                    email = new Email();
                    email.EmailType = request.EmailType;
                    _db.Emails.Add(email);
                }
                else
                {
                    email = await _db.Emails
                        .Include(e => e.EmailTemplates)
                            .ThenInclude(e => e.RegistrationTypes)
                        .SingleOrDefaultAsync(e => e.Id == request.Id);

                    if (email == null)
                    {
                        return Result.NotFound<int>($"Email with id {request.Id} could not be found");
                    }
                }

                _mapper.Map(request, email);
                var templates = new List<EmailTemplate>();
                int i = 0;
                foreach (var template in request.Templates)
                {

                    EmailTemplate existingTemplate;
                    if (template.Id == null || template.Id.Value == 0)
                    {
                        existingTemplate = new EmailTemplate();
                        email.EmailTemplates.Add(existingTemplate);
                    }
                    else
                    {
                        existingTemplate = email.EmailTemplates.SingleOrDefault(e => e.Id == template.Id.Value);
                        if (existingTemplate == null)
                        {
                            return Result.NotFound<int>("One of the templates for this email could not be found, please refresh the page and try again");
                        }

                        _db.Entry(existingTemplate).State = EntityState.Modified;
                    }


                    existingTemplate.IsDefault = i == request.DefaultTemplateIndex;

                    _mapper.Map(template, existingTemplate);
                    existingTemplate.RegistrationTypes = template.RegistrationTypes.Select(e => new EmailTemplateRegistrationType()
                    {
                        EmailTemplate = existingTemplate,
                        RegistrationTypeId = e
                    }).ToList();
                    i++;
                }

                await _db.SaveChangesAsync();
                return Result.Ok(email.Id);
            }
        }

        public class Command : EmailModel, IRequest<Result<int>>, IValidatable
        {
            public int Id { get; set; }
            public bool IsValidated { get; set; }
            public int LayoutId { get; set; }

            public class Validator : AbstractValidator<Command>
            {

            }
        }
    }
}
