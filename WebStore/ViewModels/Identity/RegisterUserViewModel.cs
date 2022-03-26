﻿using System.ComponentModel.DataAnnotations;

namespace WebStore.ViewModels.Identity;

public class RegisterUserViewModel
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    public string PasswordConfirm { get; set; }
}