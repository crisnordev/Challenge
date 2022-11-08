#nullable disable

using CourseAppChallenge.Models;
using CourseAppChallenge.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseAppChallenge.Areas.Identity.Pages.Account.Manage
{
    public class Disable2FactorModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public Disable2FactorModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [TempData] public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound(new ErrorResultViewModel("Can not find user."));

            if (!await _userManager.GetTwoFactorEnabledAsync(user))
                return BadRequest(new ErrorResultViewModel(
                    "Cannot disable 2FA for user as it's not currently enabled."));

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound(new ErrorResultViewModel("Can not find user."));

            var disable2FaResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2FaResult.Succeeded)
                return BadRequest(new ErrorResultViewModel("Unexpected error occurred disabling 2FA."));

            StatusMessage = "2fa has been disabled. You can reenable 2fa when you setup an authenticator app";
            return RedirectToPage("./TwoFactorAuthentication");
        }
    }
}