using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.FileSystem
{
    public interface IDirectoryInfo : IFileSystemInfo
    {
        IEnumerable<IFileSystemInfo> GetFileSystemInfos();
        void Create();
    }
}
