using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.Interfaces
{
    public interface ISyncWorkItem
    {
        IFileSystemInfo Source { get; }
        IFileSystemInfo Destination { get; }
        FileActions FileAction { get; }
    }
}
