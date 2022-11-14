#nullable disable

using System.ComponentModel.DataAnnotations;
using courseappchallenge.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace courseappchallenge.Areas.Identity.Pages.Account;

public class LoginModel : PageModel
{
    private readonly SignInManager<AppUser> _signInManager;

    public LoginModel(SignInManager<AppUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [BindProperty] 
    public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }

    [TempData] 
    public string ErrorMessage { get; set; }

    public class InputModel
    {
        [Required]
        [Display(Name = "User Name")]
        [StringLength(160, MinimumLength = 2, ErrorMessage = "Name must have between 2 and 160 characters long.")]         
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")] 
        public bool RememberMe { get; set; }
    }

    public async Task OnGetAsync(string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage)) ModelState.AddModelError(string.Empty, ErrorMessage);

        ReturnUrl ??= Url.Content("~/");

        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        
        if (!ModelState.IsValid) return Page();
        
        var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe,
            lockoutOnFailure: false);
        if (result.Succeeded) return LocalRedirect(returnUrl);

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return Page();

    }
}