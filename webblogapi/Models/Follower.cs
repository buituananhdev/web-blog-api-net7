using System;
using System.Collections.Generic;

namespace webblogapi.Models;

public partial class Follower
{
    public string FollowerId { get; set; } = null!;

    public string? UserId { get; set; }

    public string? FollowerUserId { get; set; }

    public virtual User? FollowerUser { get; set; }

    public virtual User? User { get; set; }
}
