// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace courseappchallenge.ViewModels.ManageViewModels;

public class SetPasswordViewModel
{
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must must have between 6 and 100 characters long.")]
    [DataType(DataType.Password, ErrorMessage = "This is not a valid password.")]
    [Display(Name = "Password")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Confirm password is required.")]
    [DataType(DataType.Password, ErrorMessage = "This is not a valid password.")]
    [Compare("Password", ErrorMessage = "Those passwords does not match.")]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; }
}
