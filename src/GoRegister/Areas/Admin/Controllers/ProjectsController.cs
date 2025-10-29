/*  MRF Changes : Change redirect to action method to Theme page on succesful creation of MRF form
    Modified Date : 16th September 2022
    Modified By : Mandar.Khade @amexgbt.com
    Team member : Harish.Rame @amexgbt.com
    JIRA Ticket No : GoRegister/GOR-213, GOR-221   */

using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Clients;
using GoRegister.ApplicationCore.Domain.ProjectPages.Services;
using GoRegister.ApplicationCore.Domain.Projects.Commands.CreateProject;
using GoRegister.ApplicationCore.Domain.Projects.Models;
using GoRegister.ApplicationCore.Domain.Projects.Queries;
using GoRegister.ApplicationCore.Domain.Projects.Services;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Services;
using GoRegister.Filters;
using GoRegister.Framework.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Projects/[action]")]
    public class ProjectsController : AdminControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProjectService _projectService;
        private readonly IProjectPageService _projectPageService;
        private readonly IConfiguration _configuration;
        private readonly IProjectThemeService _projectThemeService;
        private readonly IClientService _clientService;
        private readonly IMediator _mediator;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectsController(ApplicationDbContext context,
            IProjectService projectService,
            IConfiguration configuration,
            IProjectThemeService projectThemeService,
            IClientService clientService,
            IProjectPageService projectPageService,
            IMediator mediator,
            IAuthorizationService authorizationService,             
             UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _projectService = projectService;
            _configuration = configuration;
            _projectThemeService = projectThemeService;
            _clientService = clientService;
            _projectPageService = projectPageService;
            _mediator = mediator;
            _authorizationService = authorizationService;          
            _userManager = userManager;
        }

        [Route("~/Projects")]
        // GET: Admin/Projects
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = await _context.Users.FindAsync(userId);            
            var userRoles = await _userManager.GetRolesAsync(user);

            return View(await _mediator.Send(new ProjectListQuery.Query { UserId = userId, Roles = userRoles }));
        }

        [Route("~/Projects/ManageMRF")]
        public async Task<IActionResult> ManageMRF()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = await _context.Users.FindAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);

            return View(await _mediator.Send(new ListAllProjectsQuery.Query { UserId = userId, Roles = userRoles }));
        }

        [HttpDelete]
        public async Task<IActionResult> ManageMRFDelete(int projectid)
        {
            //find the item in the project table
            var item = await _context.Projects.FindAsync(projectid);
            //toggle the IsActive flag
            item.IsActive = !item.IsActive;
            //Update the database
            var deleted = await _context.SaveChangesAsync();
            return new JsonResult(deleted > 0);        
        }       

        [Route("~/requests")]
        // GET: Admin/Projects
        public async Task<IActionResult> Index1()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = await _context.Users.FindAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);

            return View(await _mediator.Send(new ProjectListQuery.Query { UserId = userId, Roles = userRoles }));
        }


        public async Task<IActionResult> Create(CreateProjectQuery query)
        {
            if (query.IsTemplate && !(await _authorizationService.AuthorizeAsync(User, Policies.ManageGlobalThemes)).Succeeded)
            {
                return Forbid();
            }      
            
            var model = await _mediator.Send(query);
           
           
            return View(model.Value);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelStateValidation]
        public async Task<IActionResult> Create(CreateProjectCommand model)
        {

            if (model.ProjectType == ProjectTypeEnum.Template && !(await _authorizationService.AuthorizeAsync(User, Policies.ManageGlobalThemes)).Succeeded)
            {
                return Forbid();
            }

            var result = await _mediator.Send(model);
            if (!result)
            {
                return View(model);
            }
                     
            return RedirectToAction("App", "Home", new { projectId = result.Value.Id, Area = "Admin", path= "themes" });

        }



        public IActionResult ProjectExists(string prefix)
        {
            return new JsonResult(_context.Projects.Any(p => p.Prefix == prefix));
        }
    }
}
