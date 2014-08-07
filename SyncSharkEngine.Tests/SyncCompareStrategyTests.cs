using Moq;
using NUnit.Framework;
using SyncSharkEngine.Strategies.Compare;
using SyncSharkEngine.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncSharkEngine.Strategies.DirectorySnapshot;

namespace SyncSharkEngine.Tests
{
    [TestFixture]
    public class SyncCompareStrategyTests
    {
        private const string DIRECTORY_PATH_LEFT = @"C:\Folder1";
        private const string DIRECTORY_PATH_RIGHT = @"C:\Folder2";
        private const string FILE_RELATIVE_PATH = @"TestFile.xml";
        private const string FILE_PATH_LEFT = DIRECTORY_PATH_LEFT + @"\" + FILE_RELATIVE_PATH;
        private const string FILE_PATH_RIGHT = DIRECTORY_PATH_RIGHT + @"\" + FILE_RELATIVE_PATH;

        [Test]
        public void Compare_ShouldCopyFileLeftToRightWhenAddedToTheLeft()
        {
            // Arrange
            var previousLeftArgs = new CompareTestArgs(DIRECTORY_PATH_LEFT);
            var previousRightArgs = new CompareTestArgs(DIRECTORY_PATH_RIGHT);
            var leftArgs = new CompareTestArgs(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var rightArgs = new CompareTestArgs(DIRECTORY_PATH_RIGHT);

            var dictionarySnapShotService = new Mock<IDirectorySnapshotStrategy>();
            dictionarySnapShotService.Setup(o => o.Exists(It.IsAny<IDirectoryInfo>())).Returns(true);
            dictionarySnapShotService.Setup(o => o.ReadOrCreate(leftArgs.DirectoryInfo.Object)).Returns(previousLeftArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.ReadOrCreate(rightArgs.DirectoryInfo.Object)).Returns(previousRightArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.Update(leftArgs.DirectoryInfo.Object)).Returns(leftArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.Update(rightArgs.DirectoryInfo.Object)).Returns(rightArgs.Snapshot);

            var compareService = new SyncCompareStrategy(dictionarySnapShotService.Object);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            StringAssert.AreEqualIgnoringCase(FILE_PATH_LEFT, array[0].Source.FullName);
            StringAssert.AreEqualIgnoringCase(FILE_PATH_RIGHT, array[0].Destination.FullName);
        }

        [Test]
        public void Compare_ShouldCopyFileRightToLeftWhenAddedToTheRight()
        {
            // Arrange
            var previousLeftArgs = new CompareTestArgs(DIRECTORY_PATH_LEFT);
            var previousRightArgs = new CompareTestArgs(DIRECTORY_PATH_RIGHT);
            var leftArgs = new CompareTestArgs(DIRECTORY_PATH_LEFT);
            var rightArgs = new CompareTestArgs(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));

            var dictionarySnapShotService = new Mock<IDirectorySnapshotStrategy>();
            dictionarySnapShotService.Setup(o => o.Exists(It.IsAny<IDirectoryInfo>())).Returns(true);
            dictionarySnapShotService.Setup(o => o.ReadOrCreate(leftArgs.DirectoryInfo.Object)).Returns(previousLeftArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.ReadOrCreate(rightArgs.DirectoryInfo.Object)).Returns(previousRightArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.Update(leftArgs.DirectoryInfo.Object)).Returns(leftArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.Update(rightArgs.DirectoryInfo.Object)).Returns(rightArgs.Snapshot);

            var compareService = new SyncCompareStrategy(dictionarySnapShotService.Object);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            StringAssert.AreEqualIgnoringCase(FILE_PATH_RIGHT, array[0].Source.FullName);
            StringAssert.AreEqualIgnoringCase(FILE_PATH_LEFT, array[0].Destination.FullName);
        }

        [Test]
        public void Compare_ShouldCopyFileRightToLeftWhenWhenRightIsNewer()
        {
            // Arrange
            var previousLeftArgs = new CompareTestArgs(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = new CompareTestArgs(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = new CompareTestArgs(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var rightArgs = new CompareTestArgs(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 1));

            var dictionarySnapShotService = new Mock<IDirectorySnapshotStrategy>();
            dictionarySnapShotService.Setup(o => o.Exists(It.IsAny<IDirectoryInfo>())).Returns(true);
            dictionarySnapShotService.Setup(o => o.ReadOrCreate(leftArgs.DirectoryInfo.Object)).Returns(previousLeftArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.ReadOrCreate(rightArgs.DirectoryInfo.Object)).Returns(previousRightArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.Update(leftArgs.DirectoryInfo.Object)).Returns(leftArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.Update(rightArgs.DirectoryInfo.Object)).Returns(rightArgs.Snapshot);

            var compareService = new SyncCompareStrategy(dictionarySnapShotService.Object);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            StringAssert.AreEqualIgnoringCase(FILE_PATH_RIGHT, array[0].Source.FullName);
            StringAssert.AreEqualIgnoringCase(FILE_PATH_LEFT, array[0].Destination.FullName);
        }

        [Test]
        public void Compare_ShouldCopyFileLeftToRightWhenWhenLeftIsNewer()
        {
            // Arrange
            var previousLeftArgs = new CompareTestArgs(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = new CompareTestArgs(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = new CompareTestArgs(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 1));
            var rightArgs = new CompareTestArgs(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));

            var dictionarySnapShotService = new Mock<IDirectorySnapshotStrategy>();
            dictionarySnapShotService.Setup(o => o.Exists(It.IsAny<IDirectoryInfo>())).Returns(true);
            dictionarySnapShotService.Setup(o => o.ReadOrCreate(leftArgs.DirectoryInfo.Object)).Returns(previousLeftArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.ReadOrCreate(rightArgs.DirectoryInfo.Object)).Returns(previousRightArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.Update(leftArgs.DirectoryInfo.Object)).Returns(leftArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.Update(rightArgs.DirectoryInfo.Object)).Returns(rightArgs.Snapshot);

            var compareService = new SyncCompareStrategy(dictionarySnapShotService.Object);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            StringAssert.AreEqualIgnoringCase(FILE_PATH_LEFT, array[0].Source.FullName);
            StringAssert.AreEqualIgnoringCase(FILE_PATH_RIGHT, array[0].Destination.FullName);
        }

        [Test]
        public void Compare_ShouldNotCopyFilesTheyHaveTheSameLastWriteTimeUtc()
        {
            // Arrange
            var previousLeftArgs = new CompareTestArgs(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = new CompareTestArgs(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = new CompareTestArgs(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var rightArgs = new CompareTestArgs(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));

            var dictionarySnapShotService = new Mock<IDirectorySnapshotStrategy>();
            dictionarySnapShotService.Setup(o => o.Exists(It.IsAny<IDirectoryInfo>())).Returns(true);
            dictionarySnapShotService.Setup(o => o.ReadOrCreate(leftArgs.DirectoryInfo.Object)).Returns(previousLeftArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.ReadOrCreate(rightArgs.DirectoryInfo.Object)).Returns(previousRightArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.Update(leftArgs.DirectoryInfo.Object)).Returns(leftArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.Update(rightArgs.DirectoryInfo.Object)).Returns(rightArgs.Snapshot);

            var compareService = new SyncCompareStrategy(dictionarySnapShotService.Object);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(FileActions.NONE, array[0].FileAction);
        }

        [Test]
        public void Compare_ShouldDeleteLeftFileIfDeletedFromRight()
        {
            // Arrange
            var previousLeftArgs = new CompareTestArgs(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = new CompareTestArgs(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = new CompareTestArgs(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var rightArgs = new CompareTestArgs(DIRECTORY_PATH_RIGHT);

            var dictionarySnapShotService = new Mock<IDirectorySnapshotStrategy>();
            dictionarySnapShotService.Setup(o => o.Exists(It.IsAny<IDirectoryInfo>())).Returns(true);
            dictionarySnapShotService.Setup(o => o.ReadOrCreate(leftArgs.DirectoryInfo.Object)).Returns(previousLeftArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.ReadOrCreate(rightArgs.DirectoryInfo.Object)).Returns(previousRightArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.Update(leftArgs.DirectoryInfo.Object)).Returns(leftArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.Update(rightArgs.DirectoryInfo.Object)).Returns(rightArgs.Snapshot);

            var compareService = new SyncCompareStrategy(dictionarySnapShotService.Object);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(FILE_PATH_RIGHT, array[0].Source.FullName);
            Assert.AreEqual(FILE_PATH_LEFT, array[0].Destination.FullName);
            Assert.AreEqual(FileActions.DELETE, array[0].FileAction);
        }

        [Test]
        public void Compare_ShouldDeleteRightFileIfDeletedFromLeft()
        {
            // Arrange
            var previousLeftArgs = new CompareTestArgs(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = new CompareTestArgs(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = new CompareTestArgs(DIRECTORY_PATH_LEFT);
            var rightArgs = new CompareTestArgs(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));

            var dictionarySnapShotService = new Mock<IDirectorySnapshotStrategy>();
            dictionarySnapShotService.Setup(o => o.Exists(It.IsAny<IDirectoryInfo>())).Returns(true);
            dictionarySnapShotService.Setup(o => o.ReadOrCreate(leftArgs.DirectoryInfo.Object)).Returns(previousLeftArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.ReadOrCreate(rightArgs.DirectoryInfo.Object)).Returns(previousRightArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.Update(leftArgs.DirectoryInfo.Object)).Returns(leftArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.Update(rightArgs.DirectoryInfo.Object)).Returns(rightArgs.Snapshot);

            var compareService = new SyncCompareStrategy(dictionarySnapShotService.Object);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(FILE_PATH_LEFT, array[0].Source.FullName);
            Assert.AreEqual(FILE_PATH_RIGHT, array[0].Destination.FullName);
            Assert.AreEqual(FileActions.DELETE, array[0].FileAction);
        }
    }
}
