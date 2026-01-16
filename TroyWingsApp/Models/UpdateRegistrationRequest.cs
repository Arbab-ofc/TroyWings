using System.ComponentModel.DataAnnotations;

namespace TroyWingsApp.Models;

public class UpdateRegistrationRequest
{
    public int Id { get; set; }

    [Required, StringLength(80, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Father's Name")]
    [Required, StringLength(80, MinimumLength = 3)]
    public string FatherName { get; set; } = string.Empty;

    [Required]
    public string DateOfBirth { get; set; } = string.Empty;

    [Display(Name = "Contact Number")]
    [Required, StringLength(14, MinimumLength = 10)]
    [RegularExpression("^(\\+?91[- ]?)?[6-9]\\d{9}$", ErrorMessage = "Enter a valid India mobile number (e.g., +91 9876543210).")]
    public string ContactNumber { get; set; } = string.Empty;

    [Required, StringLength(180, MinimumLength = 10)]
    public string Address { get; set; } = string.Empty;
}
