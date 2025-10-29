using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GoRegister.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class SetupConfirmationModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}
