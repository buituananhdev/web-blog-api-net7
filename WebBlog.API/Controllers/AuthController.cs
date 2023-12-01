using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebBlog.API.Validators;
using WebBlog.Data.DTOs;
using WebBlog.Service.Services.AuthService;
using WebBlog.Service.Services.UserService;
using WebBlog.Utility.Languages;

namespace WebBlog.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IUserService _UserService;
        private readonly IAuthService _AuthService;

        public AuthController(DataContext dbContext, IConfiguration configuration, IUserService userService, IAuthService authService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _UserService = userService;
            _AuthService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO request)
        {

            bool isEmailExists = await _UserService.IsEmailExists(request.Email);

            if (!isEmailExists)
            {
                return BadRequest(GetText.GetCodeStatus("0001"));
            }

            var user = _dbContext.Users.SingleOrDefault(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest(GetText.GetCodeStatus("0002"));
            }

            if (user.IsActive == 0)
            {
                return BadRequest(GetText.GetCodeStatus("0003"));
            }
            TokenDTO token = await _AuthService.Login(user.UserId, user.Email);
            return Ok(token);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Get the token from the request header
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(GetText.GetCodeStatus("0004"));
            }

            // Validate the token and extract the username
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:SecretKey").Value!));
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                var email = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest(new { status = "failure", message = "Invalid email" });
                }

                // Remove the token and refresh token from the database
                var tokenEntity = _dbContext.Tokens.FirstOrDefault(t => t.Email == email && t.AccessToken == token);
                if (tokenEntity != null)
                {
                    _dbContext.Tokens.Remove(tokenEntity);
                    _dbContext.SaveChanges();
                }

                return Ok(new { status = "success", message = "Successfully logged out" });
            }
            catch (Exception)
            {
                return BadRequest(new { status = "failure", message = "Invalid token" });
            }
        }
    }
}
