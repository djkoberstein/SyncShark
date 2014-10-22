using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.Interfaces
{
    public interface IExecuteStrategy
    {
        void CompareAndExecute(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo);
        IEnumerable<ISyncWorkItem> Compare(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo);
        void Execute(IEnumerable<ISyncWorkItem> syncWorkItems);
    }
}
