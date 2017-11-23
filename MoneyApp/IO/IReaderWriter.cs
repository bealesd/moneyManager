using System.Collections.Generic;

namespace MoneyApp.IO
{
    public interface IReaderWriter
    {
        IEnumerable<T> ReadEnumerable<T>(string path);
        void WriteEnumerable<T>(string path, IEnumerable<T> enumerable);

    }
}