using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.FileSystem
{
    public interface IFileSystemInfoFactory
    {
        IFileSystemInfo GetOtherFileSystemInfo(IDirectoryInfo side1DirectoryInfo, IDirectoryInfo side2DirectoryInfo, IFileSystemInfo side1FileInfo);
    }
}
