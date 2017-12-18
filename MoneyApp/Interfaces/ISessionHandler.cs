using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MoneyApp.Interfaces
{
    public interface ISessionHandler
    {
        void UpdateSessionHandler(HttpContext context);
        void SetSessionString(string key, string value);
        string GetSessionString(string key);

    }
}
