using SyncSharkEngine.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.Factories
{
    public class DirectorySnapshotServiceFactory : IDirectorySnapshotServiceFactory
    {
        public IDirectorySnapshotService GetDirectorySnapshotService(IDirectoryInfo left, IDirectoryInfo right)
        {
            List<IDirectoryInfo> list = new List<IDirectoryInfo>();
            list.Add(left);
            list.Add(right);
            return new DirectorySnapshotService(list);
        }
    }
}
