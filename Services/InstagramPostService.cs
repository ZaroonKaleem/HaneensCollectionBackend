using HaneensCollection.Data;
using HaneensCollection.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaneensCollection.Services
{
    public class InstagramPostService : IInstagramPostService
    {
        private readonly ApplicationDbContext _context;
        private readonly ImageCompressionService _imageCompressionService;
        private readonly ILogger<InstagramPostService> _logger;

        public InstagramPostService(
            ApplicationDbContext context,
            ImageCompressionService imageCompressionService,
            ILogger<InstagramPostService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _imageCompressionService = imageCompressionService ?? throw new ArgumentNullException(nameof(imageCompressionService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<InstagramPostResponseDto>> GetAllPostsAsync()
        {
            try
            {
                var posts = await _context.InstagramPosts
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();

                return posts.Select(p => MapToResponseDto(p));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all Instagram posts");
                throw;
            }
        }

        public async Task<InstagramPostResponseDto> GetPostByIdAsync(int id)
        {
            try
            {
                var post = await _context.InstagramPosts.FindAsync(id);
                if (post == null) return null;
                return MapToResponseDto(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting Instagram post with ID {id}");
                throw;
            }
        }

        public async Task<InstagramPostResponseDto> CreatePostAsync(InstagramPostDto postDto)
        {
            try
            {
                if (postDto.Image == null || postDto.Image.Length == 0)
                {
                    throw new ArgumentException("Image is required");
                }

                var (base64String, contentType) = await _imageCompressionService.CompressAndConvertToBase64(postDto.Image);

                var post = new InstagramPost
                {
                    ImageUrl = base64String,
                    InstagramLink = postDto.InstagramLink,
                    ImageContentType = contentType
                };

                _context.InstagramPosts.Add(post);
                await _context.SaveChangesAsync();

                return MapToResponseDto(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Instagram post");
                throw;
            }
        }

        public async Task<InstagramPostResponseDto> UpdatePostAsync(int id, InstagramPostDto postDto)
        {
            try
            {
                var existingPost = await _context.InstagramPosts.FindAsync(id);
                if (existingPost == null) return null;

                existingPost.InstagramLink = postDto.InstagramLink;
                existingPost.UpdatedAt = DateTime.UtcNow;

                if (postDto.Image != null && postDto.Image.Length > 0)
                {
                    var (base64String, contentType) = await _imageCompressionService.CompressAndConvertToBase64(postDto.Image);
                    existingPost.ImageUrl = base64String;
                    existingPost.ImageContentType = contentType;
                }

                await _context.SaveChangesAsync();
                return MapToResponseDto(existingPost);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating Instagram post with ID {id}");
                throw;
            }
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            try
            {
                var post = await _context.InstagramPosts.FindAsync(id);
                if (post == null) return false;

                _context.InstagramPosts.Remove(post);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting Instagram post with ID {id}");
                throw;
            }
        }

        private InstagramPostResponseDto MapToResponseDto(InstagramPost post)
        {
            return new InstagramPostResponseDto
            {
                Id = post.Id,
                ImageUrl = $"data:{post.ImageContentType};base64,{post.ImageUrl}",
                InstagramLink = post.InstagramLink,
                ImageContentType = post.ImageContentType,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt
            };
        }
    }
}