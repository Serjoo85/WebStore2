using System.ComponentModel.DataAnnotations;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities;

public class Employee : Entity
{
    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public string FirstName { get; set; } = null!;

    public string? Patronymic { get; set; }

    [Required]
    public int Age { get; set;}

    [Required]
    public string Position { get; set; } = null!;

    [Required]
    public decimal Salary { get; set; }
}