using GoRegister.Areas.Admin.ViewModels;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.AdminUsers;
using GoRegister.ApplicationCore.Framework.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using GoRegister.Framework.Authorization;
using Microsoft.AspNetCore.Authorization;
using GoRegister.ApplicationCore.Framework.Notifications;
using Microsoft.AspNetCore.Identity.UI.Services;
using GoRegister.ApplicationCore.Services.Email;


namespace GoRegister.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Users/[action]")]
    [Authorize(Policies.ManageUsers)]
    public class UsersController : AdminControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAdminUserService _adminUserService;
        private readonly INotifier _notifier;
        private readonly IEmailSendingService _emailSendingService;


        public UsersController(UserManager<ApplicationUser> userManager, IAdminUserService adminUserService, INotifier notifier, IEmailSendingService emailSenderService)
        {
            _userManager = userManager;
            _notifier = notifier;
            _adminUserService = adminUserService;
            _emailSendingService = emailSenderService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var vm = new UserListViewModel();
            vm.Users = users;
            return View(vm);
        }

        public IActionResult Invite()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var model = await _adminUserService.GetCreateModelAsync();
            return View("CreateEdit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminUserCreateEditModel model)
        {
            if (ModelState.IsValid)
            {
                Result<int> result;
                result = await _adminUserService.CreateAsync(model);

                if (result)
                {
                    _notifier.Success("User created successfully");

                    return RedirectToAction("Index", "Users");
                }
            }

            var tmpModel = await _adminUserService.GetCreateModelAsync();
            return View("CreateEdit", tmpModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _adminUserService.FindAsync(id);
            return View("CreateEdit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminUserCreateEditModel model)
        {
            this.ModelState.Remove("Password");
            this.ModelState.Remove("ConfirmPassword");

            var oldModelValue = await _adminUserService.FindAsync(model.Id);


            if (ModelState.IsValid)
            {
                Result<int> result;
                result = await _adminUserService.EditAsync(model);



                if (model.Email != oldModelValue.Email)
                {

                    var emailObject = new EmailObject
                    {
                        Subject = "GoRegister - Change of email",
                        Bcc = "",
                        Cc = "",
                        Body = "We have updated your email address as requested. If you believe this was done incorrectly then please email goregister.support@amexgbt.com",
                        FromEmail = "goregister@notifications.amexgbt.com",
                        FromEmailDisplayName = "GoRegister",
                        To = oldModelValue.Email
                    };

                    await _emailSendingService.Send(emailObject);
                }

                if (result)
                {
                    _notifier.Success("User updated successfully");

                    return RedirectToAction("Index", "Users");
                }
            }

            var tmpmodel = await _adminUserService.FindAsync(model.Id);



            return View("CreateEdit", tmpmodel);
        }

        public async Task<IActionResult> ChangePassword(int id)
        {
            var tmpModel = await _adminUserService.FindAsyncChangePassword(id);

            AdminUserPasswordModel model = new AdminUserPasswordModel
            {
                Id = tmpModel.Id,
                FirstName = tmpModel.FirstName,
                LastName = tmpModel.LastName,
                Email = tmpModel.Email
            };

            return View("ChangePassword", model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(AdminUserPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminUserService.ChangePasswordAsync(model.Id, model.Password);

                if (result.Succeeded)
                {
                    _notifier.Success("Password updated successfully");

                    return RedirectToAction("Index", "Users");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View("ChangePassword", model);
        }
    }
}