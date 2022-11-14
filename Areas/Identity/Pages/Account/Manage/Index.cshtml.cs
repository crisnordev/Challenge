#nullable disable

using System.ComponentModel.DataAnnotations;
using courseappchallenge.Data;
using courseappchallenge.Models;
using courseappchallenge.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace courseappchallenge.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public IndexModel(ApplicationDbContext context, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string UserName { get; set; }

        [TempData] 
        public string StatusMessage { get; set; }

        [BindProperty] 
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "User Name")]
            [StringLength(160, MinimumLength = 2, ErrorMessage = "Name must have between 2 and 160 characters long.")]
            public string UserName { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(AppUser appUser)
        {
            var userName = await _userManager.GetUserNameAsync(appUser);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(appUser);

            Input = new InputModel
            {
                UserName = userName,
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound(new ErrorResultViewModel("Can not find user."));

            await LoadAsync(user);
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

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}