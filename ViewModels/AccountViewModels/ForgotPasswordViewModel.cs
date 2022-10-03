using System.ComponentModel.DataAnnotations;

namespace courseappchallenge.ViewModels.AccountViewModels;

public class ForgotPasswordViewModel
{
    
    [Required(ErrorMessage = "User e-mail is required.")]
    [EmailAddress(ErrorMessage = "This is not a valid e-mail.")]
    [Display(Name = "E-mail")]
    public string Email { get; set; }
}
