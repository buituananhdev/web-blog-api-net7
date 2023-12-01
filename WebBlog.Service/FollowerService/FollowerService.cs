using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebBlog.Data.Data;
using WebBlog.Data.Models;

namespace WebBlog.Service.Services.FollowerService
{
    public class FollowerService : IFollowerService
    {
        private readonly DataContext _context;
        private readonly ILogger<FollowerService> _logger;

        public FollowerService(DataContext context, ILogger<FollowerService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task FollowUser(Follower follower)
        {
            try
            {
                _context.Add(follower);
                await _context.SaveChangesAsync();
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Follow user error");
                throw;
            }
        }

        public async Task<List<User>> GetFollowers(string userId)
        {
            try
            {
                var followerIds = await _context.Followers
                .Where(f => f.UserId == userId)
                .Select(f => f.FollowerUserId)
                .ToListAsync();

                var followers = await _context.Users
                    .Where(u => followerIds.Contains(u.UserId))
                    .ToListAsync();

                return followers;
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Get follower error");
                throw;
            }
        }

        public async Task<bool> IsFollowingUser(string userId, string followerUserId)
        {
            return await _context.Followers.AnyAsync(f => f.UserId == userId && f.FollowerUserId == followerUserId);
        }

        public async Task<Follower> UnfollowUser(string userId, string followerUserId)
        {
            try
            {
                var follower = await _context.Followers.FirstOrDefaultAsync(f => f.UserId == userId && f.FollowerUserId == followerUserId);

                if (follower is null)
                {
                    return null;
                }
                _context.Remove(follower);
                await _context.SaveChangesAsync();
                return follower;
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Unfollow user error");
                throw;
            }
        }
    }
}
