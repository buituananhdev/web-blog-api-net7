using Microsoft.EntityFrameworkCore;
using WebBlog.Data.Models;
using WebBlog.Data.Data;
using Microsoft.Extensions.Logging;

namespace WebBlog.Service.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly DataContext _context;
        private readonly ILogger<PostService> _logger;

        public PostService(DataContext context, ILogger<PostService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Post> CreatePost(Post post)
        {
            try
            {
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                return post;
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Create post error");
                throw;
            }
        }

        public async Task<Post> DeletePost(string postID)
        {
            try
            {
                var post = await _context.Posts.FindAsync(postID);
                if (post is null)
                {
                    return null;
                }
                _context.Remove(post);
                await _context.SaveChangesAsync();
                return post;
            } catch(Exception ex)
            {
                _logger.LogError(ex, "Delete post error");
                throw;
            }
        }

        public async Task<List<Post>> GetPopularPosts(int limit)
        {
            try
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
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Get popular posts error");
                throw;
            }
        }


        public async Task<Post> GetPost(string postID)
        {
            try
            {
                var post = await _context.Posts.FindAsync(postID);
                if (post is null)
                {
                    return null;
                }
                return post;
            } catch(Exception ex)
            {
                _logger.LogError(ex, "Get post error");
                throw;
            }
        }

        public async Task IncrementViewCount(string postID)
        {
            try
            {
                var post = await _context.Posts.FindAsync(postID);
                post.ViewCount++;
                await _context.SaveChangesAsync();
            } catch(Exception ex)
            {
                _logger.LogError(ex, "IncrementViewCount error");
                throw;
            }
        }

        public async Task<Post> UpdatePost(string id, Post request)
        {
            try
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
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Update post error");
                throw;
            }
        }
    }
}
