using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webblogapi.Helpers;
using webblogapi.Models;
using webblogapi.Services.CommentService;
using webblogapi.Services.FollowerService;

namespace webblogapi.Controllers
{
    [Route("api/followers")]
    [ApiController]
    public class FollowerController : ControllerBase
    {
        private readonly IFollowerService _FollowerService;
        public FollowerController(IFollowerService FollowerService) => _FollowerService = FollowerService;

        [Authorize]
        [HttpPost("follow")] 
        public async Task<ActionResult<List<Follower>>> FolowerUser(Follower follower)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest(new { status = "failure", message = "You don't have permission to access this page" });
            }
            try
            {

                var userId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
                follower.FollowerId = UUID.Generate();
                follower.FollowerUserId = userId;
                await _FollowerService.FollowUser(follower);
                return Ok(new { status = "success", message = "Follower user successfully"});
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { status = "failure", message = "Loi server" });
            }
        }

        [Authorize]
        [HttpDelete("unFollow")]
        public async Task<ActionResult<List<Follower>>> UnFolowerUser(string userId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest(new { status = "failure", message = "You don't have permission to access this page" });
            }
            try
            {
                var followerUserId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
                var result = await _FollowerService.UnfollowUser(userId, followerUserId);
                if(result is null)
                {
                    return NotFound(new { status = "failure", message = "You have not followed this user yet" });
                }
                return Ok(new { status = "success", message = "Unfollower user successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { status = "failure", message = "Loi server" });
            }
        }

        [Authorize]
        [HttpGet("isfollowing")]
        public async Task<ActionResult<List<Follower>>> IsFollowingUser(string userId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest(new { status = "failure", message = "You don't have permission to access this page" });
            }
            try
            {
                var followerUserId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
                var result = await _FollowerService.IsFollowingUser(userId, followerUserId);
                return Ok(new { status = "success", isFollowing = result });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { status = "failure", message = "Loi server" });
            }
        }
    }
}
