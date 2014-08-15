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
            var compareStrategy = new Mock<ICompareStrategy>();
            var directoryInfo = new Mock<IDirectoryInfo>();
            var executeStrategy = new ExecuteStrategy(compareStrategy.Object);

            // Act
            executeStrategy.Compare(directoryInfo.Object, directoryInfo.Object);

            // Assert
            compareStrategy.Verify(o => o.Compare(It.IsAny<IDirectoryInfo>(), It.IsAny<IDirectoryInfo>()), Times.Exactly(1));
        }

        private List<ISyncWorkItem> GetSyncWorkItemList(FileActions action, out Mock<IFileInfo> source, out Mock<IFileInfo> destination)
        {
            source = new Mock<IFileInfo>();
            source.Setup(o => o.FullName).Returns(FILE_PATH_LEFT);
            source.Setup(o => o.OpenRead()).Returns(new MemoryStream());
            source.Setup(o => o.OpenWrite()).Returns(new MemoryStream());

            destination = new Mock<IFileInfo>();
            destination.Setup(o => o.FullName).Returns(FILE_PATH_RIGHT);
            destination.Setup(o => o.OpenRead()).Returns(new MemoryStream());
            destination.Setup(o => o.OpenWrite()).Returns(new MemoryStream());

            var syncWorkItem = new Mock<ISyncWorkItem>();
            syncWorkItem.Setup(o => o.Source).Returns(source.Object);
            syncWorkItem.Setup(o => o.Destination).Returns(destination.Object);
            syncWorkItem.Setup(o => o.FileAction).Returns(action);

            var syncWorkItemList = new List<ISyncWorkItem>();
            syncWorkItemList.Add(syncWorkItem.Object);
            return syncWorkItemList;
        }

        [Test]
        public void Sync_ShouldCallFileInfoReadWriteMethodOnCopy()
        {
            // Arrange
            Mock<IFileInfo> sourceFileInfo = null;
            Mock<IFileInfo> destinationFileInfo = null;
            List<ISyncWorkItem> syncWorkItemList = GetSyncWorkItemList(FileActions.COPY, out sourceFileInfo, out destinationFileInfo);
            
            var executeStrategy = new ExecuteStrategy(null);

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
            Mock<IFileInfo> sourceFileInfo = null;
            Mock<IFileInfo> destinationFileInfo = null;
            List<ISyncWorkItem> syncWorkItemList = GetSyncWorkItemList(FileActions.DELETE, out sourceFileInfo, out destinationFileInfo);

            var executeStrategy = new ExecuteStrategy(null);

            // Act
            executeStrategy.Execute(syncWorkItemList);

            // Assert
            sourceFileInfo.Verify(o => o.Delete(), Times.Exactly(0));
            destinationFileInfo.Verify(o => o.Delete(), Times.Exactly(1));
        }

        [Test]
        public void Sync_ShouldDoNothingOnFileActionNone()
        {
            // Arrange
            Mock<IFileInfo> sourceFileInfo = null;
            Mock<IFileInfo> destinationFileInfo = null;
            List<ISyncWorkItem> syncWorkItemList = GetSyncWorkItemList(FileActions.NONE, out sourceFileInfo, out destinationFileInfo);

            var executeStrategy = new ExecuteStrategy(null);

            // Act
            executeStrategy.Execute(syncWorkItemList);

            // Assert
            sourceFileInfo.Verify(o => o.OpenRead(), Times.Exactly(0));
            sourceFileInfo.Verify(o => o.OpenWrite(), Times.Exactly(0));
            sourceFileInfo.Verify(o => o.Delete(), Times.Exactly(0));
            destinationFileInfo.Verify(o => o.OpenRead(), Times.Exactly(0));
            destinationFileInfo.Verify(o => o.OpenWrite(), Times.Exactly(0));
            destinationFileInfo.Verify(o => o.Delete(), Times.Exactly(0));
        }
    }
}
