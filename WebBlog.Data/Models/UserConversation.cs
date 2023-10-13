using System;
using System.Collections.Generic;

namespace WebBlog.Data.Models;

public partial class UserConversation
{
    public string? UserId { get; set; }

    public string? ConversationId { get; set; }

    public virtual Conversation? Conversation { get; set; }

    public virtual User? User { get; set; }
}
