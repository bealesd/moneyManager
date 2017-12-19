using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.OData;

namespace MoneyApp.Helper
{
    public class PathHelper
    {
        public string TempPath(string fileName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), fileName);
        }
    }
}
