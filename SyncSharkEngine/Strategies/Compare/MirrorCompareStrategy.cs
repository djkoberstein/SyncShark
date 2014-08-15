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
    public class MirrorCompareStrategy : ICompareStrategy
    {
        private IDirectorySnapshotStrategy m_DirectorySnapshotStrategy;
        private IFileSystemInfoFactory m_FileSystemInfoFactory;

        public MirrorCompareStrategy(IDirectorySnapshotStrategy directorySnapshotStrategy, IFileSystemInfoFactory fileSystemInfoFactory)
        {
            m_DirectorySnapshotStrategy = directorySnapshotStrategy;
            m_FileSystemInfoFactory = fileSystemInfoFactory;
        }

        public IEnumerable<ISyncWorkItem> Compare(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo)
        {
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
                    yield return ProcessFileFoundOnlyInLeftFolder(leftDirectoryInfo, rightDirectoryInfo, kvp.Value);
                }
                rightSnapshot.Remove(kvp.Key);
            }

            foreach (var kvp in rightSnapshot)
            {
                yield return ProcessFileFoundOnlyInRightFolder(leftDirectoryInfo, rightDirectoryInfo, kvp.Value);
            }

            yield break;
        }

        private ISyncWorkItem ProcessFileFoundInBothFolders(IFileSystemInfo leftFileInfo, IFileSystemInfo rightFileInfo)
        {
            if (leftFileInfo.LastWriteTimeUtc != rightFileInfo.LastWriteTimeUtc)
            {
                return new SyncWorkItem(leftFileInfo, rightFileInfo, FileActions.COPY);
            }
            else
            {
                return new SyncWorkItem(rightFileInfo, leftFileInfo, FileActions.NONE);
            }
        }

        private ISyncWorkItem ProcessFileFoundOnlyInLeftFolder(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo, IFileSystemInfo leftFileSystemInfo)
        {
            IFileSystemInfo rightFileSystemInfo = m_FileSystemInfoFactory.GetOtherFileSystemInfo(leftDirectoryInfo, rightDirectoryInfo, leftFileSystemInfo);
            return new SyncWorkItem(leftFileSystemInfo, rightFileSystemInfo, FileActions.COPY);
        }

        private ISyncWorkItem ProcessFileFoundOnlyInRightFolder(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo, IFileSystemInfo rightFileSystemInfo)
        {
            IFileSystemInfo leftFileSystemInfo = m_FileSystemInfoFactory.GetOtherFileSystemInfo(rightDirectoryInfo, leftDirectoryInfo, rightFileSystemInfo);
            return new SyncWorkItem(leftFileSystemInfo, rightFileSystemInfo, FileActions.DELETE);
        }
    }
}
