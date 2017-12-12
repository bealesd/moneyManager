using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MoneyApp.Helper;
using MoneyApp.Interfaces;
using MoneyApp.IO;
using MoneyApp.Models;
using Newtonsoft.Json;

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

        public bool AddUser(string username)
        {
            if (_users.Exists(u => u.Username == username) || !username.ValidUsername())
            {
                return false;
            }

            var newUser = new User()
            {
                UserGuid = Guid.NewGuid(),
                Username = username,
                AccountGuid = new List<Guid>()
            };
            _users.Add(newUser);
            Save();
            return true;
        }

        public void AddAccount(string username, Guid accountGuid)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return;
            }
            user.AccountGuid.Add(accountGuid);
            Save();
        }

        public IEnumerable<IUser> GetAllUsers()
        {
            return _users;
        }

        public User GetUser(string username)
        {
            User user = _users.FirstOrDefault(u => String.Equals(u.Username, username,
                                                    StringComparison.InvariantCultureIgnoreCase));
            return user;
        }

        public bool DeleteUser(string username)
        {
            var user = this.GetUser(username);
            if (user.Equals(null))
                return false;
            _users.Remove(user);
            Save();
            return true;
        }

        public bool DeleteAccount(Guid accountGuid, string username)
        {
            var user = this.GetUser(username);
            if (user.Equals(null))
                return false;
            if (user.AccountGuid.FirstOrDefault(g => g == accountGuid) == Guid.Empty)
            {
                return false;
            }
            
            user.AccountGuid.Remove(accountGuid);
            return true;
        }
    }
}
