using SyncSharkApp;
using SyncSharkEngine.FileSystem;
using SyncSharkEngine.Strategies;
using SyncSharkEngine.Strategies.Compare;
using SyncSharkEngine.Strategies.DirectorySnapshot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine
{
    public class SyncSharkFactory
    {
        public SyncSharkUI GetSyncShark()
        {
            // Mirror components
            MemorySnapshotStrategy memorySnapshotStrategy = new MemorySnapshotStrategy();
            ICompareStrategy mirrorCompareStrategy = new MirrorCompareStrategy(memorySnapshotStrategy);
            IExecuteStrategy mirrorExecuteStrategy = new ExecuteStrategy(mirrorCompareStrategy);

            // Sync components
            IDirectorySnapshotStrategy fileSystemSnapshotStrategy = new FileSystemSnapshotStrategy(memorySnapshotStrategy);
            IDirectorySnapshotStrategy appFileFilter = new DirectorySnapshotFilterDecorator(fileSystemSnapshotStrategy);
            IFileSystemInfoFactory fileSystemInfoFactory = new FileSystemInfoFactory();
            ICompareStrategy syncCompareStrategy = new SyncCompareStrategy(appFileFilter, fileSystemInfoFactory);
            IExecuteStrategy syncExecuteStrategy = new ExecuteStrategy(syncCompareStrategy);

            // Service
            ISyncSharkService syncSharkService = new SyncSharkService(syncExecuteStrategy, mirrorExecuteStrategy);
            SyncSharkUI syncSharkUI = new SyncSharkUI(syncSharkService);

            return syncSharkUI;
        }
    }
}
