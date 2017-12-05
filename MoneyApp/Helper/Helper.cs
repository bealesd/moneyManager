using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Data.OData;

namespace MoneyApp.Helper
{
    public class Helper
    {
        public string TempPath(string fileName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), fileName);
        }
    }

    public static class ValidationHelpers
    {
        public static bool ValidUsername(this string username)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9_-]{7,15}$");
            return regex.IsMatch(username);
        }
    }
}
