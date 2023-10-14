using Microsoft.EntityFrameworkCore;
using WebBlog.Data.Models;
using WebBlog.Data.Data;
using WebBlog.Data.DTOs;
using AutoMapper;

namespace WebBlog.Service.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CommentService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommentDTO> AddComment(CommentDTO comment)
        {
            _context.Comments.Add(_mapper.Map<Comment>(comment));
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<CommentDTO> DeleteComment(string commentID)
        {
            var comment = await _context.Comments.FindAsync(commentID);
            if (comment is null)
            {
                return null;
            }
            _context.Remove(comment);
            await _context.SaveChangesAsync();
            return _mapper.Map<CommentDTO>(comment);
        }

        public async Task<List<Comment>> GetCommentsForPost(string postID)
        {
            var comments = await _context.Comments.Where(c => c.PostId == postID).ToListAsync();
            return comments;
        }
    }
}
