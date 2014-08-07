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
        void Sync();
        void Mirror();
    }
}
