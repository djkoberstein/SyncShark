using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.Interfaces
{
    public interface IFileInfo : IFileSystemInfo
    {
        Stream OpenRead();
        Stream OpenWrite();
    }
}
