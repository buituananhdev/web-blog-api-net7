namespace webblogapi.Services.PostService
{
    public interface IPostService
    {
        Task<List<Post>> GetPopularPosts(int limit);
        Task<Post> CreatePost(Post post);
        Task<Post> GetPost(string postID);
        Task<Post?> DeletePost(string postID);
        Task<Post> UpdatePost(string id, Post post);
        Task IncrementViewCount(string postID);
    }
}
