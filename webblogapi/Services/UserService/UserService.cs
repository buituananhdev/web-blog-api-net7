using Microsoft.EntityFrameworkCore;
namespace webblogapi.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        public UserService(DataContext context)
        {
            _context = context;
        }
        public async Task<User?> DeleteUser(string userID)
        {
            var user = await _context.Users.FindAsync(userID);
            if(user is null)
            {
                return null;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetInformationUser(string userID)
        {
            var user = await _context.Users.FindAsync(userID);
            if(user is null)
            {
                return null;
            }
            return user;
        }

        public async Task<List<User>> GetListUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<bool> IsEmailExists(string email)
        {
            bool emailExists = await _context.Users.AnyAsync(u => u.Email == email);
            return emailExists;
        }

        public async Task<User> Register(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateUser(string userID, string avatar_url, string full_name, string describe)
        {
            var user = await _context.Users.FindAsync(userID);
            if(user is null)
            {
                return null;
            }

            user.Fullname = full_name;
            user.Avatar = avatar_url;
            user.Describe = describe;

            await _context.SaveChangesAsync();

            return user;
        }
        public async Task<User?> ChangePassword(string email, string new_password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Email == email);
            if (user is null)
            {
                return null;
            }

            user.Password = new_password;

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> SendRequest(string userID)
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
    }
}
