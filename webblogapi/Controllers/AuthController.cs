using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webblogapi.Helpers;
using webblogapi.Services.UserService;
using webblogapi.Validators;

namespace webblogapi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IUserService _UserService;
        public AuthController(DataContext dbContext, IConfiguration configuration, IUserService userService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _UserService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] User request)
        {
            bool isEmailExists = await _UserService.IsEmailExists(request.Email);

            if (!isEmailExists)
            {
                return BadRequest(new { status = "failure", message = "Email không tồn tại trong hệ thống" });
            }

            var validator = new LoginValidator();
            var validationErrors = validator.Validate(request);

            if (validationErrors.Count > 0)
            {
                return BadRequest(new { status = "failure", errors = validationErrors });
            }

            // Kiểm tra thông tin đăng nhập của người dùng từ database
            var user = _dbContext.Users.SingleOrDefault(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest(new { status = "failure", message = "Incorrect email or password!" });
            }

            if(user.IsActive == 0)
            {
                return BadRequest(new { status = "failure", message = "Tài khoản đã bị xóa, hãy liên hệ admin để được khôi phục lại!" });
            }

            // Tạo token và refresh token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:SecretKey").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserID", user.UserId),
                    new Claim(ClaimTypes.Name, user.Fullname),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };
            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = Guid.NewGuid().ToString();

            // Lưu token và refresh token vào cơ sở dữ liệu
            DateTimeOffset expiresAt = DateTimeOffset.UtcNow.AddDays(1);
            DateTime dateTime = DateTime.Now;
            var tokenEntity = new Token
            {
                Id = UUID.Generate(),
                Email = user.Email,
                AccessToken = tokenHandler.WriteToken(jwtToken),
                RefreshToken = refreshToken,
                ExpirationTime = expiresAt.ToUnixTimeSeconds(),
                CreatedAt = DateTimeHelper.ConvertToUnixTimeSeconds(dateTime)
        };
            _dbContext.Tokens.Add(tokenEntity);
            _dbContext.SaveChanges();

            // Trả về token và refresh token cho client

            return Ok(new
            {
                status = "success",
                data = new
                {
                    access_token = tokenHandler.WriteToken(jwtToken),
                    refresh_token = refreshToken,
                    expires_in = tokenEntity.ExpirationTime,
                    created_at = tokenEntity.CreatedAt,
                }
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Get the token from the request header
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { status = "failure", message = "Invalid token" });
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
