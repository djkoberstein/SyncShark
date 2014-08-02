using SyncSharkEngine;
using SyncSharkEngine.FileSystem;
using SyncSharkEngine.Factories;
using SyncSharkEngine.Tests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncSharkEngine.Strategies.Compare;
using SyncSharkEngine.Strategies.DirectorySnapshot;

namespace SyncSharkApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ISyncSharkService syncSharkService = ComposeMirror(args[0], args[1]);
            syncSharkService.CompareAndSync();
        }

        static ISyncSharkService ComposeSync(string leftDirectoryPath, string rightDirectoryPath)
        {
            IDirectoryInfo leftDirectoryInfo = new DirectoryInfoFacade(new DirectoryInfo(leftDirectoryPath));
            IDirectoryInfo rightDirectoryInfo = new DirectoryInfoFacade(new DirectoryInfo(rightDirectoryPath));
            List<IDirectoryInfo> directoryInfos = new List<IDirectoryInfo>();
            directoryInfos.Add(leftDirectoryInfo);
            directoryInfos.Add(rightDirectoryInfo);

            InMemorySnapshotStrategy inMemorySnapshotStrategy = new InMemorySnapshotStrategy();
            IDirectorySnapshotStrategy directorySnapshotStrategy = new FileSystemSnapshotStrategy(inMemorySnapshotStrategy, directoryInfos);
            ICompareStrategy compareStrategy = new SyncCompareStrategy(directorySnapshotStrategy);
            ISyncSharkServiceFactory syncSharkServiceFactory = new SyncSharkServiceFactory(compareStrategy);

            ISyncSharkService syncSharkService = syncSharkServiceFactory.GetSyncSharkService(leftDirectoryInfo, rightDirectoryInfo);
            return syncSharkService;
        }

        static ISyncSharkService ComposeMirror(string leftDirectoryPath, string rightDirectoryPath)
        {
            InMemorySnapshotStrategy inMemorySnapshotStrategy = new InMemorySnapshotStrategy();
            ICompareStrategy compareStrategy = new MirrorCompareStrategy(inMemorySnapshotStrategy);
            ISyncSharkServiceFactory syncSharkServiceFactory = new SyncSharkServiceFactory(compareStrategy);

            IDirectoryInfo leftDirectoryInfo = new DirectoryInfoFacade(new DirectoryInfo(leftDirectoryPath));
            IDirectoryInfo rightDirectoryInfo = new DirectoryInfoFacade(new DirectoryInfo(rightDirectoryPath));
            ISyncSharkService syncSharkService = syncSharkServiceFactory.GetSyncSharkService(leftDirectoryInfo, rightDirectoryInfo);
            return syncSharkService;
        }
    }
}
 