using SyncShark.Engine;
using SyncShark.Engine.FileSystem;
using SyncShark.Engine.Strategies;
using SyncShark.Engine.Strategies.Compare;
using SyncShark.Engine.Strategies.DirectorySnapshot;
using SyncShark.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.App
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var syncSharkUI = Componse();
                syncSharkUI.Run(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }

        static ISyncSharkUI Componse()
        {
            var fileSystemInfoFactory = new FileSystemInfoFactory();

            // Mirror components
            var memorySnapshotStrategy = new MemorySnapshotStrategy();
            var mirrorCompareStrategy = new MirrorCompareStrategy(memorySnapshotStrategy, fileSystemInfoFactory);
            var mirrorExecuteStrategy = new ExecuteStrategy(mirrorCompareStrategy);

            // Sync components
            var fileSystemSnapshotStrategy = new FileSystemSnapshotStrategy(memorySnapshotStrategy);
            var snapshotFilter = new DirectorySnapshotBlacklistFilter(fileSystemSnapshotStrategy);
            var syncCompareStrategy = new SyncCompareStrategy(snapshotFilter, fileSystemInfoFactory);
            var syncExecuteStrategy = new ExecuteStrategy(syncCompareStrategy);

            // Service
            var syncSharkService = new SyncSharkService(syncExecuteStrategy, mirrorExecuteStrategy);
            var syncSharkUI = new SyncSharkUI(syncSharkService);

            return syncSharkUI;
        }
    }
}