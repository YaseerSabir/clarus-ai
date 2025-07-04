namespace ClarusAI.Core.Entities;

public class AnalysisResult
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid MedicalImageId { get; set; }
    public Guid RequestedById { get; set; }
    public string AnalysisType { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    public string Findings { get; set; } = string.Empty;
    public decimal Confidence { get; set; }
    public decimal ConfidenceScore { get; set; }
    public string Recommendations { get; set; } = string.Empty;
    public string AiModelVersion { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; }
    public string ProcessingTime { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // Processing, Completed, Failed
    public string ErrorMessage { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    public Patient Patient { get; set; } = null!;
    public MedicalImage MedicalImage { get; set; } = null!;
    public User RequestedBy { get; set; } = null!;
}