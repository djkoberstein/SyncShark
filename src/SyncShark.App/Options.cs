﻿using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.App
{
    public class Options
    {
        [Option('l', "left", Required = true, HelpText = "Left directory path.")]
        public string LeftDirectoryPath { get; set; }

        [Option('r', "right", Required = true, HelpText = "Right directory path.")]
        public string RightDirectoryPath { get; set; }

        [Option('s', "sync", Required = false, DefaultValue = true, HelpText = "Syncronizes changes in both folders.")]
        public bool Sync { get; set; }

        [Option('m', "mirror", Required = false, DefaultValue = false, HelpText = "Mirrors files from the left directory to the right directory.")]
        public bool Mirror { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            // this without using CommandLine.Text
            var usage = new StringBuilder();
            usage.AppendLine(@"usage: SyncSharkApp [-l <dirPath>] [-r <dirPath>] [-s] [-m]");
            usage.AppendLine(@"  -l or -left     Specify the left directory path");
            usage.AppendLine(@"  -r or -right    Specify the right directory path");
            usage.AppendLine(@"  -s or -sync     Syncronizes changes in both directories. (Default)");
            usage.AppendLine(@"  -m or -mirror   Mirrors files from the left directory to the right directory");
            return usage.ToString();
        }
    }
}
