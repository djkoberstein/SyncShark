using CommandLine;
using SyncShark.Engine.FileSystem;
using SyncShark.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.App
{
    public class SyncSharkUI : ISyncSharkUI
    {
        private ISyncSharkService m_SyncSharkService;
        public SyncSharkUI(ISyncSharkService syncSharkService)
        {
            m_SyncSharkService = syncSharkService;
        }

        public void Run(string[] args)
        {
            Options options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {
                IDirectoryInfo leftDirectoryInfo = new DirectoryInfoFacade(new DirectoryInfo(options.LeftDirectoryPath));
                IDirectoryInfo rightDirectoryInfo = new DirectoryInfoFacade(new DirectoryInfo(options.RightDirectoryPath));
                if (options.Mirror)
                {
                    m_SyncSharkService.Mirror(leftDirectoryInfo, rightDirectoryInfo);
                }
                else
                {
                    m_SyncSharkService.Sync(leftDirectoryInfo, rightDirectoryInfo);
                }
            }
        }
    }
}
