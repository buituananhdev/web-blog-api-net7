using Microsoft.EntityFrameworkCore;
using WebBlog.Data.Models;
using WebBlog.Data.Data;

namespace WebBlog.Service.Services.VoteService
{
    public class VoteService : IVoteService
    {
        private readonly DataContext _context;
        public VoteService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> VoteDown(Vote votedown)
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
        }

        public async Task<bool> VoteUp(Vote voteup)
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
        }
    }
}
