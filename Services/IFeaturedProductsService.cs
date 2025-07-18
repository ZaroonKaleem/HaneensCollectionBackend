using HaneensCollection.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaneensCollection.IServices
{
    public interface IFeaturedProductService
    {
        Task<List<FeaturedProductDto>> GetFeaturedProductsAsync();
        Task<FeaturedProductDto> GetFeaturedProductAsync(Guid id);
        Task<FeaturedProductDto> CreateFeaturedProductAsync(FeaturedProductDto productDto);
        Task<bool> UpdateFeaturedProductAsync(Guid id, FeaturedProductDto productDto);
        Task<bool> DeleteFeaturedProductAsync(Guid id);
    }
}