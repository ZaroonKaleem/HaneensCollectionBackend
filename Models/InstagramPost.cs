// Models/InstagramPost.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class InstagramPost
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string ImageUrl { get; set; } = string.Empty;

    [Required]
    public string InstagramLink { get; set; } = string.Empty;

    [Required]
    public string ImageContentType { get; set; } = "image/jpeg";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}