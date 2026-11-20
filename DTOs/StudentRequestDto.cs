using System.ComponentModel.DataAnnotations;

namespace StudentManagementApi.DTOs;

/// <summary>Data accepted from the client for POST (create) and PUT (update).</summary>
public class StudentRequestDto
{
    [Required(ErrorMessage = "Enrollment number is required.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "Enrollment number must be exactly 11 digits.")]
    public string EnrollmentNo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(60, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 60 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Department is required.")]
    [StringLength(50, ErrorMessage = "Department cannot exceed 50 characters.")]
    public string Department { get; set; } = string.Empty;

    [Range(1, 8, ErrorMessage = "Semester must be between 1 and 8.")]
    public int Semester { get; set; }
}
