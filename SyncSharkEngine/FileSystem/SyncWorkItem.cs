using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.FileSystem
{
    public class SyncWorkItem : ISyncWorkItem
    {
        public IFileInfo Source { get; private set; }
        public IFileInfo Destination { get; private set; }
        public FileActions FileAction { get; private set; }

        public SyncWorkItem(IFileInfo source, IFileInfo destination, FileActions fileAction)
        {
            this.Source = source;
            this.Destination = destination;
            this.FileAction = fileAction;
        }
    }
}
