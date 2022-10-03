using courseappchallenge.Models;
using courseappchallenge.ViewModels.ManageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace courseappchallenge.Controllers;

[Authorize]
public class ManageController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public ManageController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index(ManageMessageId? message = null)
    {
        ViewData["StatusMessage"] =
            message switch
            {
                ManageMessageId.ChangePasswordSuccess => "Your password has been changed.",
                ManageMessageId.SetPasswordSuccess => "Your password has been set.",
                ManageMessageId.Error => "An error has occurred.",
                _ => ""
            };

        var user = await GetCurrentUserAsync();
        var model = new IndexViewModel
        {
            HasPassword = await _userManager.HasPasswordAsync(user),
            Logins = await _userManager.GetLoginsAsync(user),
        };
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel account)
    {
        ManageMessageId? message = ManageMessageId.Error;
        
        var user = await GetCurrentUserAsync();
        if (user == null) return RedirectToAction(nameof(ManageLogins), new { Message = message });
        
        var result = await _userManager.RemoveLoginAsync(user, account.LoginProvider, account.ProviderKey);
        if (!result.Succeeded) return RedirectToAction(nameof(ManageLogins), new { Message = message });
        
        await _signInManager.SignInAsync(user, isPersistent: false);
        message = ManageMessageId.RemoveLoginSuccess;
        
        return RedirectToAction(nameof(ManageLogins), new { Message = message });
    }

    [HttpGet]
    public IActionResult ChangePassword() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        
        var user = await GetCurrentUserAsync();
        if (user == null) return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        
        var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
        }
        
        AddErrors(result);
        
        return View(model);
    }

    public IActionResult SetPassword() => View();
    

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        
        var user = await GetCurrentUserAsync();
        if (user == null) return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        
        var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.SetPasswordSuccess });
        }
        
        AddErrors(result);
        
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ManageLogins(ManageMessageId? message = null)
    {
        ViewData["StatusMessage"] = message == ManageMessageId.Error ? "An error has occurred." : "";
        
        var user = await GetCurrentUserAsync();
        if (user == null) return View("Error");
        
        var userLogins = await _userManager.GetLoginsAsync(user);
        
        ViewData["ShowRemoveButton"] = user.PasswordHash != null || userLogins.Count > 1;
        
        return View(new ManageLoginsViewModel { CurrentLogins = userLogins });
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    public enum ManageMessageId
    {
        ChangePasswordSuccess,
        SetPasswordSuccess,
        RemoveLoginSuccess,
        Error
    }

    private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
}
