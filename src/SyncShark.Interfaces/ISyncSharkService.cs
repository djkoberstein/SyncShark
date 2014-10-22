using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.Interfaces
{
    public interface ISyncSharkService
    {
        void Sync(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo);
        void Mirror(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo);
    }
}
