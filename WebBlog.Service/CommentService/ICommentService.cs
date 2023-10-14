using WebBlog.Data.DTOs;
using WebBlog.Data.Models;

namespace WebBlog.Service.Services.CommentService
{
    public interface ICommentService
    {
        Task<List<Comment>> GetCommentsForPost(string postID);
        Task<CommentDTO> AddComment(CommentDTO comment);
        Task<CommentDTO> DeleteComment(string commentID);
    }
}
