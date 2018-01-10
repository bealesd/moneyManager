using System;
using System.Collections.Generic;

namespace MoneyApp.Interfaces
{
    public interface IUser
    {
        string Username { get; set; }
        List<Guid> AccountGuid { get; set; }
    }
}