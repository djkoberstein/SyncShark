using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.Interfaces
{
    public interface IDirectorySnapshotStrategy
    {
        Dictionary<string, IFileSystemInfo> Create(IDirectoryInfo directoryInfo);
        Dictionary<string, IFileSystemInfo> Read(IDirectoryInfo directoryInfo);
        Dictionary<string, IFileSystemInfo> Update(IDirectoryInfo directoryInfo);
    }
}
