using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebBlog.Data.DTOs;
using WebBlog.Utility.Helpers;

namespace WebBlog.Utility.Utilities
{
    public class JwtTokenHelper
    {

        private static SymmetricSecurityKey GetSymmetricSecurityKey(string secretKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        }

        private static TokenDTO GenerateToken(string userId, string secretKey, double expirationDays)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = GetSymmetricSecurityKey(secretKey);

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("code", userId),
                }),
                Expires = DateTime.UtcNow.AddDays(expirationDays),
                SigningCredentials = creds
            };

            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);

            var expiresIn = DateTimeOffset.UtcNow.AddDays(expirationDays);
            var dateTime = DateTime.Now;

            var token = new TokenDTO();
            token.AccessToken = tokenHandler.WriteToken(jwtToken);
            token.ExpirationTime = expiresIn.ToUnixTimeSeconds();
            token.CreatedAt = DateTimeHelper.ConvertToUnixTimeSeconds(dateTime);
            return token;
        }

        public static TokenDTO GenerateAccessToken(string userId, IConfiguration configuration)
        {
            var secretKey = configuration.GetSection("AppSettings:AccessTokenSecretKey").Value;
            return GenerateToken(userId, secretKey, 1);
        }

        public static TokenDTO GenerateRefreshToken(string userId, IConfiguration configuration)
        {
            var secretKey = configuration.GetSection("AppSettings:RefreshTokenSecretKey").Value;
            return GenerateToken(userId, secretKey, 7);
        }
    }
}
