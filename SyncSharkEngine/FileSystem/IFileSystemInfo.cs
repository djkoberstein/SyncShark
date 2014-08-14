using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.FileSystem
{
    public interface IFileSystemInfo
    {
        string FullName { get; }
        DateTime LastWriteTimeUtc { get; }
    }
}
