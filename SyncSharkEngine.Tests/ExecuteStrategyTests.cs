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
            var executeStrategy = new ExecuteStrategy(compareService.Object, directoryInfo.Object, directoryInfo.Object);

            // Act
            executeStrategy.Compare();

            // Assert
            compareService.Verify(o => o.Compare(It.IsAny<IDirectoryInfo>(), It.IsAny<IDirectoryInfo>()), Times.Exactly(1));
        }

        [Test]
        public void Sync_ShouldCallFileInfoReadWriteMethodOnCopy()
        {
            // Arrange
            var sourceFileInfo = new Mock<IFileInfo>();
            sourceFileInfo.Setup(o => o.FullName).Returns(FILE_PATH_LEFT);
            sourceFileInfo.Setup(o => o.OpenRead()).Returns(new MemoryStream());

            var destinationFileInfo = new Mock<IFileInfo>();
            destinationFileInfo.Setup(o => o.FullName).Returns(FILE_PATH_RIGHT);
            destinationFileInfo.Setup(o => o.OpenWrite()).Returns(new MemoryStream());

            var syncWorkItem = new Mock<ISyncWorkItem>();
            syncWorkItem.Setup(o => o.Source).Returns(sourceFileInfo.Object);
            syncWorkItem.Setup(o => o.Destination).Returns(destinationFileInfo.Object);
            syncWorkItem.Setup(o => o.FileAction).Returns(FileActions.COPY);

            List<ISyncWorkItem> syncWorkItemList = new List<ISyncWorkItem>();
            syncWorkItemList.Add(syncWorkItem.Object);

            var executeStrategy = new ExecuteStrategy(null, null, null);

            // Act
            executeStrategy.Execute(syncWorkItemList);

            // Assert
            sourceFileInfo.Verify(o => o.OpenRead(), Times.Exactly(1));
            destinationFileInfo.Verify(o => o.OpenWrite(), Times.Exactly(1));
        }

        [Test]
        public void Sync_ShouldCallFileInfoDeleteMethodOnDelete()
        {
            // Arrange
            var sourceFileInfo = new Mock<IFileInfo>();
            sourceFileInfo.Setup(o => o.FullName).Returns(FILE_PATH_LEFT);
            sourceFileInfo.Setup(o => o.OpenRead()).Returns(new MemoryStream());

            var destinationFileInfo = new Mock<IFileInfo>();
            destinationFileInfo.Setup(o => o.FullName).Returns(FILE_PATH_RIGHT);
            destinationFileInfo.Setup(o => o.OpenWrite()).Returns(new MemoryStream());

            var syncWorkItem = new Mock<ISyncWorkItem>();
            syncWorkItem.Setup(o => o.Source).Returns(sourceFileInfo.Object);
            syncWorkItem.Setup(o => o.Destination).Returns(destinationFileInfo.Object);
            syncWorkItem.Setup(o => o.FileAction).Returns(FileActions.DELETE);

            List<ISyncWorkItem> syncWorkItemList = new List<ISyncWorkItem>();
            syncWorkItemList.Add(syncWorkItem.Object);

            var executeStrategy = new ExecuteStrategy(null, null, null);

            // Act
            executeStrategy.Execute(syncWorkItemList);

            // Assert
            destinationFileInfo.Verify(o => o.Delete(), Times.Exactly(1));
        }

        [Test]
        public void Sync_ShouldDoNothingOnFileActionNone()
        {
            // Arrange
            var sourceFileInfo = new Mock<IFileInfo>();
            sourceFileInfo.Setup(o => o.FullName).Returns(FILE_PATH_LEFT);
            sourceFileInfo.Setup(o => o.OpenRead()).Returns(new MemoryStream());

            var destinationFileInfo = new Mock<IFileInfo>();
            destinationFileInfo.Setup(o => o.FullName).Returns(FILE_PATH_RIGHT);
            destinationFileInfo.Setup(o => o.OpenWrite()).Returns(new MemoryStream());

            var syncWorkItem = new Mock<ISyncWorkItem>();
            syncWorkItem.Setup(o => o.Source).Returns(sourceFileInfo.Object);
            syncWorkItem.Setup(o => o.Destination).Returns(destinationFileInfo.Object);
            syncWorkItem.Setup(o => o.FileAction).Returns(FileActions.DELETE);

            List<ISyncWorkItem> syncWorkItemList = new List<ISyncWorkItem>();
            syncWorkItemList.Add(syncWorkItem.Object);

            var executeStrategy = new ExecuteStrategy(null, null, null);

            // Act
            executeStrategy.Execute(syncWorkItemList);

            // Assert
            sourceFileInfo.Verify(o => o.OpenRead(), Times.Exactly(0));
            sourceFileInfo.Verify(o => o.OpenWrite(), Times.Exactly(0));
            sourceFileInfo.Verify(o => o.Delete(), Times.Exactly(0));
        }
    }
}
