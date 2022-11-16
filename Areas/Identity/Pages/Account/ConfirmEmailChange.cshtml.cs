#nullable disable

using System.Text;
using courseappchallenge.Models;
using courseappchallenge.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace courseappchallenge.Areas.Identity.Pages.Account;

public class ConfirmEmailChangeModel : PageModel
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public ConfirmEmailChangeModel(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [TempData] public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
    {
        if (userId == null || email == null || code == null)
            return RedirectToPage("/Index");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound(new ErrorResultViewModel("Can not find user."));

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        
        var userName = await _userManager.GetUserNameAsync(user);
        var userEmail = await _userManager.GetEmailAsync(user);
        if (userName == userEmail)
        {
            var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
            if (!setUserNameResult.Succeeded)
            {
                StatusMessage = "Error changing email, and user name.";
                return Page();
            }
        }
        
        var result = await _userManager.ChangeEmailAsync(user, email, code);
        if (!result.Succeeded)
        {
            StatusMessage = "Error changing email.";
            return Page();
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Thank you for confirming your email change.";
        return Page();
    }
}