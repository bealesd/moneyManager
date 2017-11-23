using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MoneyApp.Models;
using Newtonsoft.Json;

namespace MoneyApp.IO
{
    public class JsonReaderWriter : IReaderWriter
    {
        public IEnumerable<T> ReadEnumerable<T>(string path)
        {
            if (System.IO.File.Exists(path))
            {
                string jsonText = File.ReadAllText(path);
                IEnumerable<T> enumerable = JsonConvert.DeserializeObject<IEnumerable<T>>(jsonText);
                return enumerable;
            }
            return null;
        }

        public void WriteEnumerable<T>(string path, IEnumerable<T> enumerable)
        {
            string jsonUsers = JsonConvert.SerializeObject(enumerable.ToArray());

            System.IO.File.WriteAllText(path, jsonUsers);
        }
    }
}
