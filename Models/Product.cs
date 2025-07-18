using HaneensCollection.Models;
using System.ComponentModel.DataAnnotations;

public abstract class Product
{
    [Key]
    public Guid ProductId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; }

    [Required, StringLength(50)]
    public string Category { get; set; }

    [StringLength(900)]
    public string ShortDescription { get; set; }

    [Required]
    public decimal OriginalPrice { get; set; }

    public decimal? SalePrice { get; set; }
    public int? SalePercentage { get; set; }

    public List<string> SizesAvailable { get; set; }
    public int StockQuantity { get; set; }

    public double? AverageRating { get; set; }
    public int RatingCount { get; set; }

    public List<string> ColorOptions { get; set; }

    public string Material { get; set; }

    public bool IsNew { get; set; }
    public bool IsExclusive { get; set; }

    public string ShippingInfo { get; set; }

    public List<ProductImage> Images { get; set; } = new();
}
