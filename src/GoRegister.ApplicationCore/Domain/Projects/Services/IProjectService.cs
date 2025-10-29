/*  MRF Changes: Add new function "AddMissingProjectDefaultsMRF" to get custom field details
    Modified Date : 31st October 2022
    Modified By : Mandar.Khade @amexgbt.com
    Team member : Harish.Rame @amexgbt.com
    JIRA Ticket No : GoRegister/GOR-228 */

using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Projects.Models;
using GoRegister.ApplicationCore.Framework.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Extensions;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using GoRegister.ApplicationCore.Data.Models.Fields;

namespace GoRegister.ApplicationCore.Domain.Projects.Services
{
    public interface IProjectService
    {
        Task<Result<Project>> GetClonedProject(int projectId, CloneProjectModel model);
        bool ValidateSubdomain(string subdomain, string[] disallowedSubdomains);
        void AddMissingProjectDefaults(Project project, CloneProjectModel cloneModel);
        Task<string> GenerateProjectCodeAsync();
        Task AddProjectToRecentList(int projectId, int userId);
        void AddMissingProjectDefaultsMRF(Project project, CloneProjectModel cloneModel, string prefix);
    }

    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public ProjectService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<Result<Project>> GetClonedProject(int projectId, CloneProjectModel model)
        {
            var projectQuery = _context.Projects
                .AsNoTracking()
                .IgnoreQueryFilters()
                .AsQueryable();

            if (model.ClonePages)
            {
                projectQuery = projectQuery
                    .Include(e => e.CustomPages)
                    .ThenInclude(e => e.CustomPageRegistrationStatuses);

                projectQuery = projectQuery
                    .Include(e => e.CustomPages)
                    .ThenInclude(e => e.ProjectPage);

                if (model.CloneRegistrationTypes)
                {
                    projectQuery = projectQuery
                        .Include(e => e.CustomPages)
                        .ThenInclude(e => e.CustomPageRegistrationTypes);
                }
            }

            if (model.CloneRegistrationTypes)
            {
                projectQuery = projectQuery
                    .Include(e => e.RegistrationPaths)
                        .ThenInclude(e => e.RegistrationTypes);
            }

            if (model.CloneSessions)
            {
                projectQuery = projectQuery
                    .Include(p => p.SessionCategories)
                        .ThenInclude(sc => sc.Sessions);
            }

            var project = await projectQuery
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                return Result.NotFound<Project>("Project not found");
            }

            var regTypeMap = project.RegistrationPaths.SelectMany(rp => rp.RegistrationTypes).ToDictionary(rt => rt.Id, rt => rt);

            if (model.CloneRegistration)
            {
                var formsQuery = _context.Forms
                    .Include(f => f.RegistrationPages)
                        .ThenInclude(rp => rp.Fields)
                    .Include(f => f.RegistrationPages)
                        .ThenInclude(rp => rp.Fields)
                            .ThenInclude(f => f.FieldOptions)
                    .Include(f => f.RegistrationPages)
                        .ThenInclude(rp => rp.Fields)
                            .ThenInclude(f => f.FieldOptionRules)
                    .Include(f => f.RegistrationPages)
                        .ThenInclude(rp => rp.Fields)
                            .ThenInclude(f => (f as SessionField).SessionFieldCategories)
                    .AsQueryable();

                if (model.CloneRegistrationTypes)
                {
                    formsQuery = formsQuery
                        .Include(f => f.RegistrationPages)
                            .ThenInclude(rp => rp.Fields)
                                .ThenInclude(f => f.RegistrationTypeFields);
                }

                var forms = await formsQuery
                    .Where(f => f.ProjectId == projectId)
                    .AsNoTracking()
                    .IgnoreQueryFilters()
                    .ToListAsync();


                // reset forms
                foreach (var form in forms)
                {
                    var fieldMap = form.RegistrationPages.SelectMany(rp => rp.Fields).ToDictionary(f => f.Id, f => f);
                    var optionMap = form.RegistrationPages
                        .SelectMany(rp => rp.Fields)
                        .SelectMany(f => f.FieldOptions)
                        .ToDictionary(fo => fo.Id, fo => fo);

                    _context.DetachEntity(form);
                    DetachAndAssignProject(form.RegistrationPages, project);
                    foreach (var regPage in form.RegistrationPages)
                    {
                        DetachAndAssignProject(regPage, project);
                        foreach (var field in regPage.Fields)
                        {
                            DetachAndAssignProject(field, project);

                            if (field.FieldTypeId == FieldTypeEnum.Session)
                            {
                                var sessionField = field as SessionField;

                                if (model.CloneSessions)
                                {
                                    _context.DetachEntities(sessionField.SessionFieldCategories);

                                    // map category
                                    foreach (var sfc in sessionField.SessionFieldCategories)
                                    {
                                        sfc.SessionCategory = project.SessionCategories.First(sc => sc.Id == sfc.SessionCategoryId);
                                    }
                                }
                                else
                                {
                                    sessionField.SessionFieldCategories = new HashSet<SessionFieldCategory>();
                                }

                            }

                            field.UniqueIdentifier = Guid.NewGuid();
                            DetachAndAssignProject(field.RegistrationTypeFields, project);
                            DetachAndAssignProject(field.FieldOptionRules, project);

                            foreach (var rule in field.FieldOptionRules)
                            {
                                if (rule.NextFieldId.HasValue)
                                {
                                    rule.NextField = fieldMap[rule.NextFieldId.Value];
                                }

                                if (rule.NextFieldOptionId.HasValue)
                                {
                                    rule.NextFieldOption = optionMap[rule.NextFieldOptionId.Value];
                                }

                                rule.FieldOption = optionMap[rule.FieldOptionId];
                            }

                            // reassign reg type links to new reg type
                            if (model.CloneRegistrationTypes)
                            {
                                foreach (var rtf in field.RegistrationTypeFields)
                                {
                                    rtf.RegistrationType = regTypeMap[rtf.RegistrationTypeId];
                                }
                            }

                            foreach (var fo in field.FieldOptions)
                            {
                                DetachAndAssignProject(fo, project);
                            }
                        }
                    }
                }

                // assign forms to our detached project
                project.Forms = forms;
            }

            if (model.CloneTheme)
            {
                var projectTheme = await _context.ProjectThemes
                    .OrderByDescending(pt => pt.DateUpdated)
                    .AsNoTracking()
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(pt => pt.ProjectId == projectId);

                projectTheme.DateUpdated = SystemTime.UtcNow;
                projectTheme.ThemeGuid = Guid.NewGuid();
                projectTheme.Project = project;
                project.ProjectThemes.Add(projectTheme);
            }

            // reassign reg type links for custom pages
            if (model.CloneRegistrationTypes && model.ClonePages)
            {
                foreach (var cprt in project.CustomPages.SelectMany(cp => cp.CustomPageRegistrationTypes))
                {
                    var rt = regTypeMap[cprt.RegistrationTypeId];
                    cprt.RegistrationType = rt;
                }
            }

            var menuItems = new List<MenuItem>();
            if (model.CloneMenuItems)
            {
                // get cloned project's current menu items
                var menuItemQuery = _context.MenuItems
                    .Where(i => i.ProjectId == projectId)
                    .AsNoTracking()
                    .IgnoreQueryFilters();

                if(model.CloneRegistrationTypes)
                {
                    menuItemQuery.Include(mi => mi.MenuItemRegistrationTypes);
                }

                if(!model.ClonePages)
                {
                    menuItemQuery.Where(mi => mi.MenuItemType != MenuItemType.CustomPage);
                }

                var menuItemList = await menuItemQuery.ToListAsync();

                if(model.ClonePages)
                {
                    foreach (var item in menuItemList.Where(i => i.MenuItemType == MenuItemType.CustomPage))
                    {
                        item.CustomPage = project.CustomPages.Single(cp => cp.Id == item.CustomPageId);
                    }
                }

                if (model.CloneRegistrationTypes)
                {
                    // filter menuitems that aren't custom pages
                    foreach (var mirt in menuItemList.Where(i => i.CustomPageId == null).SelectMany(mi => mi.MenuItemRegistrationTypes))
                    {
                        var rt = regTypeMap[mirt.RegistrationTypeId];
                        rt.Id = 0;
                        mirt.RegistrationType = rt;
                    }
                }

                menuItems.AddRange(menuItemList);
            }

            // reset the project ids and uuids
            // project bits
            _context.DetachEntity(project);
            project.UniqueId = Guid.NewGuid();
            _context.DetachEntities(project.RegistrationPaths);
            DetachAndAssignProject(project.RegistrationPaths?.SelectMany(rp => rp.RegistrationTypes), project);
            //DetachAndAssignProject(project.RegistrationTypes, project);

            // themes
            _context.DetachEntities(project.ProjectThemes);

            // custom pages
            DetachAndAssignProject(project.CustomPages, project);
            DetachAndAssignProject(project.CustomPages?.Select(cp => cp.ProjectPage), project);
            _context.DetachEntities(project.CustomPages?.SelectMany(cp => cp.CustomPageRegistrationStatuses));
            _context.DetachEntities(project.CustomPages?.SelectMany(cp => cp.CustomPageRegistrationTypes));

            // menu items
            DetachAndAssignProject(menuItems, project);
            DetachAndAssignProject(menuItems?.SelectMany(mi => mi.MenuItemRegistrationTypes), project);
            _context.AddRange(menuItems);
            
            // sessions
            DetachAndAssignProject(project.SessionCategories, project);
            DetachAndAssignProject(project.SessionCategories?.SelectMany(sc => sc.Sessions), project);

            return Result.Ok(project);
        }

        public async Task<string> GenerateProjectCodeAsync()
        {
            var code = "";
            var isCodeInUse = true;

            while (isCodeInUse)
            {
                code = string.Empty;
                code = code.GenerateRandomString(6, false, true, false, false);
                isCodeInUse = await _context.Projects.AnyAsync(p => p.Code.ToUpper() == code.ToUpper());
            }

            return code;
        }

        private void DetachAndAssignProject<T>(T entity, Project project) where T : class, IMustHaveProject
        {
            _context.DetachEntity(entity);
            entity.Project = project;
        }

        private void DetachAndAssignProject<T>(IEnumerable<T> entities, Project project) where T : class, IMustHaveProject
        {
            foreach (var entity in entities)
            {
                DetachAndAssignProject(entity, project);
            }
        }

        public void AddMissingProjectDefaults(Project project, CloneProjectModel cloneModel)
        {
            // has no reg paths or types
            if (!project.RegistrationPaths.Any())
            {
                project.RegistrationPaths.Add(GetDefaultRegistrationPath(project));
            }

            // has no reg form
            if (!project.Forms.Any(e => e.FormTypeId == FormType.Registration))
            {
                project.Forms.Add(GetDefaultRegistrationForm(project));
            }

            // has no cancel form
            if (!project.Forms.Any(e => e.FormTypeId == FormType.Cancel))
            {
                project.Forms.Add(GetDefaultCancellationForm(project));
            }

            // has no decline form
            if (!project.Forms.Any(e => e.FormTypeId == FormType.Decline))
            {
                project.Forms.Add(GetDefaultDeclineForm(project));
            }

            // add default homepage
            if (!project.CustomPages.Any(cp => cp.PageType == PageType.CustomPage))
            {
                project.CustomPages.Add(GetDefaultHomePage(project));
                //project.CustomPages.Add(GetDefaultMRFPage(project));
                
            }

            // add default menu items
            if (!cloneModel.CloneMenuItems)
            {
                //SetDefaultMenuItems(project);
            }
        }


        private Form GetDefaultRegistrationForm(Project project)
        {
            return new Form
            {
                Project = project,
                FormTypeId = FormType.Registration,
                
                RegistrationPages = new List<RegistrationPage> {
                    new RegistrationPage
                    {
                        Project = project,
                        Title = "Client Contact Information",
                        Fields = new List<Field>
                        {
                            //new FirstNameField() { Project = project, Name = "First Name", DataTag = "FirstName", SortOrder = 1, IsMandatory = true },
                            //new LastNameField { Project = project, Name = "Last Name", DataTag = "LastName", SortOrder = 2, IsMandatory = true },
                            //new EmailField { Project = project, Name = "Email", DataTag = "Email", SortOrder = 3, IsMandatory = true }

                            new SubHeaderField { Project = project, Name = "Client Contact Information", ReportingHeader = "Client Contact Information", SortOrder = 1  },
                            new HorizontalRuleField { Project = project,  Name = "Client Info Divider", SortOrder = 2  },

                            new FirstNameField() { Project = project, Name = "Contact First Name", DataTag = "FirstName", SortOrder = 3, IsMandatory = true },
                            new LastNameField { Project = project, Name = "Contact Last Name", DataTag = "LastName", SortOrder = 4, IsMandatory = true },
                            new EmailField { Project = project, Name = "Contact email address", DataTag = "Email", SortOrder = 5, IsMandatory = true },
                            new TextField { Project = project, Name = "Company Name", DataTag = "CompanyName", SortOrder = 6, IsMandatory = true },
                            new TextField { Project = project, Name = "Contact phone number", DataTag = "Contactphonenumber", SortOrder = 7, IsMandatory = true },
                            new CountryField { Project = project, Name = "Servicing Country", DataTag = "ServicingCountry", SortOrder = 8, IsMandatory = true },

                            new SubHeaderField { Project = project, Name = "Event Information", ReportingHeader = "Event Information", SortOrder = 9  },
                            new HorizontalRuleField { Project = project, Name = "Event Info Divider", SortOrder = 10  },

                            new TextField { Project = project, Name = "Event Name", DataTag = "EventName", SortOrder = 11, IsMandatory = true },
                            new TextField { Project = project, Name = "Number of Attendees", DataTag = "NumberofAttendees", SortOrder = 12, IsMandatory = true },
                            new TextField { Project = project, Name = "Destination", DataTag = "Destination", SortOrder = 13, IsMandatory = true },
                            new DateField { Project = project, Name = "Event Start Date", DataTag = "EventStartDate", SortOrder = 14, IsMandatory = true },
                            new DateField { Project = project, Name = "Event End Date", DataTag = "EventEndDate", SortOrder = 15, IsMandatory = true },
                            new SingleSelectField { Project = project, Name = "Are your dates or destination flexible?", DataTag = "Areyourdatesordestinationflexible?", SortOrder = 16, IsMandatory = true, SingleSelectType = SingleSelectField.SingleSelectTypeEnum.Select,  FieldOptions = new List<FieldOption>(){ new FieldOption { Description="Yes", Project = project, SortOrder = 0 }, new FieldOption { Description = "No", Project = project, SortOrder = 1 } }  },
                            new SingleSelectField { Project = project, Name = "Meeting Type", DataTag = "MeetingType?", SortOrder = 17, IsMandatory = true, SingleSelectType = SingleSelectField.SingleSelectTypeEnum.Select,  FieldOptions = new List<FieldOption>(){ new FieldOption { Description="Advisory Board", Project = project, SortOrder = 0 }, 
                                                                                                                                                                                                                                                                       new FieldOption { Description = "Air Only", Project = project, SortOrder = 1 },
                                                                                                                                                                                                                                                                       new FieldOption { Description = "Award/Incentive", Project = project, SortOrder = 2 },
                                                                                                                                                                                                                                                                        new FieldOption { Description = "Board of Dir/Executive", Project = project, SortOrder = 3 },
                                                                                                                                                                                                                                                                        new FieldOption { Description = "Congress", Project = project, SortOrder = 4 },
                                                                                                                                                                                                                                                                        new FieldOption { Description = "Dinner", Project = project, SortOrder = 5 },
                                                                                                                                                                                                                                                                        new FieldOption { Description = "Internal Meetings", Project = project, SortOrder = 6 },
                                                                                                                                                                                                                                                                        new FieldOption { Description = "Investigator/Preceptors", Project = project, SortOrder = 7 },
                                                                                                                                                                                                                                                                        new FieldOption { Description = "Management", Project = project, SortOrder = 8 },
                                                                                                                                                                                                                                                                        new FieldOption { Description = "Product Launch", Project = project, SortOrder = 9 },
                                                                                                                                                                                                                                                                        new FieldOption { Description = "Recruiting", Project = project, SortOrder = 10 },
                                                                                                                                                                                                                                                                        new FieldOption { Description = "Sales/Marketing", Project = project, SortOrder = 11 },
                                                                                                                                                                                                                                                                        new FieldOption { Description = "Special Event", Project = project, SortOrder = 12 },
                                                                                                                                                                                                                                                                        new FieldOption { Description = "Trade Show", Project = project, SortOrder = 13 },
                                                                                                                                                                                                                                                                        new FieldOption { Description = "Training", Project = project, SortOrder = 14 },
                                                                                                                                                                                                                                                                        new FieldOption { Description = "Other", Project = project, SortOrder = 15 }
                                                                                                                                                                                                                                                                        } },

                            new SingleSelectField { Project = project, Name = "Meeting Format", DataTag = "MeetingFormat", SortOrder = 18, IsMandatory = true, SingleSelectType = SingleSelectField.SingleSelectTypeEnum.Select,  FieldOptions = new List<FieldOption>(){ new FieldOption { Description="Virtual", Project = project, SortOrder = 0 }, new FieldOption { Description = "In Person", Project = project, SortOrder = 1 }, new FieldOption { Description = "Hybrid", Project = project, SortOrder = 2 } }  },
                            new TextField { Project = project, Name = "Additional Information", DataTag = "AdditionalInformation", SortOrder = 19, IsMandatory = false },
                         }
                    }
                }
            };
        }

        private Form GetDefaultCancellationForm(Project project)
        {
            return new Form
            {
                Project = project,
                FormTypeId = FormType.Cancel,
                SubmitButtonText = "Cancel",
                IsReviewPageHidden = true,
                RegistrationPages = new List<RegistrationPage> {
                    new RegistrationPage
                    {
                        Project = project,
                        Title = "Cancellation",
                        Fields = new List<Field>
                        {
                            new HeaderField() { Project = project, Name = "Cancel Registration" },
                            new HtmlField() { Project = project, Name = "<p>Are you sure you wish to cancel?</p>" },
                            new FirstNameField() { Project = project, Name = "First Name", DataTag = "FirstName", IsReadOnly = true },
                            new LastNameField { Project = project, Name = "Last Name", DataTag = "LastName", IsReadOnly = true },
                            new EmailField { Project = project, Name = "Email", DataTag = "Email", IsReadOnly = true }
                        }
                    }
                }
            };
        }

        private Form GetDefaultDeclineForm(Project project)
        {
            return new Form
            {
                Project = project,
                FormTypeId = FormType.Decline,
                SubmitButtonText = "Decline",
                IsReviewPageHidden = true,
                RegistrationPages = new List<RegistrationPage> {
                    new RegistrationPage
                    {
                        Project = project,
                        Title = "Decline",
                        Fields = new List<Field>
                        {
                            new HeaderField() { Project = project, Name = "Decline Registration" },
                            new HtmlField() { Project = project, Name = "<p>Are you sure you wish to decline?</p>" },
                            new FirstNameField() { Project = project, Name = "First Name", DataTag = "FirstName", IsReadOnly = true },
                            new LastNameField { Project = project, Name = "Last Name", DataTag = "LastName", IsReadOnly = true },
                            new EmailField { Project = project, Name = "Email", DataTag = "Email", IsReadOnly = true }
                        }
                    }
                }
            };
        }

        private RegistrationPath GetDefaultRegistrationPath(Project project)
        {
            return new RegistrationPath
            {
                Name = "Default",
                CanCancel = false,
                CanModify = false,
                Project = project,
                DateCreated = SystemTime.UtcNow,
                NotInvitedText = _configuration.GetValue<string>("DefaultRegistrationPath:NotInvitedText"),
                InvitedText = _configuration.GetValue<string>("DefaultRegistrationPath:InvitedText"),
                ConfirmedText = _configuration.GetValue<string>("DefaultRegistrationPath:ConfirmedText"),
                DeclinedText = _configuration.GetValue<string>("DefaultRegistrationPath:DeclinedText"),
                CancelledText = _configuration.GetValue<string>("DefaultRegistrationPath:CancelledText"),
                RegistrationTypes = new List<RegistrationType>
                {
                    GetDefaultRegistrationType(project)
                }
            };
        }

        private RegistrationType GetDefaultRegistrationType(Project project)
        {
            return new RegistrationType
            {
                Name = "Default",
                DateCreated = SystemTime.UtcNow,
                Project = project,
                Capacity = 0
            };
        }

        private CustomPage GetDefaultHomePage(Project project)
        {
            var customPage = new CustomPage
            {
                Title = "Home",
                PageType = PageType.HomePage,
                Content = "Welcome, this is the home page",

                Position = 1,
                IsVisible = true,
                CustomPageRegistrationTypes = new List<CustomPageRegistrationType>(),
                ProjectPage = new ProjectPage
                {
                    MenuPosition = 1,
                    Type = ProjectPage.PageType.Custom,
                    Project = project
                }
            };

            //_context.MenuItems.AddRange(new List<MenuItem>
            //{
            //    new MenuItem { Label = "Home", MenuItemType = MenuItemType.CustomPage, Order = 0, CustomPage = customPage, Project = project },
            //});

            return customPage;
        }

        private void SetDefaultMenuItems(Project project)
        {
            _context.MenuItems.AddRange(new List<MenuItem>
            {
                new MenuItem { Label = "Register", MenuItemType = MenuItemType.Register, Order = 1, Project = project },
                new MenuItem { Label = "View Registration", MenuItemType = MenuItemType.ViewRegistration, Order = 2, Project = project },
            });
        }

        public bool ValidateSubdomain(string subdomain, string[] disallowedSubdomains)
        {
            return true;
        }

        public async Task AddProjectToRecentList(int projectId, int userId)
        {
            var userProjects = await _context.RecentProjects
                .Include(p => p.Project)
                .Where(p => p.User.Id == userId)
                .OrderBy(p => p.DateVisited)
                .ToListAsync();

            // project does not exist, add project and remove oldest (only keeping 5 in db)
            if (!userProjects.Any(p => p.Project.Id == projectId))
            {
                var project = await _context.Projects.SingleOrDefaultAsync(p => p.Id == projectId);
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);

                _context.RecentProjects.Add(new RecentProject { Project = project, User = user });

                if (userProjects.Count > 4)
                    _context.Remove(userProjects[0]);

                _context.SaveChanges();
            }

        }

        private CustomPage GetDefaultMRFPage(Project project)
        {
            var customPage = new CustomPage
            {
                Title = "Client Contact Form",
                PageType = PageType.CustomPage,
                //Content = "Welcome, this is the home page",

                Position = 1,
                IsVisible = true,
                CustomPageRegistrationTypes = new List<CustomPageRegistrationType>(),
                ProjectPage = new ProjectPage
                {
                    MenuPosition = 1,
                    Type = ProjectPage.PageType.Custom,
                    Project = project
                }
            };

            _context.MenuItems.AddRange(new List<MenuItem>
            {
                new MenuItem { Label = "MRF", MenuItemType = MenuItemType.CustomPage, Order = 0, CustomPage = customPage, Project = project },
            });

            return customPage;
        }

        public void AddMissingProjectDefaultsMRF(Project project, CloneProjectModel cloneModel, string prefix)
        {
            // has no reg paths or types
            if (!project.RegistrationPaths.Any())
            {
                project.RegistrationPaths.Add(GetDefaultRegistrationPath(project));
            }

            // has no reg form
            if (!project.Forms.Any(e => e.FormTypeId == FormType.Registration))
            {
                project.Forms.Add(GetDefaultRegistrationFormMRF(project, prefix));
            }

            // has no cancel form
            if (!project.Forms.Any(e => e.FormTypeId == FormType.Cancel))
            {
                project.Forms.Add(GetDefaultCancellationForm(project));
            }

            // has no decline form
            if (!project.Forms.Any(e => e.FormTypeId == FormType.Decline))
            {
                project.Forms.Add(GetDefaultDeclineForm(project));
            }

            // add default homepage
            if (!project.CustomPages.Any(cp => cp.PageType == PageType.CustomPage))
            {
                project.CustomPages.Add(GetDefaultHomePage(project));
                //project.CustomPages.Add(GetDefaultMRFPage(project));

            }

            // add default menu items
            if (!cloneModel.CloneMenuItems)
            {
                //SetDefaultMenuItems(project);
            }
        }

        private Form GetDefaultRegistrationFormMRF(Project project, string prefix)
        {

            Form form = new Form();

            form.Project = project;
            form.FormTypeId = FormType.Registration;

            List<RegistrationPage> registrationPagesList = new List<RegistrationPage>();

            RegistrationPage registrationPage = new RegistrationPage();

            registrationPage.Project = project;
            registrationPage.Title = "Client Contact Information";

            List<Field> fieldList = new List<Field>();

            SubHeaderField subHeaderField = new SubHeaderField();
            subHeaderField.Project = project;
            subHeaderField.Name = "Client Contact Information";
            subHeaderField.ReportingHeader = "Client Contact Information";
            subHeaderField.SortOrder = 1;
            subHeaderField.IsStandardField = true;
            fieldList.Add(subHeaderField);

            HorizontalRuleField horizontalRuleField = new HorizontalRuleField();
            horizontalRuleField.Project = project;
            horizontalRuleField.Name = "Client Info Divider";
            horizontalRuleField.SortOrder = 2;
            horizontalRuleField.IsStandardField = true;
            fieldList.Add(horizontalRuleField);

            FirstNameField firstNameField = new FirstNameField();
            firstNameField.Project = project;
            firstNameField.Name = "Contact First Name";
            firstNameField.DataTag = "FirstName";
            firstNameField.SortOrder = 3;
            firstNameField.IsMandatory = true;
            firstNameField.IsStandardField = true;
            fieldList.Add(firstNameField);

            LastNameField lastNameField = new LastNameField();
            lastNameField.Project = project;
            lastNameField.Name = "Contact Last Name";
            lastNameField.DataTag = "LastName";
            lastNameField.SortOrder = 4;
            lastNameField.IsMandatory = true;
            lastNameField.IsStandardField = true;
            fieldList.Add(lastNameField);

            EmailField emailField = new EmailField();
            emailField.Project = project;
            emailField.Name = "Contact email address";
            emailField.DataTag = "Email";
            emailField.SortOrder = 5;
            emailField.IsMandatory = true;
            emailField.IsStandardField = true;
            fieldList.Add(emailField);

            TextField textField = new TextField();
            textField.Project = project;
            textField.Name = "Company Name";
            textField.DataTag = "CompanyName";
            textField.SortOrder = 6;
            textField.IsMandatory = true;
            textField.IsReadOnly = true;
            textField.IsStandardField = true;
            textField.DefaultValue = project.Client.Name;
            fieldList.Add(textField);

            textField = new TextField();
            textField.Project = project;
            textField.Name = "Contact phone number";
            textField.DataTag = "Contactphonenumber";
            textField.SortOrder = 7;
            textField.IsMandatory = true;
            textField.IsStandardField = true;
            fieldList.Add(textField);

            MRFServicingCountryField servicingCountryField = new MRFServicingCountryField();
            servicingCountryField.Project = project;
            servicingCountryField.Name = "Servicing Country";
            servicingCountryField.DataTag = "ServicingCountry";
            servicingCountryField.SortOrder = 8;
            servicingCountryField.IsMandatory = true;
            servicingCountryField.IsStandardField = true;
            servicingCountryField.IsHidden = true;
            fieldList.Add(servicingCountryField);

            MRFRequestorCountryField requestorCountryField = new MRFRequestorCountryField();
            requestorCountryField.Project = project;
            requestorCountryField.Name = "Requestor Country";
            requestorCountryField.DataTag = "RequestorCountry";
            requestorCountryField.SortOrder = 9;
            requestorCountryField.IsMandatory = true;
            requestorCountryField.IsStandardField = true;
            fieldList.Add(requestorCountryField);
          
            subHeaderField = new SubHeaderField();
            subHeaderField.Project = project;
            subHeaderField.Name = "Event Information";
            subHeaderField.ReportingHeader = "Event Information";
            subHeaderField.SortOrder = 10;
            subHeaderField.IsStandardField = true;
            fieldList.Add(subHeaderField);

            horizontalRuleField = new HorizontalRuleField();
            horizontalRuleField.Project = project;
            horizontalRuleField.Name = "Event Info Divider";
            horizontalRuleField.SortOrder = 11;
            horizontalRuleField.IsStandardField = true;
            fieldList.Add(horizontalRuleField);

            textField = new TextField();
            textField.Project = project;
            textField.Name = "Event Name";
            textField.DataTag = "EventName";
            textField.SortOrder = 12;
            textField.IsMandatory = true;
            textField.IsStandardField = true;
            fieldList.Add(textField);

            textField = new TextField();
            textField.Project = project;
            textField.Name = "Number of Attendees";
            textField.DataTag = "NumberofAttendees";
            textField.SortOrder = 13;
            textField.IsMandatory = true;
            textField.IsStandardField = true;
            fieldList.Add(textField);
                      
            MRFDestinationField mrfDestinationField = new MRFDestinationField();
            mrfDestinationField.Project = project;
            mrfDestinationField.Name = "Destination";
            mrfDestinationField.DataTag = "DestinationExternalId";
            mrfDestinationField.SortOrder = 14;
            mrfDestinationField.IsMandatory = true;
            mrfDestinationField.IsStandardField = true;
            fieldList.Add(mrfDestinationField);

            DateField dateField = new DateField();
            dateField.Project = project;
            dateField.Name = "Event Start Date";
            dateField.DataTag = "EventStartDate";
            dateField.SortOrder = 15;
            dateField.IsMandatory = true;
            dateField.IsStandardField = true;
            fieldList.Add(dateField);

            dateField = new DateField();
            dateField.Project = project;
            dateField.Name = "Event End Date";
            dateField.DataTag = "EventEndDate";
            dateField.SortOrder = 16;
            dateField.IsMandatory = true;
            dateField.IsStandardField = true;
            fieldList.Add(dateField);

            SingleSelectField singleSelectField = new SingleSelectField();
            singleSelectField.Project = project;
            singleSelectField.Name = "Are your dates or destination flexible?";
            singleSelectField.DataTag = "Areyourdatesordestinationflexible?";
            singleSelectField.SortOrder = 17;
            singleSelectField.IsStandardField = true;
            singleSelectField.IsMandatory = true;

            singleSelectField.SingleSelectType = SingleSelectField.SingleSelectTypeEnum.Select;
            List<FieldOption> fieldOptionsList = new List<FieldOption>();
            FieldOption fieldOption = new FieldOption();
            fieldOption.Description = "Yes";
            fieldOption.AdditionalInformation = "Yes";
            fieldOption.Project = project;
            fieldOption.SortOrder = 0;
            fieldOptionsList.Add(fieldOption);

            fieldOption = new FieldOption();
            fieldOption.Description = "No";
            fieldOption.AdditionalInformation = "No";
            fieldOption.Project = project;
            fieldOption.SortOrder = 1;
            fieldOptionsList.Add(fieldOption);
            singleSelectField.FieldOptions = fieldOptionsList;
            fieldList.Add(singleSelectField);

            var accessToken = this._configuration.GetSection("APIDetails")["AccessToken"];
            var mrfMeetingTypeAPI = this._configuration.GetSection("APIDetails")["MRFMeetingTypeAPI"];

            singleSelectField = new SingleSelectField();
            singleSelectField.Project = project;
            singleSelectField.Name = "Meeting Type";
            singleSelectField.DataTag = "MeetingType";
            singleSelectField.SortOrder = 18;
            singleSelectField.IsMandatory = true;
            singleSelectField.IsStandardField = true;
            singleSelectField.SingleSelectType = SingleSelectField.SingleSelectTypeEnum.Select;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(mrfMeetingTypeAPI);
            client.DefaultRequestHeaders.Add("Authorization", accessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var response = client.GetAsync(client.BaseAddress))
            {
                if (response.Result.IsSuccessStatusCode)
                {
                    var fileJsonString = response.Result.Content.ReadAsStringAsync();

                    var model = JsonConvert.DeserializeObject<List<MRFSingleSelectFieldOptions>>(fileJsonString.Result);

                    fieldOptionsList = new List<FieldOption>();

                    int sortOrder = 0;
                    foreach (var option in model)
                    {
                        fieldOption = new FieldOption();
                        fieldOption.Project = project;
                        fieldOption.AdditionalInformation = option.UUId;
                        fieldOption.Description = option.Name;
                        fieldOption.SortOrder = sortOrder;
                        fieldOptionsList.Add(fieldOption);
                        sortOrder++;
                    }

                    singleSelectField.FieldOptions = fieldOptionsList;
                    fieldList.Add(singleSelectField);
                }
            }

            var mrfMeetingFormatsAPI = this._configuration.GetSection("APIDetails")["MRFMeetingFormatsAPI"];

            singleSelectField = new SingleSelectField();
            singleSelectField.Project = project;
            singleSelectField.Name = "Meeting Format";
            singleSelectField.DataTag = "MeetingFormat";
            singleSelectField.SortOrder = 19;
            singleSelectField.IsMandatory = true;
            singleSelectField.IsStandardField = true;
            singleSelectField.SingleSelectType = SingleSelectField.SingleSelectTypeEnum.Select;

            client = new HttpClient();
            client.BaseAddress = new Uri(mrfMeetingFormatsAPI);
            client.DefaultRequestHeaders.Add("Authorization", accessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var response = client.GetAsync(client.BaseAddress))
            {
                if (response.Result.IsSuccessStatusCode)
                {
                    var fileJsonString = response.Result.Content.ReadAsStringAsync();

                    var model = JsonConvert.DeserializeObject<List<MRFSingleSelectFieldOptions>>(fileJsonString.Result);

                    fieldOptionsList = new List<FieldOption>();

                    int sortOrder = 0;
                    foreach (var option in model)
                    {
                        fieldOption = new FieldOption();
                        fieldOption.Project = project;
                        fieldOption.AdditionalInformation = option.UUId;
                        fieldOption.Description = option.Name;
                        fieldOption.SortOrder = sortOrder;
                        fieldOptionsList.Add(fieldOption);
                        sortOrder++;
                    }

                    singleSelectField.FieldOptions = fieldOptionsList;
                    fieldList.Add(singleSelectField);
                }
            }                      
        
                       
            TextAreaField textAreaField = new TextAreaField();
            textAreaField.Project = project;
            textAreaField.Name = "Additional Information";
            textAreaField.DataTag = "AdditionalInformation";
            textAreaField.SortOrder = 19;
            textAreaField.IsStandardField = true;
            fieldList.Add(textAreaField);


            registrationPage.Fields = fieldList;
            registrationPagesList.Add(registrationPage);
            form.RegistrationPages = registrationPagesList;

            return form;

        }
    }
}
