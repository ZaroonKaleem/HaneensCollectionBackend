// DTOs/HeroSectionDto.cs
public class HeroSectionDto
{
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public IFormFile BannerImage { get; set; }
}

// DTOs/HeroSectionResponseDto.cs
public class HeroSectionResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string BannerImageBase64 { get; set; }
    public string ImageContentType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}