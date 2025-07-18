using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace HaneensCollection.Models
{
    public class FeaturedProduct
    {
        [Key]
        public Guid ProductId { get; set; } // Unique identifier (SKU)
        [Required]
        public string Name { get; set; } // Product Name
        [Required]
        public string Category { get; set; } // Product Category (e.g., Dresses, Tops)
        public string ShortDescription { get; set; } // Brief description (1-2 sentences)
        [Required]
        public decimal OriginalPrice { get; set; } // Original price
        public decimal? SalePrice { get; set; } // Price after sale (nullable if no sale)
        public int? SalePercentage { get; set; } // Sale percentage (e.g., 30 for 30% off)
        public List<string> SizesAvailable { get; set; } // List of sizes (e.g., ["S", "M", "L"])
        public int StockQuantity { get; set; } // Stock level for urgency cues
        public double? AverageRating { get; set; } // Average customer rating (e.g., 4.5)
        public int RatingCount { get; set; } // Number of reviews
        public List<string> ColorOptions { get; set; } // Available colors (e.g., ["Red", "Blue"])
        public string Material { get; set; } // Material info (e.g., "100% Cotton")
        public bool IsNew { get; set; } // Flag for "New!" badge
        public bool IsExclusive { get; set; } // Flag for "Exclusive" badge
        public string ShippingInfo { get; set; } // Shipping details (e.g., "Ships in 2-3 days")
        public List<ProductImage> Images { get; set; } // List of product images
    }

    public class ProductImage
    {
        [Key]
        public Guid ImageId { get; set; }
        public Guid ProductId { get; set; } // Foreign key to FeaturedProduct
        [Required]
        public string ImageUrl { get; set; } // URL for the image (Cloudinary)
        public string AltText { get; set; } // Alt text for accessibility/SEO
        public bool IsPrimary { get; set; } // Flag for primary image
        public string MimeType { get; set; } // MIME type (e.g., "image/jpeg", "image/png")
    }
}