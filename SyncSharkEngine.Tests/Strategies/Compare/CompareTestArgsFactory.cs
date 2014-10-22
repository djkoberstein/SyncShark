using Moq;
using SyncShark.Engine.FileSystem;
using SyncShark.Interfaces;
using SyncSharkEngine.Tests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncShark.Engine.Tests.Strategies.Compare
{
    class CompareTestArgsFactory
    {
        public CompareTestArgs GetEmptyDirectory(string dirPath)
        {
            IFileSystemInfo[] fileSystemInfos;
            fileSystemInfos = new IFileSystemInfo[0];

            var directoryInfo = new Mock<IDirectoryInfo>();
            directoryInfo.Setup(o => o.FullName).Returns(dirPath);
            directoryInfo.Setup(o => o.GetFileSystemInfos()).Returns(fileSystemInfos);

            var snapshot = new Dictionary<string, IFileSystemInfo>();

            CompareTestArgs compareTestArgs = new CompareTestArgs();
            compareTestArgs.DirectoryInfo = directoryInfo;
            compareTestArgs.Snapshot = snapshot;
            return compareTestArgs;
        }

        public CompareTestArgs GetSingleFile(string dirPath, string fileRelativePath, DateTime lastWrite)
        {
            var fileSystemInfo = new Mock<IFileSystemInfo>();
            fileSystemInfo.Setup(o => o.FullName).Returns(dirPath + @"\" + fileRelativePath);
            fileSystemInfo.Setup(o => o.LastWriteTimeUtc).Returns(lastWrite);
            IFileSystemInfo[] fileSystemInfos = new IFileSystemInfo[1];
            fileSystemInfos[0] = fileSystemInfo.Object;

            var directoryInfo = new Mock<IDirectoryInfo>();
            directoryInfo.Setup(o => o.FullName).Returns(dirPath);
            directoryInfo.Setup(o => o.GetFileSystemInfos()).Returns(fileSystemInfos);

            var snapshot = new Dictionary<string, IFileSystemInfo>();
            snapshot.Add(fileRelativePath, fileSystemInfo.Object);

            CompareTestArgs compareTestArgs = new CompareTestArgs();
            compareTestArgs.DirectoryInfo = directoryInfo;
            compareTestArgs.Snapshot = snapshot;
            return compareTestArgs;
        }

        public CompareTestArgs GetSingleSubDirectory(string dirPath, string dirRelativePath, DateTime lastWrite)
        {
            var fileSystemInfo = new Mock<IFileSystemInfo>();
            fileSystemInfo.Setup(o => o.FullName).Returns(dirPath + @"\" + dirRelativePath);
            fileSystemInfo.Setup(o => o.LastWriteTimeUtc).Returns(lastWrite);
            IFileSystemInfo[] fileSystemInfos = new IFileSystemInfo[1];
            fileSystemInfos[0] = fileSystemInfo.Object;

            var directoryInfo = new Mock<IDirectoryInfo>();
            directoryInfo.Setup(o => o.FullName).Returns(dirPath);
            directoryInfo.Setup(o => o.GetFileSystemInfos()).Returns(fileSystemInfos);

            var snapshot = new Dictionary<string, IFileSystemInfo>();
            snapshot.Add(dirRelativePath, fileSystemInfo.Object);

            CompareTestArgs compareTestArgs = new CompareTestArgs();
            compareTestArgs.DirectoryInfo = directoryInfo;
            compareTestArgs.Snapshot = snapshot;
            return compareTestArgs;
        }
    }
}
