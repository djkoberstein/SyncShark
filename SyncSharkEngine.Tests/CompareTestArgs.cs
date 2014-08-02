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
        public CompareTestArgs(string dirPath)
        {
            IFileInfo[] fileInfos;
            fileInfos = new IFileInfo[0];

            var directoryInfo = new Mock<IDirectoryInfo>();
            directoryInfo.Setup(o => o.FullName).Returns(dirPath);
            directoryInfo.Setup(o => o.GetFiles("*.*", It.IsAny<SearchOption>())).Returns(fileInfos);

            var snapshot = new Dictionary<string, IFileInfo>();

            DirectoryInfo = directoryInfo;
            Snapshot = snapshot;
        }

        public CompareTestArgs(string dirPath, string fileRelativePath, DateTime lastWrite)
        {
            IFileInfo[] fileInfos;
            var fileInfo = new Mock<IFileInfo>();
            fileInfo.Setup(o => o.FullName).Returns(dirPath + @"\" + fileRelativePath);
            fileInfo.Setup(o => o.LastWriteTimeUtc).Returns(lastWrite);
            fileInfos = new IFileInfo[1];
            fileInfos[0] = fileInfo.Object;

            var directoryInfo = new Mock<IDirectoryInfo>();
            directoryInfo.Setup(o => o.FullName).Returns(dirPath);
            directoryInfo.Setup(o => o.GetFiles("*.*", It.IsAny<SearchOption>())).Returns(fileInfos);

            var snapshot = new Dictionary<string, IFileInfo>();
            snapshot.Add(fileRelativePath, fileInfo.Object);

            DirectoryInfo = directoryInfo;
            Snapshot = snapshot;
        }

        public Mock<IDirectoryInfo> DirectoryInfo { get; set; }
        public Dictionary<string, IFileInfo> Snapshot { get; set; }
    }
}
