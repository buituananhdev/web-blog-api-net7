using Microsoft.EntityFrameworkCore;
using WebBlog.Data.Models;
using WebBlog.Data.Data;
using Microsoft.Extensions.Logging;

namespace WebBlog.Service.Services.VoteService
{
    public class VoteService : IVoteService
    {
        private readonly DataContext _context;
        private readonly ILogger<VoteService> _logger;

        public VoteService(DataContext context, ILogger<VoteService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> VoteDown(Vote votedown)
        {
            try
            {
                var existingVote = await _context.Votes.FirstOrDefaultAsync(v => v.UserId == votedown.UserId && v.PostId == votedown.PostId);

                if (existingVote != null)
                {
                    if (existingVote.VoteType == -1)
                    {
                        return false;
                    }

                    existingVote.VoteType = -1;
                }
                else
                {
                    votedown.VoteType = -1;
                    _context.Votes.Add(votedown);
                }

                await _context.SaveChangesAsync();
                return true;
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Vote down error");
                throw;
            }
        }

        public async Task<bool> VoteUp(Vote voteup)
        {
            try
            {
                var existingVote = await _context.Votes.FirstOrDefaultAsync(v => v.UserId == voteup.UserId && v.PostId == voteup.PostId);

                if (existingVote != null)
                {
                    if (existingVote.VoteType == 1)
                    {
                        return false;
                    }

                    existingVote.VoteType = 1;
                }
                else
                {
                    voteup.VoteType = 1;
                    _context.Votes.Add(voteup);
                }

                await _context.SaveChangesAsync();
                return true;
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Vote up error");
                throw;
            }
        }
    }
}
