using courseappchallenge.Models;
using courseappchallenge.ViewModels.AccountViewModels;
using courseappchallenge.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace courseappchallenge.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailService _emailService;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid) return View(model);
        
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, 
            model.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded) return RedirectToLocal(returnUrl);

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        
        return View(model);

    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid) return View(model);
        
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new 
                { UserId = user.Id, Code = code }, protocol: HttpContext.Request.Scheme);
                
            _emailService.Send(model.Email, "Confirm your account",
                "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                
            await _signInManager.SignInAsync(user, isPersistent: false);
                
            return RedirectToLocal(returnUrl);
        }
            
        AddErrors(result);
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOff()
    {
        await _signInManager.SignOutAsync();
        
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
        if (userId == null || code == null) return View("Error");
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return View("Error");
        
        var result = await _userManager.ConfirmEmailAsync(user, code);
        
        return View(result.Succeeded ? "ConfirmEmail" : "Error");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword() => View();

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user)) return View("ForgotPasswordConfirmation");
            
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            var callbackUrl = Url.Action("ResetPassword", "Account", new 
                { UserId = user.Id, Code = code }, protocol: HttpContext.Request.Scheme);
            
            _emailService.Send(model.Email, "Reset Password",
               "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
            
            return View("ForgotPasswordConfirmation");
        }

        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPasswordConfirmation() => View();
    

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPassword(string code = null) => code == null ? View("Error") : View();

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
        
        var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
        if (result.Succeeded) return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
        
        AddErrors(result);
        
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPasswordConfirmation() => View();
    

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
}