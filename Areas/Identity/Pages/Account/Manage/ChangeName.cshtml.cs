#nullable disable

using System.ComponentModel.DataAnnotations;
using courseappchallenge.Data;
using courseappchallenge.Models;
using courseappchallenge.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace courseappchallenge.Areas.Identity.Pages.Account.Manage;

public class ChangeNameModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public ChangeNameModel(ApplicationDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [BindProperty] public InputModel Input { get; set; } = default!;

    [TempData] public string StatusMessage { get; set; } = string.Empty;

    public class InputModel
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(160, MinimumLength = 2, ErrorMessage = "Name must have between 2 and 160 characters long.")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(160, MinimumLength = 2, ErrorMessage = "Name must have between 2 and 160 characters long.")]
        public string LastName { get; set; } = string.Empty;

    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound(new ErrorResultViewModel("Can not find user."));

        Input = new InputModel { FirstName = user.FirstName, LastName = user.LastName };
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound(new ErrorResultViewModel("Can not find user."));

        user.FirstName = Input.FirstName;
        user.LastName = Input.LastName;
        
        var changeName = await _userManager.UpdateAsync(user);
        if (!changeName.Succeeded)
        {
            var errorResult = new ErrorResultViewModel("Something is wrong.");
            foreach (var error in changeName.Errors)
            {
                errorResult.Errors?.Add(error.ToString());
            }

            return BadRequest(errorResult);
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Your profile has been updated";
        return RedirectToPage();
    }
}