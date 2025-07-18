using HaneensCollection.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaneensCollection.IServices
{
    public interface IStitchedSuitService
    {
        Task<List<StitchedSuitDto>> GetAllAsync();
        Task<StitchedSuitDto> GetByIdAsync(Guid id);
        Task<StitchedSuitDto> CreateAsync(StitchedSuitDto dto);
        Task<bool> UpdateAsync(Guid id, StitchedSuitDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
