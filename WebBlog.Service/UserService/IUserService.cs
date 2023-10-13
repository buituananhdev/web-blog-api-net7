using WebBlog.Data.Models;

namespace WebBlog.Service.Services.UserService
{
    public interface IUserService
    {
        Task<List<User>> GetListUsers();
        Task<User?> GetInformationUser(string userID);
        Task<User> Register(User user);
        Task<User?> UpdateUser(string userID, string avatar_url, string full_name, string describe);
        Task<User?> ChangePassword(string email, string new_password);
        Task<User?> DeleteUser(string userID);
        Task<bool> IsEmailExists(string email);
        Task<User?> SendRequest(string userID);
    }
}
