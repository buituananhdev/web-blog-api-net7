﻿using System;
using System.Collections.Generic;

namespace WebBlog.Data.Models;

public partial class Conversation
{
    public string ConversationId { get; set; } = null!;

    public string? ConversationName { get; set; }

    public virtual List<Message> Messages { get; set; } = new List<Message>();
}
