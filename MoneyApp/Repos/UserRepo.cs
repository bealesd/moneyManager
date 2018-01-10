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

        public void CreateUser(string username)
        {
            var newUser = new User()
            {
                Username = username,
                AccountGuid = new List<Guid>()
            };
            _users.Add(newUser);
            Save();
        }

        public void AddAccountToUser(string username, Guid accountGuid)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            if (user == null)
                throw new Exception();
            user.AccountGuid.Add(accountGuid);
            Save();
        }

        public IEnumerable<IUser> GetAllUsers()
        {
            return _users;
        }

        public User GetUser(string username)
        {
            User user = _users.FirstOrDefault(u => u.Username == username);
            return Equals(user, null) ? throw new Exception() : user;
        }

        public void DeleteUser(string username)
        {
            _users.Remove(this.GetUser(username));
            Save();
        }

        public void RemoveAccount(string username, Guid accountGuid)
        {
            this.GetUser(username).AccountGuid.Remove(accountGuid);
            Save();
        }
    }
}