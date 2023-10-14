using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBlog.Utility.Helpers;
using WebBlog.Service.Services.VoteService;

namespace WebBlog.API.Controllers
{
    [Route("api/votes")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly IVoteService _VoteService;
        public VoteController(IVoteService VoteService) => _VoteService = VoteService;

        [Authorize]
        [HttpPost("voteUp")]
        public async Task<ActionResult<List<Vote>>> VoteUp(Vote vote)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest(new { status = "failure", message = "You don't have permission to access this page" });
            }
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
                vote.VoteId = UUID.Generate();
                vote.UserId = userId;
                var result = await _VoteService.VoteUp(vote);
                if(result)
                {
                    return Ok(new { status = "success" , message = "Vote up succesfully"});
                }
                else
                {
                    return BadRequest(new { status = "failure", message = "You voted up for this post" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { status = "failure", message = "Loi server" });
            }
        }

        [Authorize]
        [HttpPost("voteDown")]
        public async Task<ActionResult<List<Vote>>> VoteDown(Vote vote)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest(new { status = "failure", message = "You don't have permission to access this page" });
            }
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
                vote.VoteId = Guid.NewGuid().ToString();
                vote.UserId = userId;
                var result = await _VoteService.VoteDown(vote);
                if (result)
                {
                    return Ok(new { status = "success", message = "Vote down succesfully" });
                }
                else
                {
                    return BadRequest(new { status = "failure", message = "You voted down for this post" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { status = "failure", message = "Loi server" });
            }
        }
    }
}
