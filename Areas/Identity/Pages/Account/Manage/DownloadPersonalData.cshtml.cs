#nullable disable

using System.Text.Json;
using CourseAppChallenge.Models;
using CourseAppChallenge.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseAppChallenge.Areas.Identity.Pages.Account.Manage
{
    public class DownloadPersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DownloadPersonalDataModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound(new ErrorResultViewModel("Can not find user."));

            var personalDataProps = typeof(IdentityUser).GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
            var personalData = personalDataProps.ToDictionary(p => p.Name,
                p => p.GetValue(user)?.ToString() ?? "null");

            var logins = await _userManager.GetLoginsAsync(user);
            foreach (var login in logins)
            {
                personalData.Add($"{login.LoginProvider} external login provider key", login.ProviderKey);
            }

            personalData.Add($"Authenticator Key", await _userManager.GetAuthenticatorKeyAsync(user));

            Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
            return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(personalData), "application/json");
        }
    }
}