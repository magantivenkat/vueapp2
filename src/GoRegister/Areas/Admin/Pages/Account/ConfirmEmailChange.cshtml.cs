using System.Threading.Tasks;
using GoRegister.ApplicationCore.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using GoRegister.ApplicationCore.Services.Email;

namespace GoRegister.Areas.Admin.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailChangeModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSendingService _emailSendingService;

        public ConfirmEmailChangeModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSendingService emailSenderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSendingService = emailSenderService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            string oldEmailId = user.Email;
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            //code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)); 
            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                StatusMessage = "Error changing email.";
                return Page();
            }

            // In our UI email and user name are one and the same, so when we update the email
            // we need to update the user name.
            var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
            if (!setUserNameResult.Succeeded)
            {
                StatusMessage = "Error changing user name.";
                return Page();
            }

            var emailObject = new EmailObject
            {
                Subject = "GoRegister - Change of email",
                Bcc = "",
                Cc = "",
                Body = "We have updated your email address as requested. If you believe this was done incorrectly then please email goregister.support@amexgbt.com",
                FromEmail = "goregister@notifications.amexgbt.com",
                FromEmailDisplayName = "GoRegister",
                To = oldEmailId
            };

            await _emailSendingService.Send(emailObject);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Thank you for confirming your email change.";
            return Page();
        }
    }
}
