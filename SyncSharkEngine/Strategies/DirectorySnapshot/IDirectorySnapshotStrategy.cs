using SyncSharkEngine.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.Strategies.DirectorySnapshot
{
    public interface IDirectorySnapshotStrategy
    {
        Dictionary<string, IFileInfo> Create(IDirectoryInfo directoryInfo);
        Dictionary<string, IFileInfo> Read(IDirectoryInfo directoryInfo);
        Dictionary<string, IFileInfo> Update(IDirectoryInfo directoryInfo);
        Dictionary<string, IFileInfo> ReadOrCreate(IDirectoryInfo directoryInfo);
        void Delete(IDirectoryInfo directoryInfo);
        bool Exists(IDirectoryInfo directoryInfo);
    }
}
