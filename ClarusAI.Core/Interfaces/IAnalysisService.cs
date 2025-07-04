using ClarusAI.Core.Entities;

namespace ClarusAI.Core.Interfaces;

public interface IAnalysisService
{
    Task<AnalysisResult> AnalyzeImageAsync(Guid medicalImageId, string analysisType);
    Task<IEnumerable<AnalysisResult>> GetPatientAnalysisResultsAsync(Guid patientId);
    Task<AnalysisResult?> GetAnalysisResultAsync(Guid analysisId);
    Task<bool> ProcessAnalysisQueueAsync();
    Task<string> GenerateReportAsync(Guid analysisId);
}