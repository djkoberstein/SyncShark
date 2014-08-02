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

        public SyncCompareStrategy(IDirectorySnapshotStrategy directorySnapshotStrategy)
        {
            m_DirectorySnapshotStrategy = directorySnapshotStrategy;
        }

        public IEnumerable<ISyncWorkItem> Compare(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo)
        {
            Dictionary<string, IFileInfo> previousLeftSnapshot = m_DirectorySnapshotStrategy.ReadOrCreate(leftDirectoryInfo);
            Dictionary<string, IFileInfo> previousRightSnapshot = m_DirectorySnapshotStrategy.ReadOrCreate(rightDirectoryInfo);
            Dictionary<string, IFileInfo> leftSnapshot = m_DirectorySnapshotStrategy.Update(leftDirectoryInfo);
            Dictionary<string, IFileInfo> rightSnapshot = m_DirectorySnapshotStrategy.Update(rightDirectoryInfo);

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

        private ISyncWorkItem ProcessFileFoundOnlyInLeftFolder(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo, IFileInfo leftFileInfo, bool foundInPreviousLeftSnapshot)
        {
            string relativePath = leftFileInfo.FullName.Replace(leftDirectoryInfo.FullName, "");
            string rightFullPath = rightDirectoryInfo.FullName + relativePath;
            IFileInfo rightFileInfo = new FileInfoFacade(new FileInfo(rightFullPath));
            if (foundInPreviousLeftSnapshot)
            {
                return new SyncWorkItem(rightFileInfo, leftFileInfo, FileActions.DELETE);
            }
            else
            {
                return new SyncWorkItem(leftFileInfo, rightFileInfo, FileActions.COPY);
            }
        }

        private ISyncWorkItem ProcessFileFoundOnlyInRightFolder(IDirectoryInfo leftDirectoryInfo, IDirectoryInfo rightDirectoryInfo, IFileInfo rightFileInfo, bool foundInPreviousRightSnapshot)
        {
            string relativePath = rightFileInfo.FullName.Replace(rightDirectoryInfo.FullName, "");
            string leftFullPath = leftDirectoryInfo.FullName + relativePath;
            IFileInfo leftFileInfo = new FileInfoFacade(new FileInfo(leftFullPath));
            if (foundInPreviousRightSnapshot)
            {
                return new SyncWorkItem(leftFileInfo, rightFileInfo, FileActions.DELETE);
            }
            else
            {
                return new SyncWorkItem(rightFileInfo, leftFileInfo, FileActions.COPY);
            }
        }
    }
}
