// Services/ImageHelperService.cs
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

public class ImageHelperService
{
    private const int MaxWidth = 1200;
    private const int MaxHeight = 800;
    private const int DefaultQuality = 75;
    private const long MaxFileSizeBytes = 500 * 1024; // 500KB

    public async Task<(string Base64String, string ContentType)> ConvertImageToBase64(IFormFile imageFile)
    {
        // Validate input
        if (imageFile == null || imageFile.Length == 0)
        {
            throw new ArgumentException("Image file is empty or null");
        }

        if (imageFile.Length > 5 * 1024 * 1024) // 5MB max
        {
            throw new ArgumentException("Image size exceeds maximum allowed (5MB)");
        }

        // Validate content type
        var contentType = imageFile.ContentType.ToLower();
        if (!IsSupportedImageType(contentType))
        {
            throw new ArgumentException($"Unsupported image type: {contentType}. Only JPEG, PNG are supported.");
        }

        try
        {
            // Process and compress the image
            using var outputStream = new MemoryStream();
            using var image = await Image.LoadAsync(imageFile.OpenReadStream());

            // Resize while maintaining aspect ratio
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(MaxWidth, MaxHeight),
                Mode = ResizeMode.Max
            }));

            // Save with appropriate encoder and quality
            IImageEncoder encoder = contentType switch
            {
                "image/jpeg" or "image/jpg" => new JpegEncoder { Quality = DefaultQuality },
                "image/png" => new PngEncoder(),
                _ => new JpegEncoder { Quality = DefaultQuality } // default fallback
            };

            await image.SaveAsync(outputStream, encoder);

            // If still too large, reduce quality further (for JPEGs)
            if (outputStream.Length > MaxFileSizeBytes && (contentType == "image/jpeg" || contentType == "image/jpg"))
            {
                outputStream.SetLength(0);
                await image.SaveAsync(outputStream, new JpegEncoder { Quality = DefaultQuality - 15 });
            }

            var base64String = Convert.ToBase64String(outputStream.ToArray());
            return (base64String, contentType);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to process image", ex);
        }
    }

    public string GetDataUrl(string base64String, string contentType)
    {
        if (string.IsNullOrWhiteSpace(base64String))
        {
            return string.Empty;
        }

        // Validate and normalize content type
        var normalizedContentType = IsSupportedImageType(contentType)
            ? contentType
            : "image/jpeg"; // Default fallback

        return $"data:{normalizedContentType};base64,{base64String}";
    }

    private bool IsSupportedImageType(string contentType)
    {
        return contentType switch
        {
            "image/jpeg" or "image/jpg" or "image/png" => true,
            _ => false
        };
    }
}