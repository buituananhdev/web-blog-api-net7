using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebBlog.API.Validators;
using WebBlog.Data.DTOs;
using WebBlog.Service.Services.UserService;
using WebBlog.Utility.Languages;
using WebBlog.Utility.Utilities;
using WebBlog.Utility.Validators;
namespace WebBlog.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;

        public UserController(IUserService UserService) => _UserService = UserService;

        [HttpPost]
        // Hàm thêm user
        public async Task<ActionResult<List<UserDTO>>> Register(UserDTO user)
        {
            var validator = new UserValidator();
            var validationErrors = validator.Validate(user);

            if (validationErrors.Count > 0)
            {
                return BadRequest(new { status = "failure", errors = validationErrors });
            }
            bool isEmailExists = await _UserService.IsEmailExists(user.Email);

            if (isEmailExists)
            {
                return BadRequest(GetText.GetCodeStatus("0005"));
            }
            
            var result = await _UserService.Register(user);

            return result ? Ok(GetText.GetCodeStatus("S0001", "Registration")) : BadRequest(GetText.GetCodeStatus("F0006", "Registration"));
        }

        [HttpPatch("password/reset")]
        // Func change pasword
        public async Task<ActionResult<List<UserDTO>>> ChangPassword(ChangePasswordDTO body)
        {

            bool isEmailExists = await _UserService.IsEmailExists(body.Email);

            if (!isEmailExists)
            {
                return BadRequest(GetText.GetCodeStatus("F0001"));
            }

            var validator = new UserValidator();
            var validationErrors = validator.Validate_Password(body.Password);

            if (validationErrors != "")
            {
                return BadRequest(new { status = "failure", errors = validationErrors });
            }

            if(body.Password != body.ConfirmPassword)
            {
                return BadRequest(GetText.GetCodeStatus("F0009"));
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(body.Password);
            await _UserService.ChangePassword(body.Email, hashedPassword);
            return Ok(GetText.GetCodeStatus("S0001", "Change password"));
        }
        

        [HttpPatch("update/{id}")]
        // Func update user information
        public async Task<ActionResult<List<User>>> UpdateUser(string id, string avatar_url, string full_name, string describe)
        {
            if (full_name.IsNullOrEmpty())
            {
                return BadRequest(GetText.GetCodeStatus("F0008", "Fullname"));
            }
            var result = await _UserService.UpdateUser(id, avatar_url, full_name, describe);
            return result ? StatusCode(204, GetText.GetCodeStatus("S0001", "Update user")) : BadRequest(GetText.GetCodeStatus("F0007", "User"));
        }

        [HttpPatch("send_request/{id}")]
        // Func active / Deactive user
        public async Task<ActionResult<List<User>>> SendRequest(string id)
        {
            var result = await _UserService.SendRequest(id);
            if (result is null)
            {
                return BadRequest(GetText.GetCodeStatus("F0007", "User"));
            }
            if(result.IsActive == 1)
            {
                return Ok(GetText.GetCodeStatus("S0001", "Active user"));
            } 
            else
            {
                return Ok(GetText.GetCodeStatus("S0001", "Deactive user"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<UserDTO>>> GetInformationUser(string id)
        {
            var result = await _UserService.GetInformationUser(id);
            if (result is null)
            {
                return BadRequest(GetText.GetCodeStatus("F0007", "User"));
            }

            return Ok(new { status = "success", data = result });
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            var result = await _UserService.GetListUsers();
            if (result is null)
            {
                return BadRequest(GetText.GetCodeStatus("F0007", "User"));
            }

            return Ok(new { status = "success", data = result });
        }
    }
}