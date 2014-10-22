using SyncShark.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.Engine.FileSystem
{
    public class SyncWorkItem : ISyncWorkItem
    {
        public IFileSystemInfo Source { get; private set; }
        public IFileSystemInfo Destination { get; private set; }
        public FileActions FileAction { get; private set; }

        public SyncWorkItem(IFileSystemInfo source, IFileSystemInfo destination, FileActions fileAction)
        {
            this.Source = source;
            this.Destination = destination;
            this.FileAction = fileAction;
        }
    }
}
