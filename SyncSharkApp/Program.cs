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

namespace SyncSharkApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IDirectorySnapshotServiceFactory directorySnapshotServiceFactory = new DirectorySnapshotServiceFactory();
            ICompareService compareService = new CompareService(directorySnapshotServiceFactory);
            ISyncSharkServiceFactory syncSharkServiceFactory = new SyncSharkServiceFactory(compareService);
            
            ISyncSharkService syncSharkService = syncSharkServiceFactory.GetSyncSharkService();
            IDirectoryInfo leftDirectoryInfo = new DirectoryInfoFacade(new DirectoryInfo(args[0]));
            IDirectoryInfo rightDirectoryInfo = new DirectoryInfoFacade(new DirectoryInfo(args[1]));
            syncSharkService.CompareAndSync(leftDirectoryInfo, rightDirectoryInfo);
        }

    }
}
