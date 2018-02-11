using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyApp.Interfaces
{
    public interface IFilePaths
    {
        string UserPath();
        string AccountPath();
        string CredentialsPath();
        List<string> GetFilePaths();
    }
}
