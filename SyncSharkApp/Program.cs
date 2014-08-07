using SyncSharkEngine;
using SyncSharkEngine.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncSharkEngine.Strategies.Compare;
using SyncSharkEngine.Strategies.DirectorySnapshot;
using SyncSharkEngine.Strategies;

namespace SyncSharkApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ISyncSharkService syncSharkService = Compose(args[0], args[1]);
            syncSharkService.Sync();
        }

        static ISyncSharkService Compose(string leftDirectoryPath, string rightDirectoryPath)
        {
            IDirectoryInfo leftDirectoryInfo = new DirectoryInfoFacade(new DirectoryInfo(leftDirectoryPath));
            IDirectoryInfo rightDirectoryInfo = new DirectoryInfoFacade(new DirectoryInfo(rightDirectoryPath));
            IDirectorySnapshotStrategy inMemorySnapshotStrategy = new InMemorySnapshotStrategy();
            IDirectorySnapshotStrategy fileSystemSnapshotStrategy = new FileSystemSnapshotStrategy(inMemorySnapshotStrategy, leftDirectoryInfo, rightDirectoryInfo);
            ICompareStrategy mirrorCompareStrategy = new MirrorCompareStrategy(inMemorySnapshotStrategy);
            ICompareStrategy syncCompareStrategy = new SyncCompareStrategy(fileSystemSnapshotStrategy);
            IExecuteStrategy mirrorExecuteStrategy = new ExecuteStrategy(mirrorCompareStrategy, leftDirectoryInfo, rightDirectoryInfo);
            IExecuteStrategy syncExecuteStrategy = new ExecuteStrategy(syncCompareStrategy, leftDirectoryInfo, rightDirectoryInfo);
            ISyncSharkService syncSharkService = new SyncSharkService(syncExecuteStrategy, mirrorExecuteStrategy);
            
            return syncSharkService;
        }
    }
}
 