#nullable disable

using System.Text;
using CourseAppChallenge.Models;
using CourseAppChallenge.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace CourseAppChallenge.Areas.Identity.Pages.Account;

public class ConfirmEmailModel : PageModel
{
    private readonly UserManager<AppUser> _userManager;

    public ConfirmEmailModel(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [TempData] public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(string userId, string code)
    {
        if (userId == null || code == null) return RedirectToPage("/Index");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound(new ErrorResultViewModel("Can not find user."));

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, code);
        StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
        return Page();
    }
}