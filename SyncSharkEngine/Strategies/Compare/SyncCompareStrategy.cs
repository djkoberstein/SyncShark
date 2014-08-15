using SyncSharkEngine.FileSystem;
using SyncSharkEngine.Strategies.DirectorySnapshot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.Strategies.Compare
{
    public class SyncCompareStrategy : ICompareStrategy
    {
        private IDirectorySnapshotStrategy m_DirectorySnapshotStrategy;
        private IFileSystemInfoFactory m_FileSystemInfoFactory;

        public SyncCompareStrategy(IDirectorySnapshotStrategy directorySnapshotStrategy, IFileSystemInfoFactory fileSystemInfoFactory)
        {
            m_DirectorySnapshotStrategy = directorySnapshotStrategy;
            m_FileSystemInfoFactory = fileSystemInfoFactory;
        }

        public IEnumerable<ISyncWorkItem> Compare(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo)
        {
            Dictionary<string, IFileSystemInfo> previousLeftSnapshot = m_DirectorySnapshotStrategy.Read(leftDirectoryInfo);
            Dictionary<string, IFileSystemInfo> previousRightSnapshot = m_DirectorySnapshotStrategy.Read(rightDirectoryInfo);
            Dictionary<string, IFileSystemInfo> leftSnapshot = m_DirectorySnapshotStrategy.Update(leftDirectoryInfo);
            Dictionary<string, IFileSystemInfo> rightSnapshot = m_DirectorySnapshotStrategy.Update(rightDirectoryInfo);

            foreach (var kvp in leftSnapshot)
            {
                if (rightSnapshot.ContainsKey(kvp.Key))
                {
                    yield return ProcessFileFoundInBothFolders(kvp.Value, rightSnapshot[kvp.Key]);
                }
                else
                {
                    yield return ProcessFileFoundOnlyInLeftFolder(leftDirectoryInfo, rightDirectoryInfo, kvp.Value, previousLeftSnapshot.ContainsKey(kvp.Key));
                }
                rightSnapshot.Remove(kvp.Key);
            }

            foreach (var kvp in rightSnapshot)
            {
                yield return ProcessFileFoundOnlyInRightFolder(leftDirectoryInfo, rightDirectoryInfo, kvp.Value, previousRightSnapshot.ContainsKey(kvp.Key));
            }

            yield break;
        }

        private ISyncWorkItem ProcessFileFoundInBothFolders(IFileSystemInfo leftFileInfo, IFileSystemInfo rightFileInfo)
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

        private ISyncWorkItem ProcessFileFoundOnlyInLeftFolder(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo, IFileSystemInfo leftFileSystemInfo, bool foundInPreviousLeftSnapshot)
        {
            IFileSystemInfo rightFileSystemInfo = m_FileSystemInfoFactory.GetOtherFileSystemInfo(leftDirectoryInfo, rightDirectoryInfo, leftFileSystemInfo);
            if (foundInPreviousLeftSnapshot)
            {
                return new SyncWorkItem(rightFileSystemInfo, leftFileSystemInfo, FileActions.DELETE);
            }
            else
            {
                return new SyncWorkItem(leftFileSystemInfo, rightFileSystemInfo, FileActions.COPY);
            }
        }

        private ISyncWorkItem ProcessFileFoundOnlyInRightFolder(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo, IFileSystemInfo rightFileSystemInfo, bool foundInPreviousRightSnapshot)
        {
            IFileSystemInfo leftFileSystemInfo = m_FileSystemInfoFactory.GetOtherFileSystemInfo(rightDirectoryInfo, leftDirectoryInfo, rightFileSystemInfo);
            if (foundInPreviousRightSnapshot)
            {
                return new SyncWorkItem(leftFileSystemInfo, rightFileSystemInfo, FileActions.DELETE);
            }
            else
            {
                return new SyncWorkItem(rightFileSystemInfo, leftFileSystemInfo, FileActions.COPY);
            }
        }
    }
}
