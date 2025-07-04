namespace ClarusAI.Core.Entities;

public class MedicalImage
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid UploadedById { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string ImageType { get; set; } = string.Empty; // CT, X-Ray, MRI, etc.
    public string BodyPart { get; set; } = string.Empty;
    public DateTime StudyDate { get; set; }
    public DateTime CaptureDate { get; set; }
    public string DicomData { get; set; } = string.Empty;
    public string KeyReference { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public Patient Patient { get; set; } = null!;
    public User UploadedBy { get; set; } = null!;
    public ICollection<AnalysisResult> AnalysisResults { get; set; } = new List<AnalysisResult>();
}