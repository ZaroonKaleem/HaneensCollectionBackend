// Controllers/HeroSectionController.cs
using HaneensCollection.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class HeroSectionController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ImageCompressionService _imageCompression;
    private readonly ILogger<HeroSectionController> _logger;

    public HeroSectionController(
        ApplicationDbContext context,
        ImageCompressionService imageCompression,
        ILogger<HeroSectionController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _imageCompression = imageCompression ?? throw new ArgumentNullException(nameof(imageCompression));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: api/herosection/latest
    [HttpGet("latest")]
    public async Task<ActionResult<HeroSectionResponseDto>> GetLatestHeroSection()
    {
        try
        {
            var heroSection = await _context.HeroSections
                .OrderByDescending(h => h.CreatedAt)
                .FirstOrDefaultAsync();

            if (heroSection == null)
                return NotFound("No hero section found");

            return Ok(MapToResponseDto(heroSection));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting latest hero section");
            return StatusCode(500, "Internal server error");
        }
    }

    // POST: api/herosection
    [HttpPost]
    [RequestSizeLimit(50_000_000)] // 50MB limit
    public async Task<ActionResult<HeroSectionResponseDto>> CreateHeroSection([FromForm] HeroSectionDto heroSectionDto)
    {
        try
        {
            if (heroSectionDto.BannerImage == null)
                return BadRequest("Banner image is required");

            _logger.LogInformation("Starting image processing...");
            var (base64String, contentType) = await _imageCompression.CompressAndConvertToBase64(heroSectionDto.BannerImage);
            _logger.LogInformation("Image processed successfully");

            var heroSection = new HeroSection
            {
                Title = heroSectionDto.Title,
                Subtitle = heroSectionDto.Subtitle,
                BannerImageBase64 = base64String,
                ImageContentType = contentType
            };

            _context.HeroSections.Add(heroSection);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLatestHeroSection), MapToResponseDto(heroSection));
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Validation error: {ex.Message}");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in CreateHeroSection: {ex}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // PUT: api/herosection/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<HeroSectionResponseDto>> UpdateHeroSection(int id, [FromForm] HeroSectionDto heroSectionDto)
    {
        try
        {
            var existingHero = await _context.HeroSections.FindAsync(id);
            if (existingHero == null)
                return NotFound();

            // Update text fields
            existingHero.Title = heroSectionDto.Title;
            existingHero.Subtitle = heroSectionDto.Subtitle;
            existingHero.UpdatedAt = DateTime.UtcNow;

            // Update image if provided
            if (heroSectionDto.BannerImage != null && heroSectionDto.BannerImage.Length > 0)
            {
                var (base64String, contentType) = await _imageCompression.CompressAndConvertToBase64(heroSectionDto.BannerImage);
                existingHero.BannerImageBase64 = base64String;
                existingHero.ImageContentType = contentType;
            }

            await _context.SaveChangesAsync();

            return Ok(MapToResponseDto(existingHero));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating hero section");
            return StatusCode(500, "Internal server error");
        }
    }

    private HeroSectionResponseDto MapToResponseDto(HeroSection heroSection)
    {
        if (heroSection == null)
        {
            throw new ArgumentNullException(nameof(heroSection), "Hero section cannot be null");
        }

        return new HeroSectionResponseDto
        {
            Id = heroSection.Id,
            Title = heroSection.Title ?? string.Empty,
            Subtitle = heroSection.Subtitle ?? string.Empty,
            BannerImageBase64 = !string.IsNullOrEmpty(heroSection.BannerImageBase64)
                ? $"data:{heroSection.ImageContentType ?? "image/jpeg"};base64,{heroSection.BannerImageBase64}"
                : string.Empty,
            ImageContentType = heroSection.ImageContentType ?? "image/jpeg",
            CreatedAt = heroSection.CreatedAt,
            UpdatedAt = heroSection.UpdatedAt
        };
    }
}