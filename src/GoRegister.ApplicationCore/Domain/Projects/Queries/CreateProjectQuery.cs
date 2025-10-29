/*  MRF Changes : Bind Client and Domain lists, Path prefix value
    Modified Date : 16th September 2022
    Modified By : Mandar.Khade @amexgbt.com
    Team member : Harish.Rame @amexgbt.com
    JIRA Ticket No : GoRegister/GOR-213   

    MRF Changes : Added function to get client UID
    Modified Date : 17th October 2022
    Modified By : Mandar.Khade @amexgbt.com
    Team member : Harish.Rame @amexgbt.com
    JIRA Ticket No : GoRegister/GOR-238-New
 */

using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Domain.Clients;
using GoRegister.ApplicationCore.Domain.Domains;
using GoRegister.ApplicationCore.Domain.Projects.Commands.CreateProject;
using GoRegister.ApplicationCore.Domain.Projects.Services;
using GoRegister.ApplicationCore.Extensions;
using GoRegister.ApplicationCore.Framework.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Projects.Queries
{
    public class CreateProjectQuery : IRequest<Result<CreateProjectCommand>>
    {
        public int? CloneProjectId { get; set; }
        public bool IsTemplate { get; set; }
        public int ClientIdMRF { get; set; }

        public class Handler : PopulateViewModelHandlerBase<CreateProjectCommand>, IRequestHandler<CreateProjectQuery, Result<CreateProjectCommand>>
        {
            private readonly IProjectService _projectService;
            private readonly ApplicationDbContext _db;
            private readonly IClientService _clientService;

            private readonly IDomainService _domainService;

            public Handler(IProjectService projectService, ApplicationDbContext db, IClientService clientService, IDomainService domainService)
            {
                _projectService = projectService;
                _db = db;
                _clientService = clientService;
                _domainService = domainService;
                
            }

            public async Task<Result<CreateProjectCommand>> Handle(CreateProjectQuery request, CancellationToken cancellationToken)
            {
                var model = new CreateProjectCommand();
                model.CloneProjectId = request.CloneProjectId;

                if (request.IsTemplate)
                    model.ProjectType = Data.Enums.ProjectTypeEnum.Template;

                // set some sensible clone defaults
                model.CloneModel.CloneTheme = true;
                model.CloneModel.CloneRegistration = true;
                model.CloneModel.CloneRegistrationTypes = true;
                model.CloneModel.ClonePages = true;
                model.CloneModel.CloneMenuItems = true;

                model.ClientId = request.ClientIdMRF;

                await PopulateViewModel(model);
                return Result.Ok(model);
            }

            public override async Task PopulateViewModel(CreateProjectCommand model)
            {
                if (model.CloneProjectId.HasValue)
                {
                    var cloneProject = await _db.Projects.FindAsync(model.CloneProjectId.Value);
                    model.CloneProjectName = cloneProject?.Name;
                }
                else
                {
                    // we only need the templates if we're not cloning
                    model.ProjectTemplates = await _db.Projects
                        .Where(p => p.ProjectType == Data.Enums.ProjectTypeEnum.Template)
                        .Select(p => new SelectListItem(p.Name, p.Id.ToString()))
                        .ToListAsync();
                }

                model.TimeZones.GetTimeZoneList();
                model.ClientList = await _clientService.GetClientDropdownList();
                              
                if (model.IsTemplate == false)
                {
                    model.ClientListMRF = await _clientService.GetClientDropdownListMRF((int)model.ClientId);                               
                }

                model.DomainsListMRF = await _domainService.GetDomainDropdownListMRF();
                              
                var client = await _clientService.GetClientById((int)model.ClientId);

                if (client != null)
                {
                    model.PathPrefix = client.ClientUuid;
                }

            }
        }
    }
}
