using System.Collections.Generic;
using System.IO;
using MoneyApp.Interfaces;

namespace MoneyApp.Helper
{
    public class StandardFilePaths: IFilePaths
    {
        private static string TempPath(string fileName) => Path.Combine(Directory.GetCurrentDirectory(), fileName);
        public string UserPath() => TempPath("users.txt");
        public string AccountPath() => TempPath("account.txt");
        public string CredentialsPath() => TempPath("userCredentials.txt");
        public List<string> GetFilePaths() => new List<string>() { UserPath(), AccountPath(), CredentialsPath() };
    }
}