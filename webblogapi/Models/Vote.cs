using System;
using System.Collections.Generic;

namespace webblogapi.Models;

public partial class Vote
{
    public string VoteId { get; set; } = null!;

    public string? UserId { get; set; }

    public string? PostId { get; set; }

    public int? VoteType { get; set; }

    public virtual Post? Post { get; set; }

    public virtual User? User { get; set; }
}
