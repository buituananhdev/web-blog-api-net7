using System.Text.RegularExpressions;

namespace webblogapi.Validators
{
    public class LoginValidator
    {
        public List<string> Validate(User user)
        {
            var errors = new List<string>();

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
