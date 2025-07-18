// Services/ImageCompressionService.cs
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

public class ImageCompressionService
{
    private const int MaxWidth = 1200;
    private const int MaxHeight = 800;
    private const int Quality = 80; // 80% quality for JPEG
    private const long MaxFileSize = 500 * 1024; // 500KB target

    // Services/ImageCompressionService.cs
    public async Task<(string Base64String, string ContentType)> CompressAndConvertToBase64(IFormFile imageFile)
    {
        try
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Image file is empty");

            if (imageFile.Length > 10 * 1024 * 1024) // 10MB max
                throw new ArgumentException("Image size exceeds 10MB limit");

            var contentType = imageFile.ContentType.ToLower();
            if (contentType != "image/jpeg" && contentType != "image/png" && contentType != "image/jpg")
                throw new ArgumentException("Only JPEG and PNG images are supported");

            using var outputStream = new MemoryStream();

            try
            {
                using var image = await Image.LoadAsync(imageFile.OpenReadStream());

                var resizeOptions = new ResizeOptions
                {
                    Size = new Size(MaxWidth, MaxHeight),
                    Mode = ResizeMode.Max
                };

                image.Mutate(x => x.Resize(resizeOptions));

                IImageEncoder encoder = contentType == "image/jpeg" || contentType == "image/jpg"
                    ? new JpegEncoder { Quality = Quality }
                    : new PngEncoder();

                await image.SaveAsync(outputStream, encoder);

                if (outputStream.Length > MaxFileSize && (contentType == "image/jpeg" || contentType == "image/jpg"))
                {
                    outputStream.SetLength(0);
                    await image.SaveAsync(outputStream, new JpegEncoder { Quality = Quality - 20 });
                }

                return (Convert.ToBase64String(outputStream.ToArray()), contentType);
            }
            catch (UnknownImageFormatException)
            {
                throw new ArgumentException("Invalid image format");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Image processing failed: {ex.Message}", ex);
        }
    }
}