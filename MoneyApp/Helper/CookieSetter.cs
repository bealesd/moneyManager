using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MoneyApp.Helper
{
    public static class CookieSetter
    {
        public static string Test(IHttpContextAccessor httpContextAccessor, string key, string value)
        {
            httpContextAccessor.HttpContext.Response.Cookies.Append(key, value);
            return "";
        }
    }
}
