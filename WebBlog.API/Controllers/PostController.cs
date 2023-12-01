using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBlog.Data.DTOs;
using WebBlog.Service.Services.CommentService;
using WebBlog.Service.Services.PostService;
using WebBlog.Utility.Helpers;

namespace WebBlog.API.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _PostService;
        private readonly ICommentService _CommentService;
        private readonly IMapper _mapper;

        public PostController(IPostService PostService, ICommentService CommentService, IMapper mapper)
        {
            _PostService = PostService;
            _CommentService = CommentService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        // Add new post
        public async Task<ActionResult<List<Post>>> CreatePost(Post post)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest(new { status = "failure", message = "You don't have permission to access this page" });
            }
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
                post.UserId = userId;
                post.PostId = UUID.Generate();
                DateTime dateTime = DateTime.Now;
                post.Timestamp = DateTimeHelper.ConvertToUnixTimeSeconds(dateTime);
                post.ViewCount = 0;
                var result = await _PostService.CreatePost(post);
                return Ok(new { status = "success", data = result });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { status = "failure", message = "Loi server" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO>> GetPost(string id)
        {
            try
            {
                var post = await _PostService.GetPost(id);
                if (post is null)
                {
                    return NotFound(new { status = "failure", message = "Post not found" });
                }

                var comments = await _CommentService.GetCommentsForPost(id);

                var postDto = _mapper.Map<PostDTO>(post);
                postDto.Comments = comments;

                return Ok(new { status = "success", data = postDto });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { status = "failure", message = "Loi server" });
            }
        }

        [HttpGet("popular")]
        public async Task<ActionResult<List<Post>>> GetPopularPosts(int limit = 10)
        {
            try
            {
                var posts = await _PostService.GetPopularPosts(limit);
                return Ok(new { status = "success", data = posts });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { status = "failure", message = "Loi server" });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Post>>> DeletePost(string id = "")
        {

            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest(new { status = "failure", message = "You don't have permission to access this page" });
            }
            try
            {
                var result = await _PostService.DeletePost(id);
                if (result is null)
                {
                    return NotFound(new { status = "failure", message = "Post not found" });
                }
                return Ok(new { status = "success", message = "Delete post successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { status = "failure", message = "Loi server" });
            }
        }

        [HttpPatch("{id}/increment_view")]
        public async Task<ActionResult<List<Post>>> IncrementViewCount(string id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest(new { status = "failure", message = "You don't have permission to access this page" });
            }
            try
            {
                await _PostService.IncrementViewCount(id);
                return Ok(new { status = "success", message = "Increment view successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { status = "failure", message = "Loi server" });
            }
        }
    }
}
