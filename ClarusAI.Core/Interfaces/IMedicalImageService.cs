using ClarusAI.Core.Entities;

namespace ClarusAI.Core.Interfaces;

public interface IMedicalImageService
{
    Task<MedicalImage> UploadImageAsync(Guid patientId, Stream imageStream, string fileName, string contentType, string imageType, string bodyPart);
    Task<Stream> GetImageAsync(Guid imageId);
    Task<bool> DeleteImageAsync(Guid imageId);
    Task<IEnumerable<MedicalImage>> GetPatientImagesAsync(Guid patientId);
    Task<MedicalImage?> GetImageByIdAsync(Guid imageId);
    Task<bool> ValidateImageAsync(Stream imageStream, string contentType);
}