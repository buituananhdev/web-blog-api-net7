using System;
using System.Collections.Generic;

namespace WebBlog.Data.Models;

public partial class Token
{
    public string Id { get; set; } = null!;

    public string? Email { get; set; }

    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }

    public long? ExpirationTime { get; set; }

    public long? CreatedAt { get; set; }

    public virtual User? EmailNavigation { get; set; }
}
