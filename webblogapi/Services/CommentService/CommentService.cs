using Microsoft.EntityFrameworkCore;
namespace webblogapi.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly DataContext _context;
        public CommentService(DataContext context)
        {
            _context = context;
        }

        public async Task<Comment> AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> DeleteComment(string commentID)
        {
            var comment = await _context.Comments.FindAsync(commentID);
            if (comment is null)
            {
                return null;
            }
            _context.Remove(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<List<Comment>> GetCommentsForPost(string postID)
        {
            var comments = await _context.Comments.Where(c => c.PostId == postID).ToListAsync();
            return comments;
        }
    }
}
