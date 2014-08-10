using Moq;
using SyncSharkEngine.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.Tests
{
    class CompareTestArgs
    {
        public Mock<IDirectoryInfo> DirectoryInfo { get; set; }
        public Dictionary<string, IFileSystemInfo> Snapshot { get; set; }
    }
}
