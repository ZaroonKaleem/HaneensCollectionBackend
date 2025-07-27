using HaneensCollection.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaneensCollection.IServices
{
    public interface ILuxuryService
    {
        Task<List<LuxuryDto>> GetAllAsync();
        Task<LuxuryDto> GetByIdAsync(Guid id);
        Task<LuxuryDto> CreateAsync(LuxuryDto dto);
        Task<bool> UpdateAsync(Guid id, LuxuryDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
