using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebBlog.Data.Data;
using WebBlog.Data.DTOs;
using WebBlog.Data.Models;
using WebBlog.Utility.Utilities;

namespace WebBlog.Service.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(DataContext context, IConfiguration configuration, IMapper mapper, ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<TokenDTO> Login(string id, string email)
        {
            try
            {
                TokenDTO tokenDTO = JwtTokenHelper.GenerateAccessToken(id, _configuration);
                tokenDTO.Email = email;
                Token token = _mapper.Map<Token>(tokenDTO);
                token.Id = UUID.Generate();
                _context.Add(token);
                await _context.SaveChangesAsync();
                return tokenDTO;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Login error");
                throw;
            }
        }
    }
}
