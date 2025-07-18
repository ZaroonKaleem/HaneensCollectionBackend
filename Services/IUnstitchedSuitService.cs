using HaneensCollection.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaneensCollection.IServices
{
    public interface IUnstitchedSuitService
    {
        Task<List<UnstitchedSuitDto>> GetAllAsync();
        Task<UnstitchedSuitDto> GetByIdAsync(Guid id);
        Task<UnstitchedSuitDto> CreateAsync(UnstitchedSuitDto dto);
        Task<bool> UpdateAsync(Guid id, UnstitchedSuitDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
