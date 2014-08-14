using Moq;
using NUnit.Framework;
using SyncSharkEngine.FileSystem;
using SyncSharkEngine.Strategies;
using SyncSharkEngine.Strategies.Compare;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.Tests
{
    [TestFixture]
    public class ExecuteStrategyTests
    {
        private const string DIRECTORY_PATH_LEFT = @"C:\Folder1";
        private const string DIRECTORY_PATH_RIGHT = @"C:\Folder2";
        private const string FILE_RELATIVE_PATH = @"TestFile.xml";
        private const string FILE_PATH_LEFT = DIRECTORY_PATH_LEFT + @"\" + FILE_RELATIVE_PATH;
        private const string FILE_PATH_RIGHT = DIRECTORY_PATH_RIGHT + @"\" + FILE_RELATIVE_PATH;

        [Test]
        public void Compare_ShouldInvokeCompareStrategy()
        {
            // Arrange
            var compareService = new Mock<ICompareStrategy>();
            var directoryInfo = new Mock<IDirectoryInfo>();
            var executeStrategy = new ExecuteStrategy(null, compareService.Object);

            // Act
            executeStrategy.Compare(directoryInfo.Object, directoryInfo.Object);

            // Assert
            compareService.Verify(o => o.Compare(It.IsAny<IDirectoryInfo>(), It.IsAny<IDirectoryInfo>()), Times.Exactly(1));
        }

        [Test]
        public void Sync_ShouldCallFileInfoReadWriteMethodOnCopy()
        {
            // Arrange
            var sourceFileInfo = new Mock<IFileInfo>();
            sourceFileInfo.Setup(o => o.FullName).Returns(FILE_PATH_LEFT);

            var destinationFileInfo = new Mock<IFileInfo>();
            destinationFileInfo.Setup(o => o.FullName).Returns(FILE_PATH_RIGHT);

            var syncWorkItem = new Mock<ISyncWorkItem>();
            syncWorkItem.Setup(o => o.Source).Returns(sourceFileInfo.Object);
            syncWorkItem.Setup(o => o.Destination).Returns(destinationFileInfo.Object);
            syncWorkItem.Setup(o => o.FileAction).Returns(FileActions.COPY);

            List<ISyncWorkItem> syncWorkItemList = new List<ISyncWorkItem>();
            syncWorkItemList.Add(syncWorkItem.Object);

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(o => o.OpenRead(sourceFileInfo.Object)).Returns(new MemoryStream());
            fileSystem.Setup(o => o.OpenWrite(destinationFileInfo.Object)).Returns(new MemoryStream());

            var executeStrategy = new ExecuteStrategy(fileSystem.Object, null);

            // Act
            executeStrategy.Execute(syncWorkItemList);

            // Assert
            fileSystem.Verify(o => o.OpenRead(sourceFileInfo.Object), Times.Exactly(1));
            fileSystem.Verify(o => o.OpenWrite(destinationFileInfo.Object), Times.Exactly(1));
        }

        [Test]
        public void Sync_ShouldCallFileInfoDeleteMethodOnDelete()
        {
            // Arrange
            var sourceFileSystemInfo = new Mock<IFileSystemInfo>();
            sourceFileSystemInfo.Setup(o => o.FullName).Returns(FILE_PATH_LEFT);

            var destinationFileSystemInfo = new Mock<IFileSystemInfo>();
            destinationFileSystemInfo.Setup(o => o.FullName).Returns(FILE_PATH_RIGHT);

            var syncWorkItem = new Mock<ISyncWorkItem>();
            syncWorkItem.Setup(o => o.Source).Returns(sourceFileSystemInfo.Object);
            syncWorkItem.Setup(o => o.Destination).Returns(destinationFileSystemInfo.Object);
            syncWorkItem.Setup(o => o.FileAction).Returns(FileActions.DELETE);

            List<ISyncWorkItem> syncWorkItemList = new List<ISyncWorkItem>();
            syncWorkItemList.Add(syncWorkItem.Object);

            var fileSystem = new Mock<IFileSystem>();
            var executeStrategy = new ExecuteStrategy(fileSystem.Object, null);

            // Act
            executeStrategy.Execute(syncWorkItemList);

            // Assert
            fileSystem.Verify(o => o.Delete(It.IsAny<IFileSystemInfo>()), Times.Exactly(1));
        }

        [Test]
        public void Sync_ShouldDoNothingOnFileActionNone()
        {
            // Arrange
            var sourceFileSystemInfo = new Mock<IFileSystemInfo>();
            sourceFileSystemInfo.Setup(o => o.FullName).Returns(FILE_PATH_LEFT);

            var destinationFileSystemInfo = new Mock<IFileSystemInfo>();
            destinationFileSystemInfo.Setup(o => o.FullName).Returns(FILE_PATH_RIGHT);

            var syncWorkItem = new Mock<ISyncWorkItem>();
            syncWorkItem.Setup(o => o.Source).Returns(sourceFileSystemInfo.Object);
            syncWorkItem.Setup(o => o.Destination).Returns(destinationFileSystemInfo.Object);
            syncWorkItem.Setup(o => o.FileAction).Returns(FileActions.NONE);

            List<ISyncWorkItem> syncWorkItemList = new List<ISyncWorkItem>();
            syncWorkItemList.Add(syncWorkItem.Object);

            var fileSystem = new Mock<IFileSystem>();
            var executeStrategy = new ExecuteStrategy(fileSystem.Object, null);

            // Act
            executeStrategy.Execute(syncWorkItemList);

            // Assert
            fileSystem.Verify(o => o.OpenRead(It.IsAny<IFileInfo>()), Times.Exactly(0));
            fileSystem.Verify(o => o.OpenWrite(It.IsAny<IFileInfo>()), Times.Exactly(0));
            fileSystem.Verify(o => o.Delete(It.IsAny<IFileInfo>()), Times.Exactly(0));
            fileSystem.Verify(o => o.Delete(It.IsAny<IDirectoryInfo>()), Times.Exactly(0));
            fileSystem.Verify(o => o.Delete(It.IsAny<IFileSystemInfo>()), Times.Exactly(0));
        }
    }
}
