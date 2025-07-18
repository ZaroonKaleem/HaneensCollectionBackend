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
    public class UnstitchedSuitService : IUnstitchedSuitService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;

        public UnstitchedSuitService(ApplicationDbContext context, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<List<UnstitchedSuitDto>> GetAllAsync()
        {
            var suits = await _context.UnstitchedSuits
                .Include(p => p.Images)
                .ToListAsync();

            return suits.Select(MapToDto).ToList();
        }

        public async Task<UnstitchedSuitDto> GetByIdAsync(Guid id)
        {
            var suit = await _context.UnstitchedSuits
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            return suit == null ? null : MapToDto(suit);
        }

        //public async Task<UnstitchedSuitDto> CreateAsync(UnstitchedSuitDto dto)
        //{
        //    var suit = MapToEntity(dto);
        //    suit.ProductId = Guid.NewGuid();

        //    if (dto.Images != null)
        //    {
        //        suit.Images = new List<ProductImage>();

        //        foreach (var imageDto in dto.Images)
        //        {
        //            if (imageDto.ImageUrl.StartsWith("data:image"))
        //            {
        //                var upload = await _cloudinaryService.UploadImageAsync(imageDto.ImageUrl);
        //                suit.Images.Add(new ProductImage
        //                {
        //                    ImageId = Guid.NewGuid(),
        //                    ProductId = suit.ProductId,
        //                    ImageUrl = upload.ImageUrl,
        //                    AltText = imageDto.AltText,
        //                    IsPrimary = imageDto.IsPrimary,
        //                    MimeType = upload.MimeType
        //                });
        //            }
        //        }
        //    }

        //    _context.UnstitchedSuits.Add(suit);
        //    await _context.SaveChangesAsync();
        //    return MapToDto(suit);
        //}

        public async Task<UnstitchedSuitDto> CreateAsync(UnstitchedSuitDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var suit = MapToEntity(dto);
            suit.ProductId = Guid.NewGuid();

            if (dto.Images != null && dto.Images.Any())
            {
                suit.Images = new List<ProductImage>();

                foreach (var imageDto in dto.Images)
                {
                    if (!string.IsNullOrWhiteSpace(imageDto.ImageUrl) && imageDto.ImageUrl.StartsWith("data:image"))
                    {
                        // Upload base64 image to Cloudinary
                        var upload = await _cloudinaryService.UploadImageAsync(imageDto.ImageUrl);
                        suit.Images.Add(new ProductImage
                        {
                            ImageId = Guid.NewGuid(),
                            ProductId = suit.ProductId,
                            ImageUrl = upload.ImageUrl,
                            AltText = imageDto.AltText,
                            IsPrimary = imageDto.IsPrimary,
                            MimeType = upload.MimeType
                        });
                    }
                    else if (!string.IsNullOrWhiteSpace(imageDto.ImageUrl))
                    {
                        // Save existing URL directly
                        suit.Images.Add(new ProductImage
                        {
                            ImageId = Guid.NewGuid(),
                            ProductId = suit.ProductId,
                            ImageUrl = imageDto.ImageUrl,
                            AltText = imageDto.AltText,
                            IsPrimary = imageDto.IsPrimary,
                            MimeType = imageDto.MimeType
                        });
                    }
                }
            }

            // Add the new suit to the database
            _context.UnstitchedSuits.Add(suit);
            await _context.SaveChangesAsync();

            // Return the DTO
            return MapToDto(suit);
        }

        public async Task<bool> UpdateAsync(Guid id, UnstitchedSuitDto dto)
        {
            var suit = await _context.UnstitchedSuits.Include(p => p.Images).FirstOrDefaultAsync(p => p.ProductId == id);
            if (suit == null) return false;

            _context.Entry(suit).CurrentValues.SetValues(MapToEntity(dto, suit));

            _context.ProductImages.RemoveRange(suit.Images);
            suit.Images.Clear();

            if (dto.Images != null)
            {
                foreach (var imageDto in dto.Images)
                {
                    if (imageDto.ImageUrl.StartsWith("data:image"))
                    {
                        var upload = await _cloudinaryService.UploadImageAsync(imageDto.ImageUrl);
                        suit.Images.Add(new ProductImage
                        {
                            ImageId = Guid.NewGuid(),
                            ProductId = suit.ProductId,
                            ImageUrl = upload.ImageUrl,
                            AltText = imageDto.AltText,
                            IsPrimary = imageDto.IsPrimary,
                            MimeType = upload.MimeType
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var suit = await _context.UnstitchedSuits.Include(p => p.Images).FirstOrDefaultAsync(p => p.ProductId == id);
            if (suit == null) return false;

            foreach (var image in suit.Images)
            {
                if (!string.IsNullOrEmpty(image.ImageUrl))
                {
                    try
                    {
                        var publicId = GetPublicIdFromUrl(image.ImageUrl);
                        await _cloudinaryService.DeleteImageAsync(publicId);
                    }
                    catch
                    {
                        // Optional: log error
                    }
                }
            }

            _context.UnstitchedSuits.Remove(suit);
            await _context.SaveChangesAsync();
            return true;
        }

        private string GetPublicIdFromUrl(string imageUrl)
        {
            var uri = new Uri(imageUrl);
            var segments = uri.Segments;
            var uploadIndex = Array.IndexOf(segments, "upload/") + 1;
            return string.Join("", segments.Skip(uploadIndex)).Split('.')[0];
        }

        private UnstitchedSuitDto MapToDto(UnstitchedSuit suit)
        {
            return new UnstitchedSuitDto
            {
                ProductId = suit.ProductId,
                Name = suit.Name,
                Category = suit.Category,
                ShortDescription = suit.ShortDescription,
                OriginalPrice = suit.OriginalPrice,
                SalePrice = suit.SalePrice,
                SalePercentage = suit.SalePercentage,
                SizesAvailable = suit.SizesAvailable,
                StockQuantity = suit.StockQuantity,
                AverageRating = suit.AverageRating,
                RatingCount = suit.RatingCount,
                ColorOptions = suit.ColorOptions,
                Material = suit.Material,
                IsNew = suit.IsNew,
                IsExclusive = suit.IsExclusive,
                ShippingInfo = suit.ShippingInfo,
                Note = suit.Note,
                Images = suit.Images?.Select(i => new HaneensCollection.DTOs.ProductImageDto
                {
                    ImageId = i.ImageId,
                    ImageUrl = i.ImageUrl,
                    AltText = i.AltText,
                    IsPrimary = i.IsPrimary,
                    MimeType = i.MimeType
                }).ToList(),

                Shirt = new ShirtDetailsDto
                {
                    EmbroideredNeckline = suit.Shirt.EmbroideredNeckline,
                    DigitalPrint = suit.Shirt.DigitalPrint,
                    EmbroideredBorder = suit.Shirt.EmbroideredBorder,
                    Fabric = suit.Shirt.Fabric,
                    Color = suit.Shirt.Color
                },
                Dupatta = new DupattaDetailsDto
                {
                    DigitalPrint = suit.Dupatta.DigitalPrint,
                    Fabric = suit.Dupatta.Fabric,
                    Color = suit.Dupatta.Color
                },
                Trouser = new TrouserDetailsDto
                {
                    Description = suit.Trouser.Description,
                    Fabric = suit.Trouser.Fabric,
                    Color = suit.Trouser.Color
                }
            };
        }

        private UnstitchedSuit MapToEntity(UnstitchedSuitDto dto, UnstitchedSuit existing = null)
        {
            var suit = existing ?? new UnstitchedSuit
            {
                Shirt = new ShirtDetails(),
                Dupatta = new DupattaDetails(),
                Trouser = new TrouserDetails()
            };

            suit.Name = dto.Name;
            suit.Category = dto.Category;
            suit.ShortDescription = dto.ShortDescription;
            suit.OriginalPrice = dto.OriginalPrice;
            suit.SalePrice = dto.SalePrice;
            suit.SalePercentage = dto.SalePercentage;
            suit.SizesAvailable = dto.SizesAvailable;
            suit.StockQuantity = dto.StockQuantity;
            suit.AverageRating = dto.AverageRating;
            suit.RatingCount = dto.RatingCount;
            suit.ColorOptions = dto.ColorOptions;
            suit.Material = dto.Material;
            suit.IsNew = dto.IsNew;
            suit.IsExclusive = dto.IsExclusive;
            suit.ShippingInfo = dto.ShippingInfo;
            suit.Note = dto.Note;

            if (dto.Shirt != null)
            {
                suit.Shirt.EmbroideredNeckline = dto.Shirt.EmbroideredNeckline;
                suit.Shirt.DigitalPrint = dto.Shirt.DigitalPrint;
                suit.Shirt.EmbroideredBorder = dto.Shirt.EmbroideredBorder;
                suit.Shirt.Fabric = dto.Shirt.Fabric;
                suit.Shirt.Color = dto.Shirt.Color;
            }

            if (dto.Dupatta != null)
            {
                suit.Dupatta.DigitalPrint = dto.Dupatta.DigitalPrint;
                suit.Dupatta.Fabric = dto.Dupatta.Fabric;
                suit.Dupatta.Color = dto.Dupatta.Color;
            }

            if (dto.Trouser != null)
            {
                suit.Trouser.Description = dto.Trouser.Description;
                suit.Trouser.Fabric = dto.Trouser.Fabric;
                suit.Trouser.Color = dto.Trouser.Color;
            }

            return suit;
        }
    }
}
