
// DTOs/InstagramPostDto.cs
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class InstagramPostDto
{
    public IFormFile? Image { get; set; }  // Made nullable with ?

    [Required]
    [Url]
    public string InstagramLink { get; set; } = string.Empty;
}

// DTOs/InstagramPostResponseDto.cs
public class InstagramPostResponseDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string InstagramLink { get; set; } = string.Empty;
    public string ImageContentType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}