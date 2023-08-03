namespace webblogapi.Services.CommentService
{
    public interface ICommentService
    {
        Task<List<Comment>> GetCommentsForPost(string postID);
        Task<Comment> AddComment(Comment comment);
        Task<Comment> DeleteComment(string commentID);
    }
}
