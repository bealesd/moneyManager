using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyApp.Interfaces
{
    public interface IAccount
    {
        string AccountName { get; set; }
        Guid AccountGuid { get; set; }
        Guid UserGuid { get; set; }
    }
}
