using SyncSharkEngine.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.Strategies
{
    public interface IExecuteStrategy
    {
        void CompareAndExecute();
        IEnumerable<ISyncWorkItem> Compare();
        void Execute(IEnumerable<ISyncWorkItem> syncWorkItems);
    }
}
