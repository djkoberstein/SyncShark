using SyncSharkEngine.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine
{
    public interface IDirectorySnapshotService
    {
        Dictionary<string, IFileInfo> Create(IDirectoryInfo directoryInfo);
        Dictionary<string, IFileInfo> Read(IDirectoryInfo directoryInfo);
        Dictionary<string, IFileInfo> Update(IDirectoryInfo directoryInfo);
        void Delete(IDirectoryInfo directoryInfo);
        bool Exists(IDirectoryInfo directoryInfo);
    }
}
