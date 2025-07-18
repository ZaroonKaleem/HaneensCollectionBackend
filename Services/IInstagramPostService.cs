// Services/II
// InstagramPostService.cs
using HaneensCollection.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaneensCollection.Services
{
    public interface IInstagramPostService
    {
        Task<IEnumerable<InstagramPostResponseDto>> GetAllPostsAsync();
        Task<InstagramPostResponseDto?> GetPostByIdAsync(int id);  // Added ? for nullable return
        Task<InstagramPostResponseDto> CreatePostAsync(InstagramPostDto postDto);
        Task<InstagramPostResponseDto?> UpdatePostAsync(int id, InstagramPostDto postDto);  // Added ? for nullable return
        Task<bool> DeletePostAsync(int id);
    }
}