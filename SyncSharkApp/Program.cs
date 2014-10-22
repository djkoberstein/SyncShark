using SyncShark.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                var syncSharkServiceFactory = new SyncSharkFactory();
                var syncSharkService = syncSharkServiceFactory.GetSyncShark();
                var syncSharkUI = new SyncSharkUI(syncSharkService);
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
 