using MoneyApp.Interfaces;
using System;
using System.Collections.Generic;

namespace MoneyApp.Models
{
    public class User : IUser
    {
        public string Username { get; set; }
        public List<Guid> AccountGuid { get; set; }
    }
}