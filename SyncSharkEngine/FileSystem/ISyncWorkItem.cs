using SyncSharkEngine.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.FileSystem
{
    public interface ISyncWorkItem
    {
        IFileSystemInfo Source { get; }
        IFileSystemInfo Destination { get; }
        FileActions FileAction { get; }
    }
}
