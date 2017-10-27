using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MoneyApp.Models;
using Newtonsoft.Json;

namespace MoneyApp.Repos
{
    public class UserRepo : IUserRepo
    {
        private List<User> _users = new List<User>();
        private string usersPath = @"C:\Users\dave\Desktop\Users.txt";

        public UserRepo()
        {
            Load();
        }
        public void Save()
        {
            string jsonUsers = JsonConvert.SerializeObject(_users.ToArray());
            System.IO.File.WriteAllText(usersPath, jsonUsers);
        }
        public void Load()
        {
            if (System.IO.File.Exists(usersPath))
            {
                string jsonUsers = File.ReadAllText(usersPath);
                _users = JsonConvert.DeserializeObject<List<User>>(jsonUsers);
            }
        }
        public void AddAccountToUser(Guid userGuid, Guid accountGuid)
        {
            throw new NotImplementedException();
        }

        public void AddUser(string username)
        {
            var newUser = new User()
            {
                UserGuid = Guid.NewGuid(),
                Username = username,
                AccountGuid = new List<Guid>()
            };
            _users.Add(newUser);

            Save();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _users;
        }

        public User GetUser(Guid userGuid)
        {
            User user = _users.FirstOrDefault(u => u.UserGuid == userGuid);
            return user;
        }
    }
}
