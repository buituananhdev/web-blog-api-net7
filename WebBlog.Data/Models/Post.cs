using System;
using System.Collections.Generic;

namespace WebBlog.Data.Models;

public partial class Post
{
    public string PostId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? Thumbnail { get; set; }

    public int? ViewCount { get; set; }

    public string? UserId { get; set; }

    public long? Timestamp { get; set; }

    public string? PostType { get; set; }

    public virtual List<Comment> Comments { get; set; } = new List<Comment>();

    public virtual User? User { get; set; }

    public virtual List<Vote> Votes { get; set; } = new List<Vote>();

    public virtual List<Tag> Tags { get; set; } = new List<Tag>();
}
