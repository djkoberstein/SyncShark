using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.FileSystem
{
    public interface IFileSystemInfoFactory
    {
        IFileSystemInfo GetFileSystemInfo(string path, bool isDirectory);
    }
}
