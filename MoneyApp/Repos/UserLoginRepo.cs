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
        //Remove passwords from each view and hide url data.
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
            var userGuid = Guid.NewGuid();
            var newUserCredentials = new UserCredentials(){Username = username, UserGuid = userGuid, Password = password};
            _userCredentials.Add(newUserCredentials);
            Save();
        }

        public Guid GetUserGuid(string username, string password)
        {
            var userCredentials = _userCredentials.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (userCredentials == null)
                return Guid.Empty;
            return userCredentials.UserGuid;
        }

        public void DeleteUser(Guid userGuid)//string username, string password
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
