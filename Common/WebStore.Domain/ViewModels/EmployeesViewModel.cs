using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Domain.ViewModels;
public class EmployeesViewModel: IValidatableObject
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name ="Last name")]
    [Required(ErrorMessage = "Enter Last name!")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "Last name length should be between 2 and 255 symbols!")]
    [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Format string error")]
    public string LastName { get; set; }

    [StringLength(255, MinimumLength = 2, ErrorMessage = "Last name length should be between 2 and 255 symbols!")]
    [Display(Name = "First name")]
    [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Format string error")]
    public string FirstName { get; set; }
    [StringLength(255, ErrorMessage = "Last name length should be up to 255 symbols!")]
    [RegularExpression(@"(([А-ЯЁ][а-яё]+)|([A-Z][a-z]+))?", ErrorMessage = "Format string error")]
    public string? Patronymic { get; set; }

    [Range(18,80, ErrorMessage = "The age should be between 18 and 80!")]
    public int Age { get; set; }

    public string? Position { get; set; }
    
    [Range(12000, 2000000, ErrorMessage = "The salary should be between 12000$ and 200000$!")]
    public decimal Salary { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (Age == 29)
            yield return new ValidationResult("Boss don't like this number!", new []{ nameof(Age) });
        yield return ValidationResult.Success!;
    }
}