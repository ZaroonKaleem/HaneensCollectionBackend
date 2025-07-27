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
    public class PretService : IPretService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;

        public PretService(ApplicationDbContext context, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<List<PretDto>> GetAllAsync()
        {
            var pretItems = await _context.Prets.Include(p => p.Images).ToListAsync();
            return pretItems.Select(MapToDto).ToList();
        }

        public async Task<PretDto> GetByIdAsync(Guid id)
        {
            var pret = await _context.Prets.Include(p => p.Images).FirstOrDefaultAsync(p => p.ProductId == id);
            return pret == null ? null : MapToDto(pret);
        }

        public async Task<PretDto> CreateAsync(PretDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var pret = MapToEntity(dto);
            pret.ProductId = Guid.NewGuid();

            if (dto.Images != null && dto.Images.Any())
            {
                pret.Images = new List<ProductImage>();

                foreach (var imageDto in dto.Images)
                {
                    if (!string.IsNullOrWhiteSpace(imageDto.ImageUrl) && imageDto.ImageUrl.StartsWith("data:image"))
                    {
                        // Image is in base64 format, upload to Cloudinary
                        var upload = await _cloudinaryService.UploadImageAsync(imageDto.ImageUrl);
                        pret.Images.Add(new ProductImage
                        {
                            ImageId = Guid.NewGuid(),
                            ProductId = pret.ProductId,
                            ImageUrl = upload.ImageUrl,
                            AltText = imageDto.AltText,
                            IsPrimary = imageDto.IsPrimary,
                            MimeType = upload.MimeType
                        });
                    }
                    else if (!string.IsNullOrWhiteSpace(imageDto.ImageUrl))
                    {
                        // Image URL is already valid (uploaded earlier), just save it
                        pret.Images.Add(new ProductImage
                        {
                            ImageId = Guid.NewGuid(),
                            ProductId = pret.ProductId,
                            ImageUrl = imageDto.ImageUrl,
                            AltText = imageDto.AltText,
                            IsPrimary = imageDto.IsPrimary,
                            MimeType = imageDto.MimeType
                        });
                    }
                }
            }

            _context.Prets.Add(pret);
            await _context.SaveChangesAsync();
            return MapToDto(pret);
        }

        public async Task<bool> UpdateAsync(Guid id, PretDto dto)
        {
            var pret = await _context.Prets.Include(p => p.Images).FirstOrDefaultAsync(p => p.ProductId == id);
            if (pret == null) return false;

            _context.Entry(pret).CurrentValues.SetValues(MapToEntity(dto, pret));

            _context.ProductImages.RemoveRange(pret.Images);
            pret.Images.Clear();

            if (dto.Images != null)
            {
                foreach (var imageDto in dto.Images)
                {
                    var upload = await _cloudinaryService.UploadImageAsync(imageDto.ImageUrl);
                    pret.Images.Add(new ProductImage
                    {
                        ImageId = Guid.NewGuid(),
                        ProductId = pret.ProductId,
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
            var pret = await _context.Prets.Include(p => p.Images).FirstOrDefaultAsync(p => p.ProductId == id);
            if (pret == null) return false;

            _context.Prets.Remove(pret);
            await _context.SaveChangesAsync();
            return true;
        }

        private PretDto MapToDto(Pret pret)
        {
            return new PretDto
            {
                ProductId = pret.ProductId,
                Name = pret.Name,
                Category = pret.Category,
                ShortDescription = pret.ShortDescription,
                OriginalPrice = pret.OriginalPrice,
                SalePrice = pret.SalePrice,
                SalePercentage = pret.SalePercentage,
                SizesAvailable = pret.SizesAvailable,
                StockQuantity = pret.StockQuantity,
                AverageRating = pret.AverageRating,
                RatingCount = pret.RatingCount,
                ColorOptions = pret.ColorOptions,
                Material = pret.Material,
                IsNew = pret.IsNew,
                IsExclusive = pret.IsExclusive,
                ShippingInfo = pret.ShippingInfo,
                Note = pret.Note,
                Images = pret.Images?.Select(i => new HaneensCollection.DTOs.ProductImageDto
                {
                    ImageId = i.ImageId,
                    ImageUrl = i.ImageUrl,
                    AltText = i.AltText,
                    IsPrimary = i.IsPrimary,
                    MimeType = i.MimeType
                }).ToList()
            };
        }

        private Pret MapToEntity(PretDto dto, Pret existing = null)
        {
            var pret = existing ?? new Pret();
            pret.Name = dto.Name;
            pret.Category = dto.Category;
            pret.ShortDescription = dto.ShortDescription;
            pret.OriginalPrice = dto.OriginalPrice;
            pret.SalePrice = dto.SalePrice;
            pret.SalePercentage = dto.SalePercentage;
            pret.SizesAvailable = dto.SizesAvailable;
            pret.StockQuantity = dto.StockQuantity;
            pret.AverageRating = dto.AverageRating;
            pret.RatingCount = dto.RatingCount;
            pret.ColorOptions = dto.ColorOptions;
            pret.Material = dto.Material;
            pret.IsNew = dto.IsNew;
            pret.IsExclusive = dto.IsExclusive;
            pret.ShippingInfo = dto.ShippingInfo;
            pret.Note = dto.Note;
            return pret;
        }
    }
}
