using Microsoft.EntityFrameworkCore;
using WebBlog.Data.Models;
using WebBlog.Data.Data;

namespace WebBlog.Service.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly DataContext _context;
        public PostService(DataContext context)
        {
            _context = context;
        }
        public async Task<Post> CreatePost(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<Post> DeletePost(string postID)
        {
            var post = await _context.Posts.FindAsync(postID);
            if (post is null)
            {
                return null;
            }
            _context.Remove(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<List<Post>> GetPopularPosts(int limit)
        {
            var posts = await _context.Posts.ToListAsync();
            var votes = await _context.Votes.ToListAsync();

            var postVoteCounts = posts.Select(post =>
            {
                var voteUpCount = votes.Count(vote => vote.PostId == post.PostId && vote.VoteType == 1);
                var voteDownCount = votes.Count(vote => vote.PostId == post.PostId && vote.VoteType == -1);
                var voteCount = voteUpCount - voteDownCount;

                return new
                {
                    Post = post,
                    VoteCount = voteCount
                };
            }).OrderByDescending(pvc => pvc.VoteCount)
            .Take(limit)
            .ToList();

            var popularPosts = postVoteCounts.Select(pvc => pvc.Post).ToList();

            return popularPosts;
        }


        public async Task<Post> GetPost(string postID)
        {
            var post = await _context.Posts.FindAsync(postID);
            if (post is null)
            {
                return null;
            }
            return post;
        }

        public async Task IncrementViewCount(string postID)
        {
            var post = await _context.Posts.FindAsync(postID);
            post.ViewCount++;
            await _context.SaveChangesAsync();
        }

        public async Task<Post> UpdatePost(string id, Post request)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post is null)
            {
                return null;
            }
            post.Title = request.Title;
            post.Content = request.Content;
            post.Thumbnail = request.Thumbnail;
            await _context.SaveChangesAsync();
            return post;
        }
    }
}
