using SyncSharkEngine.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.Factories
{
    public interface IDirectorySnapshotServiceFactory
    {
        IDirectorySnapshotService GetDirectorySnapshotService(IDirectoryInfo left, IDirectoryInfo right);
    }
}
