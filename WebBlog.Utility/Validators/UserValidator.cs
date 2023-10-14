using System.Text.RegularExpressions;
using WebBlog.Data.DTOs;

namespace WebBlog.Utility.Validators
{
    public class UserValidator
    {
        public List<string> Validate(UserDTO user)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(user.Fullname))
            {
                errors.Add("'FullName' is required.");
            }
            else if (user.Fullname.Length > 50)
            {
                errors.Add("'FullName' cannot exceed 50 characters.");
            }

            if (!IsValidEmail(user.Email))
            {
                errors.Add("Invalid email.");
            }

            if (!IsValidPassword(user.Password))
            {
                errors.Add("Invalid password.");
            }

            return errors;
        }

        public string Validate_Password(string password)
        {
            string error = "";
            if (string.IsNullOrEmpty(password))
            {
                error = "Invalid password.";
            }

            return error;
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return true; // Email is optional
            }

            var regex = new Regex(@"^[a-zA-Z0-9+._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return regex.IsMatch(email);
        }

        private static bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return true; // Password is optional
            }

            var regex = new Regex(@"^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]{8,32}$");
            return regex.IsMatch(password);
        }
    }

}