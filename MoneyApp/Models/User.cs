using MoneyApp.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoneyApp.Models
{
    public class User : IUser
    {
        public Guid UserGuid { get; set; }
        
        public string Username { get; set; }
        public List<Guid> AccountGuid { get; set; }
    }
}
