using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public void AddUser(string username)
        {
            var newUser = new User()
            {
                UserGuid = Guid.NewGuid(),
                Username = username,
                //AccountGuid = new List<Guid>()
            };
            _users.Add(newUser);

            Save();
        }

        public IEnumerable<IUser> GetAllUsers()
        {
            return _users;
        }

        public Guid GetUser(string username)
        {
            User user = _users.FirstOrDefault(u => String.Equals(u.Username, username,
                                                    StringComparison.InvariantCultureIgnoreCase));
            
            // could make a readonly User Object
            return user?.UserGuid ?? Guid.Empty;
        }
    }
}
