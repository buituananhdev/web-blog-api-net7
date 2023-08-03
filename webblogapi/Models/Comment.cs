using System;
using System.Collections.Generic;

namespace webblogapi.Models;

public partial class Comment
{
    public string CommentId { get; set; } = null!;

    public string CommentText { get; set; } = null!;

    public string? UserId { get; set; }

    public string? PostId { get; set; }

    public long? Timestamp { get; set; }

    public string? CommentType { get; set; }

    public virtual Post? Post { get; set; }

    public virtual User? User { get; set; }
}
