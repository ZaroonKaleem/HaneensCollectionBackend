using HaneensCollection.DTOs;

public interface ICloudinaryService
{
    Task<CloudinaryUploadResult> UploadImageAsync(string base64Image, string publicId = null);
    Task<bool> DeleteImageAsync(string publicId);
}
