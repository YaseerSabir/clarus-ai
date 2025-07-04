using System.ComponentModel.DataAnnotations;

namespace ClarusAI.Core.DTOs;

public class PatientDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string MedicalRecordNumber { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreatePatientDto
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "First name can only contain letters and spaces")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50, MinimumLength = 2)]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Last name can only contain letters and spaces")]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    public DateTime DateOfBirth { get; set; }
    
    [Required]
    [StringLength(10)]
    public string Gender { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string MedicalRecordNumber { get; set; } = string.Empty;
    
    [Phone]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;
}

public class UpdatePatientDto
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "First name can only contain letters and spaces")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50, MinimumLength = 2)]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Last name can only contain letters and spaces")]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    public DateTime DateOfBirth { get; set; }
    
    [Required]
    [StringLength(10)]
    public string Gender { get; set; } = string.Empty;
    
    [Phone]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;
}