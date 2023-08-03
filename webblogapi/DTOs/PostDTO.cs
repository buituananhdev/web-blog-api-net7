namespace webblogapi.DTOs
{
    public class PostDTO
    {
        public string PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? Thumbnail { get; set; }
        public int? ViewCount { get; set; }
        public long? Timestamp { get; set; }
        public string? PostType { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
