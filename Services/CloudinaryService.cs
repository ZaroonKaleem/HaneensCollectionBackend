using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HaneensCollection.DTOs;
using HaneensCollection.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HaneensCollection.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var cloudinarySettings = configuration.GetSection("CloudinarySettings");
            var cloudName = cloudinarySettings["CloudName"];
            var apiKey = cloudinarySettings["ApiKey"];
            var apiSecret = cloudinarySettings["ApiSecret"];

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<CloudinaryUploadResult> UploadImageAsync(string base64Image, string publicId = null)
        {
            if (string.IsNullOrEmpty(base64Image))
                throw new ArgumentNullException(nameof(base64Image));

            var base64Data = base64Image.Contains(",") ? base64Image.Split(',')[1] : base64Image;

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription("file", new MemoryStream(Convert.FromBase64String(base64Data))),
                PublicId = publicId
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return new CloudinaryUploadResult
            {
                ImageUrl = uploadResult.SecureUrl?.ToString() ?? "",
                MimeType = uploadResult.Format // Cloudinary gives this
            };
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result.Result == "ok";
        }
    }
}
