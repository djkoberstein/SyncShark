using SyncSharkEngine.FileSystem;
using SyncSharkEngine.Factories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine
{
    public class SyncSharkService : ISyncSharkService
    {
        private ICompareService m_CompareService;

        public SyncSharkService(ICompareService compareService)
        {
            m_CompareService = compareService;
        }

        public void CompareAndSync(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo)
        {
            var syncWorkItems = Compare(leftDirectoryInfo, rightDirectoryInfo);
            Sync(syncWorkItems);
        }

        public IEnumerable<ISyncWorkItem> Compare(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo)
        {
            return m_CompareService.Compare(leftDirectoryInfo, rightDirectoryInfo);
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
