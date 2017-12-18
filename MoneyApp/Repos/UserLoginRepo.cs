using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public void CreateUser(string username, Guid userGuid)
        {
            var newUserCredentials = new UserCredentials(){Username = username, UserGuid = userGuid};
            _userCredentials.Add(newUserCredentials);
            Save();
        }

        public Guid GetUserGuid(string username)
        {
            var userCredentials = _userCredentials.FirstOrDefault(u => u.Username == username);
            if (userCredentials == null)
                return Guid.Empty;
            return userCredentials.UserGuid;
        }
    }
}
