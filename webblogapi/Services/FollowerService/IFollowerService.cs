namespace webblogapi.Services.FollowerService
{
    public interface IFollowerService
    {
        Task FollowUser(Follower follower);
        Task<Follower> UnfollowUser(string userId, string followerUserId);
        Task<List<User>> GetFollowers(string userId);
        Task<bool> IsFollowingUser(string userId, string followerUserId);
    }
}
