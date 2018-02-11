using System;

namespace MoneyApp.Models
{
    public class UserCredentials
    {
        public Guid UserGuid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}