using System.Collections.Generic;
using System.IO;
using MoneyApp.Interfaces;

namespace MoneyApp.Helper
{
    public class TestFilePaths: IFilePaths
    {
        private static string TempPath(string fileName) => Path.Combine(Directory.GetCurrentDirectory(), fileName);
        public string UserPath() => TempPath("testUsers.txt");
        public string AccountPath() => TempPath("testAccount.txt");
        public string CredentialsPath() => TempPath("testUserCredentials.txt");
        public List<string> GetFilePaths() => new List<string>() { UserPath(), AccountPath(), CredentialsPath() };
    }

    public static class FileCleaner
    {
        public static void WipeFiles(this IFilePaths filePaths)
        {
            filePaths.GetFilePaths().ForEach(f =>
            {
                if (File.Exists(f))
                    File.Delete(f);
            });
        }
    }
}