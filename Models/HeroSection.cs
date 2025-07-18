// Models/HeroSection.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class HeroSection
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [StringLength(500)]
    public string Subtitle { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(max)")] // Explicitly set column type
    public string BannerImageBase64 { get; set; }

    [Required]
    [StringLength(30)]
    public string ImageContentType { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}