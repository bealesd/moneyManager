using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MoneyApp.Interfaces;

namespace MoneyApp.Credentials
{
    public class SessionHandler : ISessionHandler
    {
        private HttpContext _context;

            public void UpdateSessionHandler(HttpContext context)
            {
                _context = context;
            }

            public void SetSessionString(string key, string value)
            {
                _context.Session.SetString(key, value);
            }

            public string GetSessionString(string key)
            {
                return _context.Session.GetString(key);
            }
    }
}
