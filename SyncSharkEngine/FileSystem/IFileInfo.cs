using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.DataModel
{
    public interface IFileInfo
    {
        string FullName { get; }
        DateTime LastWriteTimeUtc { get; }
        Stream OpenRead();
        Stream OpenWrite();
        void Delete();
    }
}
