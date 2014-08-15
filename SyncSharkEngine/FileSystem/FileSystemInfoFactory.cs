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
        public IFileSystemInfo GetOtherFileSystemInfo(IDirectoryInfo side1DirectoryInfo, IDirectoryInfo side2DirectoryInfo, IFileSystemInfo side1FileInfo)
        {
            string relativePath = side1FileInfo.FullName.Replace(side1DirectoryInfo.FullName, "");
            string rightFullPath = side2DirectoryInfo.FullName + relativePath;

            if (side1FileInfo is IFileInfo)
            {
                return new FileInfoFacade(new FileInfo(rightFullPath));
            }
            else
            {
                return new DirectoryInfoFacade(new DirectoryInfo(rightFullPath));
            }
        }
    }
}
