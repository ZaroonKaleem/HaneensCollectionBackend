//using HaneensCollection.Services;
//using HaneensCollection.Data;
//using HaneensCollection.DTOs;
//using HaneensCollection.IServices;
//using HaneensCollection.Models;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace HaneensCollection.Services
//{
//    public class FeaturedProductService : IFeaturedProductService
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly ICloudinaryService _cloudinaryService;

//        public FeaturedProductService(
//            ApplicationDbContext context,
//            ICloudinaryService cloudinaryService)
//        {
//            _context = context;
//            _cloudinaryService = cloudinaryService;
//        }

//        public async Task<List<FeaturedProductDto>> GetFeaturedProductsAsync()
//        {
//            return await _context.FeaturedProducts
//                .AsNoTracking()
//                .Where(p => p.SalePercentage > 0 || p.IsNew || p.IsExclusive)
//                .Include(p => p.Images)
//                .Select(p => new FeaturedProductDto
//                {
//                    ProductId = p.ProductId,
//                    Name = p.Name,
//                    Category = p.Category,
//                    ShortDescription = p.ShortDescription,
//                    OriginalPrice = p.OriginalPrice,
//                    SalePrice = p.SalePrice,
//                    SalePercentage = p.SalePercentage,
//                    SizesAvailable = p.SizesAvailable,
//                    StockQuantity = p.StockQuantity,
//                    AverageRating = p.AverageRating,
//                    RatingCount = p.RatingCount,
//                    ColorOptions = p.ColorOptions,
//                    Material = p.Material,
//                    IsNew = p.IsNew,
//                    IsExclusive = p.IsExclusive,
//                    ShippingInfo = p.ShippingInfo,
//                    Images = p.Images
//                        .OrderByDescending(i => i.IsPrimary)
//                        .Select(i => new ProductImageDto
//                        {
//                            ImageId = i.ImageId,
//                            ImageUrl = i.ImageUrl,
//                            AltText = i.AltText,
//                            IsPrimary = i.IsPrimary,
//                            MimeType = i.MimeType
//                        })
//                        .ToList()
//                })
//                .Take(100)
//                .ToListAsync();
//        }

//        public async Task<FeaturedProductDto> GetFeaturedProductAsync(Guid id)
//        {
//            var product = await _context.FeaturedProducts
//                .Include(p => p.Images)
//                .FirstOrDefaultAsync(p => p.ProductId == id);

//            if (product == null)
//                return null;

//            return MapToDto(product);
//        }

//        public async Task<FeaturedProductDto> CreateFeaturedProductAsync(FeaturedProductDto productDto)
//        {
//            var product = new FeaturedProduct
//            {
//                ProductId = Guid.NewGuid(),
//                Name = productDto.Name,
//                Category = productDto.Category,
//                ShortDescription = productDto.ShortDescription,
//                OriginalPrice = productDto.OriginalPrice,
//                SalePrice = productDto.SalePrice,
//                SalePercentage = productDto.SalePercentage,
//                SizesAvailable = productDto.SizesAvailable,
//                StockQuantity = productDto.StockQuantity,
//                AverageRating = productDto.AverageRating,
//                RatingCount = productDto.RatingCount,
//                ColorOptions = productDto.ColorOptions,
//                Material = productDto.Material,
//                IsNew = productDto.IsNew,
//                IsExclusive = productDto.IsExclusive,
//                ShippingInfo = productDto.ShippingInfo,
//                Images = new List<ProductImage>()
//            };

//            if (productDto.Images != null && productDto.Images.Any())
//            {
//                foreach (var imageDto in productDto.Images)
//                {
//                    if (!string.IsNullOrEmpty(imageDto.ImageUrl))
//                    {
//                        product.Images.Add(new ProductImage
//                        {
//                            ImageId = Guid.NewGuid(),
//                            ProductId = product.ProductId,
//                            ImageUrl = imageDto.ImageUrl,
//                            AltText = imageDto.AltText,
//                            IsPrimary = imageDto.IsPrimary,
//                            MimeType = imageDto.MimeType
//                        });
//                    }
//                }
//            }

//            _context.FeaturedProducts.Add(product);
//            await _context.SaveChangesAsync();

//            return MapToDto(product);
//        }

//        public async Task<bool> UpdateFeaturedProductAsync(Guid id, FeaturedProductDto productDto)
//        {
//            var product = await _context.FeaturedProducts
//                .Include(p => p.Images)
//                .FirstOrDefaultAsync(p => p.ProductId == id);

//            if (product == null)
//                return false;

//            product.Name = productDto.Name;
//            product.Category = productDto.Category;
//            product.ShortDescription = productDto.ShortDescription;
//            product.OriginalPrice = productDto.OriginalPrice;
//            product.SalePrice = productDto.SalePrice;
//            product.SalePercentage = productDto.SalePercentage;
//            product.SizesAvailable = productDto.SizesAvailable;
//            product.StockQuantity = productDto.StockQuantity;
//            product.AverageRating = productDto.AverageRating;
//            product.RatingCount = productDto.RatingCount;
//            product.ColorOptions = productDto.ColorOptions;
//            product.Material = productDto.Material;
//            product.IsNew = productDto.IsNew;
//            product.IsExclusive = productDto.IsExclusive;
//            product.ShippingInfo = productDto.ShippingInfo;

//            _context.ProductImages.RemoveRange(product.Images);
//            product.Images.Clear();

//            if (productDto.Images != null && productDto.Images.Any())
//            {
//                foreach (var imageDto in productDto.Images)
//                {
//                    if (!string.IsNullOrEmpty(imageDto.ImageUrl))
//                    {
//                        product.Images.Add(new ProductImage
//                        {
//                            ImageId = Guid.NewGuid(),
//                            ProductId = product.ProductId,
//                            ImageUrl = imageDto.ImageUrl,
//                            AltText = imageDto.AltText,
//                            IsPrimary = imageDto.IsPrimary,
//                            MimeType = imageDto.MimeType
//                        });
//                    }
//                }
//            }

//            await _context.SaveChangesAsync();
//            return true;
//        }

//        public async Task<bool> DeleteFeaturedProductAsync(Guid id)
//        {
//            var product = await _context.FeaturedProducts
//                .Include(p => p.Images)
//                .FirstOrDefaultAsync(p => p.ProductId == id);

//            if (product == null)
//                return false;

//            foreach (var image in product.Images)
//            {
//                if (!string.IsNullOrEmpty(image.ImageUrl))
//                {
//                    try
//                    {
//                        var publicId = GetPublicIdFromUrl(image.ImageUrl);
//                        await _cloudinaryService.DeleteImageAsync(publicId);
//                    }
//                    catch
//                    {
//                        // Log error but continue
//                    }
//                }
//            }

//            _context.FeaturedProducts.Remove(product);
//            await _context.SaveChangesAsync();
//            return true;
//        }

//        private FeaturedProductDto MapToDto(FeaturedProduct product)
//        {
//            return new FeaturedProductDto
//            {
//                ProductId = product.ProductId,
//                Name = product.Name,
//                Category = product.Category,
//                ShortDescription = product.ShortDescription,
//                OriginalPrice = product.OriginalPrice,
//                SalePrice = product.SalePrice,
//                SalePercentage = product.SalePercentage,
//                SizesAvailable = product.SizesAvailable,
//                StockQuantity = product.StockQuantity,
//                AverageRating = product.AverageRating,
//                RatingCount = product.RatingCount,
//                ColorOptions = product.ColorOptions,
//                Material = product.Material,
//                IsNew = product.IsNew,
//                IsExclusive = product.IsExclusive,
//                ShippingInfo = product.ShippingInfo,
//                Images = product.Images.Select(i => new ProductImageDto
//                {
//                    ImageId = i.ImageId,
//                    ImageUrl = i.ImageUrl,
//                    AltText = i.AltText,
//                    IsPrimary = i.IsPrimary,
//                    MimeType = i.MimeType
//                }).ToList()
//            };
//        }

//        private string GetPublicIdFromUrl(string imageUrl)
//        {
//            var uri = new Uri(imageUrl);
//            var segments = uri.Segments;
//            var uploadIndex = Array.IndexOf(segments, "upload/") + 1;
//            return string.Join("", segments.Skip(uploadIndex)).Split('.')[0];
//        }
//    }
//}