using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HaneensCollection.DTOs
{
    public class ProductDto
    {
        public Guid ProductId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; } // Pret, Unstitched, Stitched, etc.

        [StringLength(900)]
        public string ShortDescription { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal OriginalPrice { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? SalePrice { get; set; }

        [Range(0, 100)]
        public int? SalePercentage { get; set; }

        public List<string> SizesAvailable { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [Range(0, 5)]
        public double? AverageRating { get; set; }

        [Range(0, int.MaxValue)]
        public int RatingCount { get; set; }

        public List<string> ColorOptions { get; set; }

        [StringLength(100)]
        public string Material { get; set; }

        public bool IsNew { get; set; }
        public bool IsExclusive { get; set; }

        [StringLength(200)]
        public string ShippingInfo { get; set; }

        public List<ProductImageDto> Images { get; set; } = new();
    }
}

public class ProductImageDto
{
    public Guid ImageId { get; set; }

    [Required]
    public string ImageUrl { get; set; }

    public string AltText { get; set; }

    public bool IsPrimary { get; set; }

    public string MimeType { get; set; }
}
