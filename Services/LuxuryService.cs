using HaneensCollection.Data;
using HaneensCollection.DTOs;
using HaneensCollection.IServices;
using HaneensCollection.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaneensCollection.Services
{
    public class LuxuryService : ILuxuryService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;

        public LuxuryService(ApplicationDbContext context, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<List<LuxuryDto>> GetAllAsync()
        {
            var luxuries = await _context.Luxuries.Include(p => p.Images).ToListAsync();
            return luxuries.Select(MapToDto).ToList();
        }

        public async Task<LuxuryDto> GetByIdAsync(Guid id)
        {
            var luxury = await _context.Luxuries.Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ProductId == id);
            return luxury == null ? null : MapToDto(luxury);
        }

        //public async Task<LuxuryDto> CreateAsync(LuxuryDto dto)
        //{
        //    var luxury = MapToEntity(dto);
        //    luxury.ProductId = Guid.NewGuid();

        //    if (dto.Images != null && dto.Images.Any())
        //    {
        //        luxury.Images = new List<ProductImage>();

        //        foreach (var imageDto in dto.Images)
        //        {
        //            var upload = await _cloudinaryService.UploadImageAsync(imageDto.ImageUrl);
        //            luxury.Images.Add(new ProductImage
        //            {
        //                ImageId = Guid.NewGuid(),
        //                ProductId = luxury.ProductId,
        //                ImageUrl = upload.ImageUrl,
        //                AltText = imageDto.AltText,
        //                IsPrimary = imageDto.IsPrimary,
        //                MimeType = upload.MimeType
        //            });
        //        }
        //    }

        //    _context.Luxuries.Add(luxury);
        //    await _context.SaveChangesAsync();
        //    return MapToDto(luxury);
        //}
        public async Task<LuxuryDto> CreateAsync(LuxuryDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var luxury = MapToEntity(dto);
            luxury.ProductId = Guid.NewGuid();

            if (dto.Images != null && dto.Images.Any())
            {
                luxury.Images = new List<ProductImage>();

                foreach (var imageDto in dto.Images)
                {
                    if (!string.IsNullOrWhiteSpace(imageDto.ImageUrl) && imageDto.ImageUrl.StartsWith("data:image"))
                    {
                        // Image is in base64 format, upload to Cloudinary
                        var upload = await _cloudinaryService.UploadImageAsync(imageDto.ImageUrl);
                        luxury.Images.Add(new ProductImage
                        {
                            ImageId = Guid.NewGuid(),
                            ProductId = luxury.ProductId,
                            ImageUrl = upload.ImageUrl,
                            AltText = imageDto.AltText,
                            IsPrimary = imageDto.IsPrimary,
                            MimeType = upload.MimeType
                        });
                    }
                    else if (!string.IsNullOrWhiteSpace(imageDto.ImageUrl))
                    {
                        // Image URL is already valid (uploaded earlier), just save it
                        luxury.Images.Add(new ProductImage
                        {
                            ImageId = Guid.NewGuid(),
                            ProductId = luxury.ProductId,
                            ImageUrl = imageDto.ImageUrl,
                            AltText = imageDto.AltText,
                            IsPrimary = imageDto.IsPrimary,
                            MimeType = imageDto.MimeType
                        });
                    }
                }
            }

            _context.Luxuries.Add(luxury);
            await _context.SaveChangesAsync();
            return MapToDto(luxury);
        }
        public async Task<bool> UpdateAsync(Guid id, LuxuryDto dto)
        {
            var luxury = await _context.Luxuries.Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (luxury == null) return false;

            _context.Entry(luxury).CurrentValues.SetValues(MapToEntity(dto, luxury));

            _context.ProductImages.RemoveRange(luxury.Images);
            luxury.Images.Clear();

            if (dto.Images != null)
            {
                foreach (var imageDto in dto.Images)
                {
                    var upload = await _cloudinaryService.UploadImageAsync(imageDto.ImageUrl);
                    luxury.Images.Add(new ProductImage
                    {
                        ImageId = Guid.NewGuid(),
                        ProductId = luxury.ProductId,
                        ImageUrl = upload.ImageUrl,
                        AltText = imageDto.AltText,
                        IsPrimary = imageDto.IsPrimary,
                        MimeType = upload.MimeType
                    });
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var luxury = await _context.Luxuries.Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (luxury == null) return false;

            _context.Luxuries.Remove(luxury);
            await _context.SaveChangesAsync();
            return true;
        }

        private LuxuryDto MapToDto(Luxury luxury)
        {
            return new LuxuryDto
            {
                ProductId = luxury.ProductId,
                Name = luxury.Name,
                Category = luxury.Category,
                ShortDescription = luxury.ShortDescription,
                OriginalPrice = luxury.OriginalPrice,
                SalePrice = luxury.SalePrice,
                SalePercentage = luxury.SalePercentage,
                SizesAvailable = luxury.SizesAvailable,
                StockQuantity = luxury.StockQuantity,
                AverageRating = luxury.AverageRating,
                RatingCount = luxury.RatingCount,
                ColorOptions = luxury.ColorOptions,
                Material = luxury.Material,
                IsNew = luxury.IsNew,
                IsExclusive = luxury.IsExclusive,
                ShippingInfo = luxury.ShippingInfo,
                Note = luxury.Note,
                Images = luxury.Images?.Select(i => new HaneensCollection.DTOs.ProductImageDto
                {
                    ImageId = i.ImageId,
                    ImageUrl = i.ImageUrl,
                    AltText = i.AltText,
                    IsPrimary = i.IsPrimary,
                    MimeType = i.MimeType
                }).ToList()
            };
        }

        private Luxury MapToEntity(LuxuryDto dto, Luxury existing = null)
        {
            var luxury = existing ?? new Luxury();
            luxury.Name = dto.Name;
            luxury.Category = dto.Category;
            luxury.ShortDescription = dto.ShortDescription;
            luxury.OriginalPrice = dto.OriginalPrice;
            luxury.SalePrice = dto.SalePrice;
            luxury.SalePercentage = dto.SalePercentage;
            luxury.SizesAvailable = dto.SizesAvailable;
            luxury.StockQuantity = dto.StockQuantity;
            luxury.AverageRating = dto.AverageRating;
            luxury.RatingCount = dto.RatingCount;
            luxury.ColorOptions = dto.ColorOptions;
            luxury.Material = dto.Material;
            luxury.IsNew = dto.IsNew;
            luxury.IsExclusive = dto.IsExclusive;
            luxury.ShippingInfo = dto.ShippingInfo;
            luxury.Note = dto.Note;
            return luxury;
        }
    }
}
