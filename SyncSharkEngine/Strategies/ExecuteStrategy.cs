using SyncSharkEngine.FileSystem;
using SyncSharkEngine.Strategies.Compare;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.Strategies
{
    public class ExecuteStrategy : IExecuteStrategy
    {
        private ICompareStrategy m_CompareStrategy;

        public ExecuteStrategy(ICompareStrategy compareStrategy)
        {
            m_CompareStrategy = compareStrategy;
        }


        public void CompareAndExecute(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo)
        {
            var syncWorkItems = Compare(leftDirectoryInfo, rightDirectoryInfo);
            Execute(syncWorkItems);
        }

        public IEnumerable<ISyncWorkItem> Compare(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo)
        {
            return m_CompareStrategy.Compare(leftDirectoryInfo, rightDirectoryInfo);
        }

        public void Execute(IEnumerable<ISyncWorkItem> syncWorkItems)
        {
            foreach (var syncWorkItem in syncWorkItems)
            {
                switch (syncWorkItem.FileAction)
                {
                    case FileActions.NONE:
                        break;
                    case FileActions.COPY:
                        syncWorkItem.Destination.Delete();
                        using (Stream readStream = syncWorkItem.Source.OpenRead())
                        using (Stream writeStream = syncWorkItem.Destination.OpenWrite())
                        {
                            readStream.CopyTo(writeStream);
                        }
                        break;
                    case FileActions.DELETE:
                        syncWorkItem.Destination.Delete();
                        break;
                    case FileActions.CONFLICT:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
