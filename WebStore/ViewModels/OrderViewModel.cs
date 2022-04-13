using System.ComponentModel.DataAnnotations;

namespace WebStore.ViewModels;

public class OrderViewModel
{
    [Required]
    [MaxLength(200)]
    public string Address { get; set; } = null!;
    [Required]
    [MaxLength(200)]
    public string Phone { get; set; } = null!;
    [Required]
    public string? Description { get; set; }
}