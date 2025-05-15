using System.Text.RegularExpressions;

namespace rethus_backend.Utilities.Security.Hashing
{
    public class Password
    {
        private static readonly Regex PasswordRegex = new Regex(
            @"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{6,}$"
        );

        public static bool ValidatePassword(string password, string confirmedPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmedPassword))
            {
                return false; // Passwords cannot be null or empty
            }

            if (!PasswordRegex.IsMatch(password))
            {
                return false; // Password does not meet the security requirements
            }

            return password == confirmedPassword; // Check if passwords match
        }
    }
}
