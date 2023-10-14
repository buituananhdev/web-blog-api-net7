using WebBlog.Data.DTOs;
using WebBlog.Data.Models;

namespace WebBlog.Service.Services.UserService
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetListUsers();
        Task<UserDTO?> GetInformationUser(string userID);
        Task<bool> Register(UserDTO user);
        Task<bool> UpdateUser(string userID, string avatar_url, string full_name, string describe);
        Task<bool> ChangePassword(string email, string new_password);
        Task<bool> DeleteUser(string userID);
        Task<bool> IsEmailExists(string email);
        Task<User> SendRequest(string userID);
    }
}
