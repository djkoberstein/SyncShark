using SyncSharkEngine.FileSystem;
using SyncSharkEngine.Factories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncSharkEngine.Strategies.Compare;

namespace SyncSharkEngine
{
    public class SyncSharkService : ISyncSharkService
    {
        private ICompareStrategy m_CompareStrategy;
        private IDirectoryInfo m_LeftDirectoryInfo;
        private IDirectoryInfo m_RightDirectoryInfo;

        public SyncSharkService(ICompareStrategy compareStrategy, IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo)
        {
            m_CompareStrategy = compareStrategy;
            m_LeftDirectoryInfo = leftDirectoryInfo;
            m_RightDirectoryInfo = rightDirectoryInfo;
        }

        public void CompareAndSync()
        {
            var syncWorkItems = Compare();
            Sync(syncWorkItems);
        }

        public IEnumerable<ISyncWorkItem> Compare()
        {
            return m_CompareStrategy.Compare(m_LeftDirectoryInfo, m_RightDirectoryInfo);
        }

        public void Sync(IEnumerable<ISyncWorkItem> syncWorkItems)
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
