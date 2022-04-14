using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.ViewModels.Identity;

public class RegisterUserViewModel
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Display(Name = "Confirm password")]
    [DataType(DataType.Password)]
    public string PasswordConfirm { get; set; }
}