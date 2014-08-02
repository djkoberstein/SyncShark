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
        IFileInfo Source { get; }
        IFileInfo Destination { get; }
        FileActions FileAction { get; }
    }
}
