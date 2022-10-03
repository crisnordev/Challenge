// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace courseappchallenge.ViewModels.ManageViewModels;

public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "Current password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Current password must must have between 6 and 100 characters long.")]
    [DataType(DataType.Password, ErrorMessage = "This is not a valid password.")]
    [Display(Name = "Current password")]
    public string OldPassword { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must must have between 6 and 100 characters long.")]
    [DataType(DataType.Password, ErrorMessage = "This is not a valid password.")]
    [Display(Name = "New password")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Confirm new password is required.")]
    [DataType(DataType.Password, ErrorMessage = "This is not a valid password.")]
    [Compare("NewPassword", ErrorMessage = "Those passwords does not match.")]
    [Display(Name = "Confirm new password")]
    public string ConfirmPassword { get; set; }
}
