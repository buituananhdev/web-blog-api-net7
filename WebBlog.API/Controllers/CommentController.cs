using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBlog.Data.DTOs;
using WebBlog.Service.Services.CommentService;
using WebBlog.Utility.Helpers;

namespace WebBlog.API.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _CommentService;
        public CommentController(ICommentService CommentService) => _CommentService = CommentService;

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<List<CommentDTO>>> AddComment(CommentDTO comment)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
                comment.CommentId = UUID.Generate();
                comment.UserId = userId;
                DateTime dateTime = DateTime.Now;
                comment.Timestamp = DateTimeHelper.ConvertToUnixTimeSeconds(dateTime);
                var result = await _CommentService.AddComment(comment);

                var commentDTO = new CommentDTO
                {
                    CommentId = result.CommentId,
                    CommentText = result.CommentText,
                    UserId = result.UserId,
                    PostId = result.PostId,
                    Timestamp = result.Timestamp,
                    CommentType = result.CommentType
                };

                return Ok(new { status = "success", data = commentDTO });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { status = "failure", message = "Loi server" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Comment>>> GetCommentsForPost(string id)
        {
            try
            {
                var result = await _CommentService.GetCommentsForPost(id);

                var commentsDTO = result.Select(c => new CommentDTO
                {
                    CommentId = c.CommentId,
                    CommentText = c.CommentText,
                    UserId = c.UserId,
                    PostId = c.PostId,
                    Timestamp = c.Timestamp,
                    CommentType = c.CommentType
                }).ToList();

                return Ok(new { status = "success", data = commentsDTO });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { status = "failure", message = "Loi server" });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Comment>>> DeleteComment(string id)
        {
            try
            {
                var result = await _CommentService.DeleteComment(id);
                return Ok(new { status = "success", message = "Delete comment successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { status = "failure", message = "Loi server" });
            }
        }

    }
}
