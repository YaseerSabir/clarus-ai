using System.ComponentModel.DataAnnotations;

namespace ClarusAI.Core.DTOs;

public class MedicalImageDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string ImageType { get; set; } = string.Empty;
    public string BodyPart { get; set; } = string.Empty;
    public DateTime CaptureDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string PatientName { get; set; } = string.Empty;
}

public class UploadMedicalImageDto
{
    [Required]
    public Guid PatientId { get; set; }
    
    [Required]
    [StringLength(255)]
    public string FileName { get; set; } = string.Empty;
    
    [Required]
    [RegularExpression(@"^(image/|application/dicom)", ErrorMessage = "Only image files and DICOM files are allowed")]
    public string ContentType { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string ImageType { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string BodyPart { get; set; } = string.Empty;
    
    [Required]
    public DateTime CaptureDate { get; set; }
    
    [Required]
    public Stream ImageStream { get; set; } = Stream.Null;
}