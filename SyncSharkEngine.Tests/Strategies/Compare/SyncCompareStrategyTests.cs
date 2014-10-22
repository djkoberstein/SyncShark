using Moq;
using NUnit.Framework;
using SyncShark.Engine.Strategies.Compare;
using SyncShark.Engine.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncShark.Engine.Strategies.DirectorySnapshot;
using SyncShark.Engine.Tests.Strategies.Compare;
using SyncShark.Engine.Tests;
using SyncShark.Interfaces;

namespace SyncSharkEngine.Tests.Compare
{
    [TestFixture]
    public class SyncCompareStrategyTests
    {
        private const string DIRECTORY_PATH_LEFT = @"C:\Folder1";
        private const string DIRECTORY_PATH_RIGHT = @"C:\Folder2";
        private const string FILE_RELATIVE_PATH = @"TestFile.xml";
        private const string FILE_PATH_LEFT = DIRECTORY_PATH_LEFT + @"\" + FILE_RELATIVE_PATH;
        private const string FILE_PATH_RIGHT = DIRECTORY_PATH_RIGHT + @"\" + FILE_RELATIVE_PATH;
        private const string SUBDIRECTORY_RELATIVE_PATH = @"Test\";
        private const string SUBDIRECTORY_PATH_LEFT = DIRECTORY_PATH_LEFT + @"\" + SUBDIRECTORY_RELATIVE_PATH;
        private const string SUBDIRECTORY_PATH_RIGHT = DIRECTORY_PATH_RIGHT + @"\" + SUBDIRECTORY_RELATIVE_PATH;
        private CompareTestArgsFactory m_CompareTestArgsFactory;

        [SetUp]
        public void Setup()
        {
            m_CompareTestArgsFactory = new CompareTestArgsFactory();
        }

        #region File Tests
        [Test]
        public void Compare_ShouldCopyFileLeftToRightWhenAddedToTheLeft()
        {
            // Arrange
            var previousLeftArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_LEFT);
            var previousRightArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_RIGHT);
            var leftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var rightArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_RIGHT);
            var compareService = GetSyncCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

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
            var previousLeftArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_LEFT);
            var previousRightArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_RIGHT);
            var leftArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_LEFT);
            var rightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var compareService = GetSyncCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

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
            var previousLeftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var rightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 1));
            var compareService = GetSyncCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

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
            var previousLeftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 1));
            var rightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var compareService = GetSyncCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

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
            var previousLeftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var rightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var compareService = GetSyncCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

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
            var previousLeftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var rightArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_RIGHT);
            var compareService = GetSyncCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

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
            var previousLeftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_LEFT);
            var rightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var compareService = GetSyncCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(FILE_PATH_LEFT, array[0].Source.FullName);
            Assert.AreEqual(FILE_PATH_RIGHT, array[0].Destination.FullName);
            Assert.AreEqual(FileActions.DELETE, array[0].FileAction);
        }
        #endregion

        #region Directory Tests
        [Test]
        public void Compare_ShouldCopySubDirectoryLeftToRightWhenAddedToTheLeft()
        {
            // Arrange
            var previousLeftArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_LEFT);
            var previousRightArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_RIGHT);
            var leftArgs = m_CompareTestArgsFactory.GetSingleSubDirectory(DIRECTORY_PATH_LEFT, SUBDIRECTORY_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var rightArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_RIGHT);
            var compareService = GetSyncCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            StringAssert.AreEqualIgnoringCase(SUBDIRECTORY_PATH_LEFT, array[0].Source.FullName);
            StringAssert.AreEqualIgnoringCase(SUBDIRECTORY_PATH_RIGHT, array[0].Destination.FullName);
        }

        [Test]
        public void Compare_ShouldCopySubDirectoryRightToLeftWhenAddedToTheRight()
        {
            // Arrange
            var previousLeftArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_LEFT);
            var previousRightArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_RIGHT);
            var leftArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_LEFT);
            var rightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, SUBDIRECTORY_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var compareService = GetSyncCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            StringAssert.AreEqualIgnoringCase(SUBDIRECTORY_PATH_RIGHT, array[0].Source.FullName);
            StringAssert.AreEqualIgnoringCase(SUBDIRECTORY_PATH_LEFT, array[0].Destination.FullName);
        }

        [Test]
        public void Compare_ShouldCopySubDirectoryRightToLeftWhenWhenRightIsNewer()
        {
            // Arrange
            var previousLeftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, SUBDIRECTORY_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, SUBDIRECTORY_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, SUBDIRECTORY_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var rightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, SUBDIRECTORY_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 1));
            var compareService = GetSyncCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            StringAssert.AreEqualIgnoringCase(SUBDIRECTORY_PATH_RIGHT, array[0].Source.FullName);
            StringAssert.AreEqualIgnoringCase(SUBDIRECTORY_PATH_LEFT, array[0].Destination.FullName);
        }

        [Test]
        public void Compare_ShouldCopySubDirectoryLeftToRightWhenWhenLeftIsNewer()
        {
            // Arrange
            var previousLeftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, SUBDIRECTORY_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, SUBDIRECTORY_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, SUBDIRECTORY_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 1));
            var rightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, SUBDIRECTORY_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var compareService = GetSyncCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            StringAssert.AreEqualIgnoringCase(SUBDIRECTORY_PATH_LEFT, array[0].Source.FullName);
            StringAssert.AreEqualIgnoringCase(SUBDIRECTORY_PATH_RIGHT, array[0].Destination.FullName);
        }

        [Test]
        public void Compare_ShouldNotCopySubDirectoryTheyHaveTheSameLastWriteTimeUtc()
        {
            // Arrange
            var previousLeftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, SUBDIRECTORY_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, SUBDIRECTORY_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, SUBDIRECTORY_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var rightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, SUBDIRECTORY_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var compareService = GetSyncCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(FileActions.NONE, array[0].FileAction);
        }

        [Test]
        public void Compare_ShouldDeleteLeftSubDirectoryIfDeletedFromRight()
        {
            // Arrange
            var previousLeftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, SUBDIRECTORY_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, SUBDIRECTORY_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, SUBDIRECTORY_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var rightArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_RIGHT);
            var compareService = GetSyncCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(SUBDIRECTORY_PATH_RIGHT, array[0].Source.FullName);
            Assert.AreEqual(SUBDIRECTORY_PATH_LEFT, array[0].Destination.FullName);
            Assert.AreEqual(FileActions.DELETE, array[0].FileAction);
        }

        [Test]
        public void Compare_ShouldDeleteRightSubDirectoryIfDeletedFromLeft()
        {
            // Arrange
            var previousLeftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, SUBDIRECTORY_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, SUBDIRECTORY_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_LEFT);
            var rightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, SUBDIRECTORY_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var compareService = GetSyncCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(SUBDIRECTORY_PATH_LEFT, array[0].Source.FullName);
            Assert.AreEqual(SUBDIRECTORY_PATH_RIGHT, array[0].Destination.FullName);
            Assert.AreEqual(FileActions.DELETE, array[0].FileAction);
        }
        #endregion


        private SyncCompareStrategy GetSyncCompareStrategy(CompareTestArgs previousLeftArgs, CompareTestArgs previousRightArgs, CompareTestArgs leftArgs, CompareTestArgs rightArgs)
        {
            var dictionarySnapShotService = new Mock<IDirectorySnapshotStrategy>();
            dictionarySnapShotService.Setup(o => o.Read(leftArgs.DirectoryInfo.Object)).Returns(previousLeftArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.Read(rightArgs.DirectoryInfo.Object)).Returns(previousRightArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.Update(leftArgs.DirectoryInfo.Object)).Returns(leftArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.Update(rightArgs.DirectoryInfo.Object)).Returns(rightArgs.Snapshot);
            FileSystemInfoFactory fileSystemInfoFactory = new FileSystemInfoFactory();
            return new SyncCompareStrategy(dictionarySnapShotService.Object, fileSystemInfoFactory);
        }
    }
}
