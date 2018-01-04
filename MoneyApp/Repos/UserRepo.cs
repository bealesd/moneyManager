using System;
using System.Collections.Generic;
using System.Linq;
using MoneyApp.Helper;
using MoneyApp.Interfaces;
using MoneyApp.IO;
using MoneyApp.Models;

namespace MoneyApp.Repos
{
    public class UserRepo : IUserRepo
    {
        private List<User> _users = new List<User>();
        private IReaderWriter _readerWriter;
        private string _filePath;

        public UserRepo(IReaderWriter readerWriter, string filePath)
        {
            _readerWriter = readerWriter;
            _filePath = filePath;
            Load();
        }

        public void Save()
        {
            _readerWriter.WriteEnumerable(_filePath, _users);
        }

        public void Load()
        {
            var loadedUsers = _readerWriter.ReadEnumerable<User>(_filePath);
            if (loadedUsers != null)
                _users = loadedUsers.ToList();
        }

        public void CreateUser(string username, Guid userGuid)
        {
            if (_users.Exists(u => String.Equals(u.Username, username, StringComparison.InvariantCultureIgnoreCase) || !username.ValidUsername()))
                throw new Exception();

            var newUser = new User()
            {
                UserGuid = userGuid,
                Username = username,
                AccountGuid = new List<Guid>()
            };
            _users.Add(newUser);
            Save();
        }

        public void AddAccountToUser(Guid userGuid, Guid accountGuid)
        {
            var user = _users.FirstOrDefault(u => u.UserGuid == userGuid);
            if (user == null)
                throw new Exception();
            user.AccountGuid.Add(accountGuid);
            Save();
        }

        public IEnumerable<IUser> GetAllUsers()
        {
            return _users;
        }

        public User GetUser(Guid userGuid)
        {
            User user = _users.FirstOrDefault(u => u.UserGuid == userGuid);
            return Equals(user, null) ? throw new Exception() : user;
        }

        public void DeleteUser(Guid userGuid)
        {
            _users.Remove(this.GetUser(userGuid));
            Save();
        }

        public void RemoveAccount(Guid userGuid, Guid accountGuid)
        {
            this.GetUser(userGuid).AccountGuid.Remove(accountGuid);
            Save();
        }
    }
}
