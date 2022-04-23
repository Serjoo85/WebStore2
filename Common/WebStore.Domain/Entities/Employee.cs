using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    [Column(TypeName = "decimal(18,2)")]
    public decimal Salary { get; set; }

    public override string ToString() => $"{LastName} {FirstName} {Patronymic}";
}