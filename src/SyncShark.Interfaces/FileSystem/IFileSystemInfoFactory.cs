using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.Interfaces
{
    public interface IFileSystemInfoFactory
    {
        IFileSystemInfo GetOtherFileSystemInfo(IDirectoryInfo side1DirectoryInfo, IDirectoryInfo side2DirectoryInfo, IFileSystemInfo side1FileInfo);
    }
}
