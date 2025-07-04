using System.ComponentModel.DataAnnotations;

namespace ClarusAI.Core.DTOs;

public class AnalysisResultDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid MedicalImageId { get; set; }
    public string AnalysisType { get; set; } = string.Empty;
    public string Findings { get; set; } = string.Empty;
    public decimal ConfidenceScore { get; set; }
    public string Recommendations { get; set; } = string.Empty;
    public string AiModelVersion { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; }
    public string ProcessingTime { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string ImageFileName { get; set; } = string.Empty;
}

public class RequestAnalysisDto
{
    [Required]
    public Guid MedicalImageId { get; set; }
    
    [Required]
    [StringLength(50)]
    public string AnalysisType { get; set; } = string.Empty;
}