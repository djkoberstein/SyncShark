using SyncShark.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.Engine.FileSystem
{
    public class DirectoryInfoFactory : IDirectoryInfoFactory
    {
        public IDirectoryInfo GetDirectoryInfo(string directoryPath)
        {
            return new DirectoryInfoFacade(new DirectoryInfo(directoryPath));
        }
    }
}
