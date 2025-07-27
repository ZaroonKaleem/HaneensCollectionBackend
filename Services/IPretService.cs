using HaneensCollection.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaneensCollection.IServices
{
    public interface IPretService
    {
        Task<List<PretDto>> GetAllAsync();
        Task<PretDto> GetByIdAsync(Guid id);
        Task<PretDto> CreateAsync(PretDto dto);
        Task<bool> UpdateAsync(Guid id, PretDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
