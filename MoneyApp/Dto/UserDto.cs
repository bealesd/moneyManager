using System;
using System.Collections.Generic;
using MoneyApp.Models;

namespace MoneyApp.Dto
{
    public class UserDto
    {
        public string Username { get; set; }
        public List<Account> Accounts { get; set; }
    }
}
