using Microsoft.EntityFrameworkCore;
namespace webblogapi.Services.FollowerService
{
    public class FollowerService : IFollowerService
    {
        private readonly DataContext _context;
        public FollowerService(DataContext context)
        {
            _context = context;
        }

        public async Task FollowUser(Follower follower)
        {
            _context.Add(follower);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetFollowers(string userId)
        {
            var followerIds = await _context.Followers
                .Where(f => f.UserId == userId)
                .Select(f => f.FollowerUserId)
                .ToListAsync();

            var followers = await _context.Users
                .Where(u => followerIds.Contains(u.UserId))
                .ToListAsync();

            return followers;
        }

        public async Task<bool> IsFollowingUser(string userId, string followerUserId)
        {
            return await _context.Followers.AnyAsync(f => f.UserId == userId && f.FollowerUserId == followerUserId);
        }

        public async Task<Follower> UnfollowUser(string userId, string followerUserId)
        {
            var follower = await _context.Followers.FirstOrDefaultAsync(f => f.UserId == userId && f.FollowerUserId == followerUserId);

            if (follower is null)
            {
                return null;
            }
            _context.Remove(follower);
            await _context.SaveChangesAsync();
            return follower;
        }
    }
}
