using System;
using System.Collections.Generic;

namespace WebBlog.Data.Models;

public partial class Tag
{
    public string TagId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual List<Post> Posts { get; set; } = new List<Post>();
}
