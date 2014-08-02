using SyncSharkEngine.DataModel;
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
        private IDirectorySnapshotServiceFactory m_DirectorySnapshotServiceFactory;

        public SyncSharkService(IDirectorySnapshotServiceFactory directorySnapshotServiceFactory)
        {
            m_DirectorySnapshotServiceFactory = directorySnapshotServiceFactory;
        }

        public void CompareAndSync(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo)
        {
            var syncWorkItems = Compare(leftDirectoryInfo, rightDirectoryInfo);
            Sync(syncWorkItems);
        }

        public IEnumerable<ISyncWorkItem> Compare(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo)
        {
            IDirectorySnapshotService directorySnapshotService = m_DirectorySnapshotServiceFactory.GetDirectorySnapshotService(leftDirectoryInfo, rightDirectoryInfo);
            Dictionary<string, IFileInfo> previousLeftSnapshot = ReadOrCreate(directorySnapshotService, leftDirectoryInfo);
            Dictionary<string, IFileInfo> previousRightSnapshot = ReadOrCreate(directorySnapshotService, rightDirectoryInfo);
            Dictionary<string, IFileInfo> leftSnapshot = directorySnapshotService.Update(leftDirectoryInfo);
            Dictionary<string, IFileInfo> rightSnapshot = directorySnapshotService.Update(rightDirectoryInfo);

            foreach (var kvp in leftSnapshot)
            {
                if (rightSnapshot.ContainsKey(kvp.Key))
                {
                    IFileInfo leftFileInfo = kvp.Value;
                    IFileInfo rightFileInfo = rightSnapshot[kvp.Key];
                    yield return ProcessFileFoundInBothFolders(leftFileInfo, rightFileInfo);
                }
                else
                {
                    IFileInfo leftFileInfo = kvp.Value;
                    string relativePath = leftFileInfo.FullName.Replace(leftDirectoryInfo.FullName, "");
                    string rightFullPath = rightDirectoryInfo.FullName + relativePath;
                    IFileInfo rightFileInfo = new FileInfoFacade(new FileInfo(rightFullPath));
                    if (previousLeftSnapshot.ContainsKey(kvp.Key))
                    {
                        yield return new SyncWorkItem(rightFileInfo, leftFileInfo, FileActions.DELETE);
                    }
                    else
                    {
                        yield return new SyncWorkItem(leftFileInfo, rightFileInfo, FileActions.COPY);
                    }
                }
                rightSnapshot.Remove(kvp.Key);
            }

            foreach(var kvp in rightSnapshot)
            {
                IFileInfo rightFileInfo = kvp.Value;
                string relativePath = rightFileInfo.FullName.Replace(rightDirectoryInfo.FullName, "");
                string leftFullPath = leftDirectoryInfo.FullName + relativePath;
                IFileInfo leftFileInfo = new FileInfoFacade(new FileInfo(leftFullPath));
                if (previousRightSnapshot.ContainsKey(kvp.Key))
                {
                    yield return new SyncWorkItem(leftFileInfo, rightFileInfo, FileActions.DELETE);
                }
                else
                {
                    yield return new SyncWorkItem(rightFileInfo, leftFileInfo, FileActions.COPY);
                }
            }

            yield break;
        }

        private Dictionary<string, IFileInfo> ReadOrCreate(IDirectorySnapshotService directorySnapshotService, IDirectoryInfo directoryInfo)
        {
            if (directorySnapshotService.Exists(directoryInfo))
            {
                return directorySnapshotService.Read(directoryInfo);
            }
            else
            {
                return directorySnapshotService.Create(directoryInfo);
            }
        }

        private ISyncWorkItem ProcessFileFoundInBothFolders(IFileInfo leftFileInfo, IFileInfo rightFileInfo)
        {
            if (leftFileInfo.LastWriteTimeUtc > rightFileInfo.LastWriteTimeUtc)
            {
                return new SyncWorkItem(leftFileInfo, rightFileInfo, FileActions.COPY);
            }
            else if (leftFileInfo.LastWriteTimeUtc < rightFileInfo.LastWriteTimeUtc)
            {
                return new SyncWorkItem(rightFileInfo, leftFileInfo, FileActions.COPY);
            }
            else
            {
                return new SyncWorkItem(rightFileInfo, leftFileInfo, FileActions.NONE);
            }
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
