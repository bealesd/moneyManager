﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyApp.Models;

namespace MoneyApp.Repos
{
    public interface IUserRepo
    {
        IEnumerable<User> GetAllUsers();
        User GetUser(string username);
        void AddUser(string username);
        //void AddAccountToUser(Guid userGuid, Guid accountGuid);
        void Save();
        void Load();
    }
}
