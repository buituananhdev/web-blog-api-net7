using Microsoft.EntityFrameworkCore;
using WebBlog.Data.Models;
using WebBlog.Data.Data;
using WebBlog.Data.DTOs;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace WebBlog.Service.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CommentService> _logger;

        public CommentService(DataContext context, IMapper mapper, ILogger<CommentService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CommentDTO> AddComment(CommentDTO comment)
        {
            try
            {
                _context.Comments.Add(_mapper.Map<Comment>(comment));
                await _context.SaveChangesAsync();
                return comment;
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Add comment error");
                throw;
            }
        }

        public async Task<CommentDTO> DeleteComment(string commentID)
        {
            try
            {
                var comment = await _context.Comments.FindAsync(commentID);
                if (comment is null)
                {
                    return null;
                }
                _context.Remove(comment);
                await _context.SaveChangesAsync();
                return _mapper.Map<CommentDTO>(comment);
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Delete comment error");
                throw;
            }
        }

        public async Task<List<Comment>> GetCommentsForPost(string postID)
        {
            try
            {
                var comments = await _context.Comments.Where(c => c.PostId == postID).ToListAsync();
                return comments;
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Get comments error");
                throw;
            }
        }
    }
}
