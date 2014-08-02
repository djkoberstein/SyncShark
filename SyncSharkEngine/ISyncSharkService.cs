using SyncSharkEngine.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine
{
    public interface ISyncSharkService
    {
        void CompareAndSync();
        IEnumerable<ISyncWorkItem> Compare();
        void Sync(IEnumerable<ISyncWorkItem> syncWorkItems);
    }
}
