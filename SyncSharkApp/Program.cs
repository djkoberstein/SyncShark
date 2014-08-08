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
            try
            {
                ISyncSharkService syncSharkService = Compose();

                IDirectoryInfo leftDirectoryInfo = new DirectoryInfoFacade(new DirectoryInfo(args[0]));
                IDirectoryInfo rightDirectoryInfo = new DirectoryInfoFacade(new DirectoryInfo(args[1]));
                syncSharkService.Mirror(leftDirectoryInfo, rightDirectoryInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }

        static ISyncSharkService Compose()
        {
            // Mirror components
            MemorySnapshotStrategy memorySnapshotStrategy = new MemorySnapshotStrategy();
            ICompareStrategy mirrorCompareStrategy = new MirrorCompareStrategy(memorySnapshotStrategy);
            IExecuteStrategy mirrorExecuteStrategy = new ExecuteStrategy(mirrorCompareStrategy);

            // Sync components
            IDirectorySnapshotStrategy fileSystemSnapshotStrategy = new FileSystemSnapshotStrategy(memorySnapshotStrategy);
            IDirectorySnapshotStrategy appFileFilter = new DirectorySnapshotFilterDecorator(fileSystemSnapshotStrategy);
            ICompareStrategy syncCompareStrategy = new SyncCompareStrategy(appFileFilter);
            IExecuteStrategy syncExecuteStrategy = new ExecuteStrategy(syncCompareStrategy);
            
            // Service
            ISyncSharkService syncSharkService = new SyncSharkService(syncExecuteStrategy, mirrorExecuteStrategy);
            
            return syncSharkService;
        }
    }
}
 