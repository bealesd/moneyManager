using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MoneyApp.Helper;
using MoneyApp.Interfaces;
using MoneyApp.IO;
using MoneyApp.Models;

namespace MoneyApp.Repos
{
    public class UserLoginRepo : IUserLogin
    {
        private IReaderWriter _readerWriter;
        private string _filePath;
        private List<UserCredentials> _userCredentials = new List<UserCredentials>();

        public UserLoginRepo(IReaderWriter readerWriter, string filePath)
        {
            _readerWriter = readerWriter;
            _filePath = filePath;
            Load();
        }
        public void Save()
        {
            _readerWriter.WriteEnumerable(_filePath, _userCredentials);
        }

        public void Load()
        {
            var loadedUsers = _readerWriter.ReadEnumerable<UserCredentials>(_filePath);
            if (loadedUsers != null)
                _userCredentials = loadedUsers.ToList();
        }
        public void CreateUser(string username, string password)
        {
            if (_userCredentials.Exists(u => String.Equals(u.Username, username, StringComparison.InvariantCultureIgnoreCase) || !username.ValidUsername()))
                throw new Exception("Username taken.");

            var userGuid = Guid.NewGuid();

            //create the salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            // concat password to the salt and hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string savedPassword = Convert.ToBase64String(hashBytes);

            var newUserCredentials = new UserCredentials() { Username = username, UserGuid = userGuid, Password = savedPassword };
            _userCredentials.Add(newUserCredentials);
            Save();
        }

        public Guid GetUserGuid(string username, string password)
        {
            var userCredentials = _userCredentials.FirstOrDefault(u => u.Username == username);
            string passwordHash = userCredentials.Password;

            byte[] hashBytes = Convert.FromBase64String(passwordHash);
            byte[] salt = new byte[16];
            //Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            bool isValidPassword = true;
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    isValidPassword = false;
            }

            if (isValidPassword)
                return userCredentials.UserGuid;
            return Guid.Empty;
        }

        public void DeleteUser(Guid userGuid)
        {
            var userCredentials = _userCredentials.FirstOrDefault(u => u.UserGuid == userGuid);
            if (userCredentials != null)
            {
                _userCredentials.Remove(userCredentials);
            }
            Save();
        }
    }
}
