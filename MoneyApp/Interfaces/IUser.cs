using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyApp.Interfaces
{
    public interface IUser
    {
        Guid UserGuid { get; set; }
        string Username { get; set; }
        List<Guid> AccountGuid { get; set; }
    }
}
