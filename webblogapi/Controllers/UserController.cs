using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using webblogapi.DTOs;
using webblogapi.Models;
using webblogapi.Services.UserService;
using webblogapi.Validators;

namespace webblogapi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;

        public UserController(IUserService UserService) => _UserService = UserService;

        [HttpPost]
        // Hàm thêm user
        public async Task<ActionResult<List<UserDTO>>> Register(User user)
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
                return BadRequest(new {status = "failure", message = "Email đã tồn tại trong hệ thống" });
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = hashedPassword;
            user.Avatar = "";
            user.UserId = UUID.Generate();
            user.IsActive = 1;
            var result = await _UserService.Register(user);
            var userDTO = new UserDTO
            {
                UserId = result.UserId,
                Fullname = result.Fullname,
                Describe = result.Describe,
                Email = result.Email,
                Password = result.Password,
                Avatar = result.Avatar,
                IsActive = result.IsActive

            };
            return Ok(new { status = "success", data = userDTO });
        }

        [HttpPatch("password/reset")]
        // Func change pasword
        public async Task<ActionResult<List<UserDTO>>> ChangPassword(ChangePasswordDTO body)
        {

            bool isEmailExists = await _UserService.IsEmailExists(body.Email);

            if (!isEmailExists)
            {
                return BadRequest(new { status = "failure", message = "Email không tồn tại trong hệ thống" });
            }

            var validator = new UserValidator();
            var validationErrors = validator.Validate_Password(body.Password);

            if (validationErrors != "")
            {
                return BadRequest(new { status = "failure", errors = validationErrors });
            }

            if(body.Password != body.ConfirmPassword)
            {
                return BadRequest(new { status = "failure", error = "Password not match!" });
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(body.Password);
            await _UserService.ChangePassword(body.Email, hashedPassword);
            return Ok(new { status = "success", message = "Change password successfully!" });
        }
        

        [HttpPatch("update/{id}")]
        // Func update user information
        public async Task<ActionResult<List<User>>> UpdateUser(string id, string avatar_url, string full_name, string describe)
        {
            if (avatar_url.IsNullOrEmpty())
            {
                return BadRequest(new { status = "failure", error = "Avatar URL is required!" });
            }
            if (full_name.IsNullOrEmpty())
            {
                return BadRequest(new { status = "failure", error = "Fullname is required!" });
            }
            var result = await _UserService.UpdateUser(id, avatar_url, full_name, describe);
            if (result is null)
            {
                return BadRequest(new { status = "failure", message = "User not found" });
            }
            return StatusCode(204, new { status = "success", message = "Update user information successfully" });
        }

        [HttpPatch("send_request/{id}")]
        // Func active / Deactive user
        public async Task<ActionResult<List<User>>> SendRequest(string id)
        {
            var result = await _UserService.SendRequest(id);
            if (result is null)
            {
                return BadRequest(new { status = "failure", message = "User not found" });
            }
            string message = "";
            if(result.IsActive == 1)
            {
                message = "Active user successfully";
            } 
            else
            {
                message = "Deactive user successfully";
            }
            return Ok(new { status = "success", message = message });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<UserDTO>>> GetInformationUser(string id)
        {
            var result = await _UserService.GetInformationUser(id);
            if (result is null)
            {
                return BadRequest(new { status = "failure", message = "User not found" });
            }
            var userDTO = new UserDTO
            {
                UserId = result.UserId,
                Fullname = result.Fullname,
                Describe = result.Describe,
                Email = result.Email,
                Password = result.Password,
                Avatar = result.Avatar,
                IsActive = result.IsActive

            };
            return Ok(new { status = "success", data = userDTO });
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            var result = await _UserService.GetListUsers();
            if (result is null)
            {
                return BadRequest(new { status = "failure", message = "User not found" });
            }

            var userDTOs = result.Select(user => new UserDTO
            {
                UserId = user.UserId,
                Fullname = user.Fullname,
                Describe = user.Describe,
                Email = user.Email,
                Password = user.Password,
                Avatar = user.Avatar,
                IsActive = user.IsActive
            }).ToList();

            return Ok(new { status = "success", data = userDTOs });
        }
    }
}