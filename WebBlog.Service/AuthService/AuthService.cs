using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebBlog.Data.Data;
using WebBlog.Data.DTOs;
using WebBlog.Utility.Utilities;

namespace WebBlog.Service.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<TokenDTO> Login(LoginDTO request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return null;
            }

            TokenDTO token = JwtTokenHelper.GenerateAccessToken(user.UserId, _configuration);
            return token;
        }
    }
}
