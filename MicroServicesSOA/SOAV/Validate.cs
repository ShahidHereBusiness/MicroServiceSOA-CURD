using System.Text.RegularExpressions;

namespace MicroServicesSOA.SOAV
{
    internal static class Validate
    {
        #region Instance User
        internal static bool EmailError(string mail, ref string msg)
        {
            if (string.IsNullOrEmpty(mail))
            {
                msg = "Email can't be empty";
                return true;
            }

            msg = "Valid Email format desired";
            mail = mail.Trim().ToUpper();
            // Define a regular expression pattern for basic mail validation
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Use Regex.IsMatch to check if the mail matches the pattern
            if (!Regex.IsMatch(mail, pattern))
                return true;

            return false;
        }
        internal static bool PasswordError(string watchWord, ref string msg)
        {
            if (string.IsNullOrEmpty(watchWord))
            {
                msg = "Password can't be empty";
                return true;
            }
            msg = "Policy Validated Password desired";
            // Minimum and maximum length requirements
            int minLength = 8;
            int maxLength = 20;

            // Check if the watchWord meets the length requirements
            if (watchWord.Length < minLength || watchWord.Length > maxLength)
                return true;

            // Check for the use of uppercase letters
            if (!watchWord.Any(char.IsUpper))
                return true;

            // Check for the use of lowercase letters
            if (!watchWord.Any(char.IsLower))
                return true;

            // Check for the use of numbers
            if (!watchWord.Any(char.IsDigit))
                return true;

            // Check for the use of special characters
            string specialCharacters = @"!@#$%^&*()_+";
            if (!watchWord.Intersect(specialCharacters).Any())
                return true;

            // Check for common patterns or easily guessable information (e.g., "watchWord")
            if (Regex.IsMatch(watchWord, @"\b(?:watchWord)\b", RegexOptions.IgnoreCase))
                return true;

            // If all checks pass, the watchWord is considered valid
            msg = string.Empty;
            return false;
        }
        internal static bool PhoneNumberError(string number, ref string msg)
        {
            if (string.IsNullOrEmpty(number))
            {
                msg = "Phone Number can't be empty";
                return true;
            }
            msg = "Phone Number format +1-923335555555, Country code with 1 to 4 digits, followed by 7 to 14 digits desired";
            // This example assumes a country code with 1 to 4 digits, followed by 7 to 14 digits for the actual phone number
            string pattern = @"^\+|0\d{1,4}-\d{7,14}$";

            // Create a Regex object
            Regex regex = new Regex(pattern);

            return !regex.IsMatch(number);
        }
        internal static bool UserNameError(string userName, ref string msg)
        {
            // Check if the username is not null or empty
            if (string.IsNullOrEmpty(userName))
            {
                msg = "Username cannot be null or empty.";
                return true;
            }

            // Check if the username meets a minimum length requirement
            if (userName.Length < 3)
            {
                msg = "Username must be at least 3 characters long.";
                return true;
            }

            // Check if the username contains only alphanumeric characters
            foreach (char c in userName)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    msg = "Username can only contain alphanumeric characters.";
                    return true;
                }
            }

            return false;
        }
        internal static bool UserIdError(string id, ref string msg)
        {
            // Check if the username is not null or empty
            if (string.IsNullOrEmpty(id))
            {
                msg = "User Surrogate cannot be null or empty.";
                return true;
            }
            return false;
        }
        #endregion
        
        #region Instance Role
        internal static bool RoleNameError(string role, ref string msg)
        {
            if (string.IsNullOrEmpty(role))
                return true;           
            role = role.Trim().ToUpper();
            string pattern = @"^[a-zA-Z0-9-]+$";
            if (!Regex.IsMatch(role, pattern))
                return true;
            return false;
        }
        internal static bool RoleIdError(string id, ref string msg)
        {
            // Check if the username is not null or empty
            if (string.IsNullOrEmpty(id))
            {
                msg = "Role Surrogate cannot be null or empty.";
                return true;
            }
            return false;
        }
        #endregion
    }
}