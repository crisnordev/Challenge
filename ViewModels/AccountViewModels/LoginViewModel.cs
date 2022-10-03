using System.ComponentModel.DataAnnotations;

namespace courseappchallenge.ViewModels.AccountViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "User e-mail is required.")]
    [EmailAddress(ErrorMessage = "This is not a valid e-mail.")]
    [Display(Name = "E-mail")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password, ErrorMessage = "This is not a valid password")]
    public string Password { get; set; }
    
    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}