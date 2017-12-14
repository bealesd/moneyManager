using System.Text.RegularExpressions;

namespace MoneyApp.Helper
{
    public static class ValidationHelpers
    {
        public static bool ValidUsername(this string username)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9_-]{7,15}$");
            return regex.IsMatch(username);
        }
    }
}