#nullable disable

using System.ComponentModel.DataAnnotations;
using CourseAppChallenge.Data;
using CourseAppChallenge.Models;
using CourseAppChallenge.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseAppChallenge.Areas.Identity.Pages.Account.Manage;

public class IndexModel : RoleNamePageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;

    public IndexModel(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }

    public string UserName { get; set; }

    [TempData] public string StatusMessage { get; set; }

    [BindProperty] public InputModel Input { get; set; }

    public class InputModel
    {
        [Required]
        [Display(Name = "User Name")]
        [StringLength(160, MinimumLength = 2, ErrorMessage = "Name must have between 2 and 160 characters long.")]
        public string UserName { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public Guid? AppRoleId { get; set; }

        public AppRole AppRole { get; set; } 
    }

    private async Task LoadAsync(AppUser appUser)
    {
        var userName = await _userManager.GetUserNameAsync(appUser);
        var phoneNumber = await _userManager.GetPhoneNumberAsync(appUser);
        var userRoles = await _userManager.GetRolesAsync(appUser);
        var roleString = userRoles.FirstOrDefault(x => !string.IsNullOrEmpty(x));
        var role =await _roleManager.FindByNameAsync(roleString);

        Input = new InputModel
        {
            UserName = userName,
            PhoneNumber = phoneNumber,
            AppRoleId = role.Id,
            AppRole = role
        };
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound(new ErrorResultViewModel("Can not find user."));

        await LoadAsync(user);
        PopulateRolesDropDownList(_roleManager);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound(new ErrorResultViewModel("Can not find user."));

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        var userName = await _userManager.GetUserNameAsync(user);
        if (Input.UserName != userName)
        {
            if (_context.Users.AsNoTracking().Any(x => x.UserName == Input.UserName))
            {
                StatusMessage = "This user name has already been used.";
                return RedirectToPage();
            }

            var setUserName = await _userManager.SetUserNameAsync(user, Input.UserName);
            if (!setUserName.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to set user name.";
                return RedirectToPage();
            }
        }

        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (Input.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to set phone number.";
                return RedirectToPage();
            }
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var roleString = userRoles.FirstOrDefault(x => !string.IsNullOrEmpty(x));
        var inputRole = await _roleManager.Roles.FirstAsync(x => x.Id == Input.AppRoleId);
        if (roleString != inputRole.Name)
        {
            var removeToRole = await _userManager.RemoveFromRoleAsync(user, roleString);
            if (!removeToRole.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to remove user role.";
                return RedirectToPage();
            }

            var addToRole = await _userManager.AddToRoleAsync(user, inputRole.Name);
            if (!addToRole.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to add user role.";
                return RedirectToPage();
            }

            user.AppRoleId = Input.AppRoleId;
            await _signInManager.RefreshSignInAsync(user);
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Your profile has been updated";
        return RedirectToPage();
    }
}