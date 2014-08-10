using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.FileSystem
{
    public class FileSystemInfoFactory : IFileSystemInfoFactory
    {
        public IFileSystemInfo GetFileSystemInfo(string path, bool isDirectory)
        {
            if (isDirectory)
            {
                return new DirectoryInfoFacade(new DirectoryInfo(path));
            }
            else
            {
                return new FileInfoFacade(new FileInfo(path));
            }
        }
    }
}
