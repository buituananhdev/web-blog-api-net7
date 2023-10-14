using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBlog.Data.DTOs
{
    public class TokenDTO
    {
        public string? Email { get; set; }

        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }

        public long? ExpirationTime { get; set; }

        public long? CreatedAt { get; set; }
    }
}
