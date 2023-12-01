using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Data.DTOs;

namespace WebBlog.Service.Services.AuthService
{
    public interface IAuthService
    {
        Task<TokenDTO> Login(string userId, string email);
    }
}
