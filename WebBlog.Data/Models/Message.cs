using System;
using System.Collections.Generic;

namespace WebBlog.Data.Models;

public partial class Message
{
    public string MessageId { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string SenderId { get; set; } = null!;

    public string ConversationId { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Conversation Conversation { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}
