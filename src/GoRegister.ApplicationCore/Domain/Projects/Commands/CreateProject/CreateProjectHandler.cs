/*  MRF Changes : Remove code depend on Custom Email functionality
    Modified Date : 16th September 2022
    Modified By : Mandar.Khade @amexgbt.com
    Team member : Harish.Rame @amexgbt.com
    JIRA Ticket No : GoRegister/GOR-213  
 
    MRF Changes : changes related to save prefix 
    Modified Date :  01st November 2022
    Modified By : Mandar.Khade @amexgbt.com
    Team member : Harish.Rame @amexgbt.com
    JIRA Ticket No : GoRegister/GOR-228
 
 */

using AutoMapper;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Services;
using GoRegister.ApplicationCore.Framework.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Data.Enums;
using Microsoft.Extensions.Configuration;
using MediatR;
using System.Threading;
using GoRegister.ApplicationCore.Domain.Projects.Services;
using GoRegister.ApplicationCore.Domain.Projects.Configuration;
using GoRegister.ApplicationCore.Domain.Projects.Enums;
using GoRegister.ApplicationCore.Framework.Identity;
using GoRegister.ApplicationCore.Data.Models.Emails;
using GoRegister.ApplicationCore.Helpers;

namespace GoRegister.ApplicationCore.Domain.Projects.Commands.CreateProject
{
    public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, Result<Project>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IProjectThemeService _projectThemeService;
        private readonly IConfiguration _configuration;
        private readonly IProjectService _projectService;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public CreateProjectHandler(ApplicationDbContext context, IMapper mapper, IProjectThemeService projectThemeService, IConfiguration configuration, IProjectService projectService, ICurrentUserAccessor currentUserAccessor)
        {
            _context = context;
            _mapper = mapper;
            _projectThemeService = projectThemeService;
            _configuration = configuration;
            _projectService = projectService;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<Result<Project>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var model = request;

            var client = await _context.Clients.SingleOrDefaultAsync(c => c.Id == model.ClientId);

            string host;
            string prefix = null;
            Expression<Func<Project, bool>> query;
            if (model.ProjectUrlFormat == Enums.ProjectUrlFormat.Subdomain)
            {
                var tenantUrl = _context.TenantUrls.FirstOrDefault(e => e.IsSubdomainHost && (e.ClientId == null));
                if (tenantUrl == null)
                {
                    return Result.Fail<Project>();
                }

                var subdomain = model.Subdomain.Trim();
                if (!_projectService.ValidateSubdomain(model.Subdomain, tenantUrl.DisallowedSubdomainsArray))
                {
                    return Result.Fail<Project>();
                }

                host = $"{subdomain}.{tenantUrl.Host}";
                query = (e => e.Host == host);
            }
            else
            {
                var tenantUrl = _context.TenantUrls.FirstOrDefault(e => !e.IsSubdomainHost && (e.ClientId == null));
                if (tenantUrl == null)
                {
                    return Result.Fail<Project>();
                }

                host = tenantUrl.Host;

                if (model.PathPrefix != null)
                {
                    prefix = model.PathPrefix.Trim();
                }
                else
                {
                    prefix = client.ClientUuid;
                }

                if (model.IsTemplate)
                {
                    query = (e => e.Host == tenantUrl.Host && e.Prefix == prefix && e.ProjectType == ProjectTypeEnum.Template);
                }
                else
                {
                    query = (e => e.Host == tenantUrl.Host && e.Prefix == prefix && e.ProjectType == ProjectTypeEnum.Project);
                }

            }

            if (await _context.Projects.AnyAsync(query))
            {
                return Result.Fail<Project>();
            }

            var emailSettings = _configuration.GetSection("ProjectEmailNotification").Get<ProjectEmailNotificationConfiguration>();
            
            Project project;
            var projectToCloneId = model.CloneProjectId ?? model.TemplateId;
            if (projectToCloneId.HasValue)
            {
                var clone = await _projectService.GetClonedProject(projectToCloneId.Value, model.CloneModel);
                if(clone.Failed)
                {
                    return Result.NotFound<Project>("Not found");
                }
                project = clone.Value;
            }
            else
            {
                project = new Project();
            }

            // after we clone we need to make sure that the project contains a default reg type, reg form etc.
            //_projectService.AddMissingProjectDefaults(project, model.CloneModel);
            project.Client = client;
            _projectService.AddMissingProjectDefaultsMRF(project, model.CloneModel, prefix);
            _mapper.Map(model, project);
            // set up the domain
            project.Host = host;
            project.Prefix = prefix;
            
            project.Code = await _projectService.GenerateProjectCodeAsync();

            // override some sensible defaults
            project.StatusId = ProjectStatus.Pending;
            project.UniqueId = Guid.NewGuid();
            project.DateCreated = SystemTime.UtcNow;
            // if project has no theme assign a default theme to it
            if(!project.ProjectThemes.Any())
            {
                var defaultTheme = await _projectThemeService.GetDefaultProjectTheme();
                project.ProjectThemes.Add(defaultTheme);
            }
            project.CreatedByUserId =_currentUserAccessor.GetUserId();

            _context.Add(project);

            await _context.SaveChangesAsync();

            var defaultEmailLayout = _context.EmailLayouts.Include(et => et.Project).FirstOrDefault(et => et.Project.Name.Trim() == Constants.DefaultEmailTemplateProjectName);
            EmailLayout mrfEmailLayout = null;

            if (defaultEmailLayout != null)
            {
                mrfEmailLayout = new EmailLayout
                {
                    ProjectId = project.Id,
                    Name = defaultEmailLayout.Name,
                    Html = defaultEmailLayout.Html
                };

                _context.EmailLayouts.Add(mrfEmailLayout);

                await _context.SaveChangesAsync();


                Email email = new Email();

                email.ProjectId = project.Id;
                email.Name = "Base MRF Email";
                email.Description = "Base MRF Email";
                email.EmailType = GoRegister.ApplicationCore.Data.Enums.EmailType.Custom;
                email.IsEnabled = true;
                email.Subject = "MRF Confirmation Email";
                email.Cc = "";
                email.Bcc = "";
                email.EmailLayoutId = mrfEmailLayout.Id;

                _context.Emails.Add(email);

                await _context.SaveChangesAsync();

                //var mrfFormEmailLogoPath = this._configuration.GetSection("MRFNotifications")["MRFFormEmailLogoPath"];
                //var mrfFormEmailBannerPath = this._configuration.GetSection("MRFNotifications")["MRFFormEmailBannerPath"];
                //var mrfFormEmailSignPath = this._configuration.GetSection("MRFNotifications")["MRFFormEmailSignPath"];

                //reading default email template

                var defaultEmailTemplate = _context.EmailTemplates.Include(et => et.Project).FirstOrDefault(et => et.Project.Name.Trim() == Constants.DefaultEmailTemplateProjectName);
                EmailTemplate mrfEmailTemplate = null;
                if (defaultEmailTemplate != null)
                {
                    mrfEmailTemplate = new EmailTemplate
                    {
                        ProjectId = project.Id,
                        BodyHtml = defaultEmailTemplate.BodyHtml,

                        HasTextBody = false,
                        BodyText = "",
                        EmailId = email.Id,
                        IsDefault = true
                    };
                    _context.EmailTemplates.Add(mrfEmailTemplate);

                    await _context.SaveChangesAsync();
                }
            }

            return Result.Ok(project);
        }
    }
}

