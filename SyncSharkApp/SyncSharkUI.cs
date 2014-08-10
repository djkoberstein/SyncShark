using CommandLine;
using SyncSharkEngine;
using SyncSharkEngine.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkApp
{
    public class Options 
    {
        [Option('l', "left", Required = true, HelpText = "Left directory path.")]
        public string LeftDirectoryPath { get; set; }

        [Option('r', "right", Required = true, HelpText = "Right directory path.")]
        public string RightDirectoryPath { get; set; }

        [Option('s', "sync", Required = false, DefaultValue = false, HelpText = "Syncronizes changes in both folders.")]
        public bool Mirror { get; set; }

        [Option('m', "mirror", Required = false, DefaultValue = true, HelpText = "Mirrors files from the left directory to the right directory.")]
        public bool Sync { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            // this without using CommandLine.Text
            var usage = new StringBuilder();
            usage.AppendLine(@"SyncSharkApp");
            usage.AppendLine();
            usage.AppendLine(@"  \l or \left - Left directory path");
            usage.AppendLine(@"  \r or \right - Right directory path");
            usage.AppendLine(@"  \s or \sync - Syncronizes changes in both folders");
            usage.AppendLine(@"  \m or \mirror - Mirrors files from the left directory to the right directory");
            return usage.ToString();
        }
    }

    public class SyncSharkUI
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
