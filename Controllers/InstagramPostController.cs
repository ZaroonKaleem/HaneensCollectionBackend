using HaneensCollection.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class InstagramPostController : ControllerBase
{
    private readonly IInstagramPostService _postService;
    private readonly ILogger<InstagramPostController> _logger;

    public InstagramPostController(
        IInstagramPostService postService,
        ILogger<InstagramPostController> logger)
    {
        _postService = postService ?? throw new ArgumentNullException(nameof(postService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InstagramPostResponseDto>>> GetAllPosts()
    {
        try
        {
            var posts = await _postService.GetAllPostsAsync();
            return Ok(posts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all Instagram posts");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InstagramPostResponseDto>> GetPostById(int id)
    {
        try
        {
            var post = await _postService.GetPostByIdAsync(id);
            return post == null ? NotFound() : Ok(post);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting Instagram post with ID {id}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    [RequestSizeLimit(50_000_000)]

    public async Task<ActionResult<InstagramPostResponseDto>> CreatePost([FromForm] InstagramPostDto postDto)
    {
        try
        {
            _logger.LogInformation("Starting Instagram post creation");

            if (postDto.Image == null)
            {
                _logger.LogError("Image file is null");
                return BadRequest("Image file is required");
            }

            if (postDto.Image.Length == 0)
            {
                _logger.LogError("Image file is empty");
                return BadRequest("Image file cannot be empty");
            }

            _logger.LogInformation($"Processing image: {postDto.Image.FileName}, Size: {postDto.Image.Length} bytes");

            var createdPost = await _postService.CreatePostAsync(postDto);

            _logger.LogInformation($"Successfully created Instagram post with ID: {createdPost.Id}");

            return CreatedAtAction(
                nameof(GetPostById),
                new { id = createdPost.Id },
                createdPost);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreatePost. Message: {Message}\nStackTrace: {StackTrace}",
                ex.Message, ex.StackTrace);

            return StatusCode(500, new
            {
                Error = "Internal server error",
                Message = ex.Message,
                Detailed = ex.InnerException?.Message,
                StackTrace = ex.StackTrace
            });
        }
    }

    [HttpPut("{id}")]
    [RequestSizeLimit(50_000_000)]
    public async Task<ActionResult<InstagramPostResponseDto>> UpdatePost(int id, [FromForm] InstagramPostDto postDto)
    {
        try
        {
            var updatedPost = await _postService.UpdatePostAsync(id, postDto);
            return updatedPost == null ? NotFound() : Ok(updatedPost);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating Instagram post with ID {id}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        try
        {
            var success = await _postService.DeletePostAsync(id);
            return success ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting Instagram post with ID {id}");
            return StatusCode(500, "Internal server error");
        }
    }
}