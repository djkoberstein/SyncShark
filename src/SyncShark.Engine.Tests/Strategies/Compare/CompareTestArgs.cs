using Moq;
using SyncShark.Engine.FileSystem;
using SyncShark.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.Engine.Tests
{
    class CompareTestArgs
    {
        public Mock<IDirectoryInfo> DirectoryInfo { get; set; }
        public Dictionary<string, IFileSystemInfo> Snapshot { get; set; }
    }
}
