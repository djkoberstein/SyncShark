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
using SyncSharkEngine.Tests.Compare;

namespace SyncSharkApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SyncSharkFactory syncSharkServiceFactory = new SyncSharkFactory();
                SyncSharkUI syncSharkUI = syncSharkServiceFactory.GetSyncShark();
                syncSharkUI.Run(args);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }
    }
}
 