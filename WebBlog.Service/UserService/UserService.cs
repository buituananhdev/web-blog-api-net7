using Microsoft.EntityFrameworkCore;
using WebBlog.Data.Models;
using WebBlog.Data.Data;
using WebBlog.Data.DTOs;
using AutoMapper;
using WebBlog.Utility.Utilities;
using Microsoft.Extensions.Logging;

namespace WebBlog.Service.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(DataContext context, IMapper mapper, ILogger<UserService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<bool> DeleteUser(string userID)
        {
            try
            {
                var user = await _context.Users.FindAsync(userID);
                if (user is null)
                {
                    return false;
                }
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete user error");
                throw;
            }
        }

        public async Task<UserDTO?> GetInformationUser(string userID)
        {
            try
            {
                var user = await _context.Users.FindAsync(userID);
                if (user is null)
                {
                    return null;
                }
                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get information user error");
                throw;
            }
        }

        public async Task<List<UserDTO>> GetListUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return _mapper.Map<List<UserDTO>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get list user error");
                throw;
            }
        }

        public async Task<bool> IsEmailExists(string email)
        {
            try
            {
                bool emailExists = await _context.Users.AnyAsync(u => u.Email == email);
                return emailExists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "IsEmailExists error");
                throw;
            }
        }

        public async Task<bool> Register(UserDTO userDTO)
        {
            try
            {
                User user = _mapper.Map<User>(userDTO);
                user.UserId = UUID.Generate();
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Password = hashedPassword;
                user.Avatar = "";
                user.IsActive = 1;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                if (await IsEmailExists(user.Email))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Register error");
                throw;
            }
        }

        public async Task<bool> UpdateUser(string userID, string avatar_url, string full_name, string describe)
        {
            try
            {
                var user = await _context.Users.FindAsync(userID);
                if (user is null)
                {
                    return false;
                }

                user.Fullname = full_name;
                user.Avatar = avatar_url;
                user.Describe = describe;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update user error");
                throw;
            }
        }
        public async Task<bool> ChangePassword(string email, string new_password)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(m => m.Email == email);
                if (user is null)
                {
                    return false;
                }

                user.Password = new_password;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Change password error");
                throw;
            }
        }

        public async Task<User> updateUserStatus(string userID)
        {
            try
            {
                var user = await _context.Users.FindAsync(userID);
                if (user is null)
                {
                    return null;
                }
                if (user.IsActive == 0)
                {
                    user.IsActive = 1;
                }
                else
                {
                    user.IsActive = 0;
                }

                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update user status error");
                throw;
            }
        }
    }
}
