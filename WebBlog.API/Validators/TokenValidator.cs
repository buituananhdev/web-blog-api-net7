using System.Text.RegularExpressions;
using WebBlog.Data.Models;

namespace WebBlog.API.Validators
{
    public class TokenValidator
    {
        public List<string> Validate(string token)
        {
            var errors = new List<string>();

            if (IsValidToken(token))
            {
                errors.Add("Invalid token");
            }

            return errors;
        }

        private static bool IsValidToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return true; // Email is optional
            }

            var regex = new Regex(@"^[A-Za-z0-9-_]+\.[A-Za-z0-9-_]+\.[A-Za-z0-9-_]+$");
            return regex.IsMatch(token);
        }
    }
}
