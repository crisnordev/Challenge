#nullable disable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseAppChallenge.Areas.Identity.Pages.Account
{
    [AllowAnonymous] public class ResetPasswordConfirmationModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}