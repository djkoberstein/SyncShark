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
using SyncShark.Interfaces;

namespace SyncShark.Engine.Tests.Compare
{
    [TestFixture]
    class MirrorCompareStrategyTests
    {
        private const string DIRECTORY_PATH_LEFT = @"C:\Folder1";
        private const string DIRECTORY_PATH_RIGHT = @"C:\Folder2";
        private const string FILE_RELATIVE_PATH = @"TestFile.xml";
        private const string FILE_PATH_LEFT = DIRECTORY_PATH_LEFT + @"\" + FILE_RELATIVE_PATH;
        private const string FILE_PATH_RIGHT = DIRECTORY_PATH_RIGHT + @"\" + FILE_RELATIVE_PATH;
        private CompareTestArgsFactory m_CompareTestArgsFactory;

        [SetUp]
        public void Setup()
        {
            m_CompareTestArgsFactory = new CompareTestArgsFactory();
        }

        private MirrorCompareStrategy GetMirrorCompareStrategy(CompareTestArgs previousLeftArgs, CompareTestArgs previousRightArgs, CompareTestArgs leftArgs, CompareTestArgs rightArgs)
        {
            var dictionarySnapShotService = new Mock<IDirectorySnapshotStrategy>();
            dictionarySnapShotService.Setup(o => o.Read(leftArgs.DirectoryInfo.Object)).Returns(previousLeftArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.Read(rightArgs.DirectoryInfo.Object)).Returns(previousRightArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.Update(leftArgs.DirectoryInfo.Object)).Returns(leftArgs.Snapshot);
            dictionarySnapShotService.Setup(o => o.Update(rightArgs.DirectoryInfo.Object)).Returns(rightArgs.Snapshot);
            FileSystemInfoFactory fileSystemInfoFactory = new FileSystemInfoFactory();
            return new MirrorCompareStrategy(dictionarySnapShotService.Object, fileSystemInfoFactory);
        }

        [Test]
        public void Compare_ShouldCopyFileLeftToRightWhenAddedToTheLeft()
        {
            // Arrange
            var previousLeftArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_LEFT);
            var previousRightArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_RIGHT);
            var leftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var rightArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_RIGHT);
            var compareService = GetMirrorCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            StringAssert.AreEqualIgnoringCase(FILE_PATH_LEFT, array[0].Source.FullName);
            StringAssert.AreEqualIgnoringCase(FILE_PATH_RIGHT, array[0].Destination.FullName);
            Assert.AreEqual(FileActions.COPY, array[0].FileAction);
        }

        [Test]
        public void Compare_ShouldDeleteFileOnRightWhenAddedToTheRight()
        {
            // Arrange
            var previousLeftArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_LEFT);
            var previousRightArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_RIGHT);
            var leftArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_LEFT);
            var rightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var compareService = GetMirrorCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            StringAssert.AreEqualIgnoringCase(FILE_PATH_LEFT, array[0].Source.FullName);
            StringAssert.AreEqualIgnoringCase(FILE_PATH_RIGHT, array[0].Destination.FullName);
            Assert.AreEqual(FileActions.DELETE, array[0].FileAction);
        }

        [Test]
        public void Compare_ShouldCopyFileLeftToRightWhenRightIsNewer()
        {
            // Arrange
            var previousLeftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var rightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 1));
            var compareService = GetMirrorCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            StringAssert.AreEqualIgnoringCase(FILE_PATH_LEFT, array[0].Source.FullName);
            StringAssert.AreEqualIgnoringCase(FILE_PATH_RIGHT, array[0].Destination.FullName);
            Assert.AreEqual(FileActions.COPY, array[0].FileAction);
        }

        [Test]
        public void Compare_ShouldCopyFileLeftToRightWhenWhenLeftIsNewer()
        {
            // Arrange
            var previousLeftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 1));
            var rightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var compareService = GetMirrorCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            StringAssert.AreEqualIgnoringCase(FILE_PATH_LEFT, array[0].Source.FullName);
            StringAssert.AreEqualIgnoringCase(FILE_PATH_RIGHT, array[0].Destination.FullName);
            Assert.AreEqual(FileActions.COPY, array[0].FileAction);
        }

        [Test]
        public void Compare_ShouldDoNothingWhenTheFilesHaveTheSameLastWriteTimeUtc()
        {
            // Arrange
            var previousLeftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var rightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var compareService = GetMirrorCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(FileActions.NONE, array[0].FileAction);
        }

        [Test]
        public void Compare_ShouldCopyFileLeftToRightIfDeletedFromRight()
        {
            // Arrange
            var previousLeftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var rightArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_RIGHT);
            var compareService = GetMirrorCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(FILE_PATH_LEFT, array[0].Source.FullName);
            Assert.AreEqual(FILE_PATH_RIGHT, array[0].Destination.FullName);
            Assert.AreEqual(FileActions.COPY, array[0].FileAction);
        }

        [Test]
        public void Compare_ShouldDeleteRightFileIfDeletedFromLeft()
        {
            // Arrange
            var previousLeftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = m_CompareTestArgsFactory.GetEmptyDirectory(DIRECTORY_PATH_LEFT);
            var rightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var compareService = GetMirrorCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(FILE_PATH_LEFT, array[0].Source.FullName);
            Assert.AreEqual(FILE_PATH_RIGHT, array[0].Destination.FullName);
            Assert.AreEqual(FileActions.DELETE, array[0].FileAction);
        }

        [Test]
        public void Compare_ShouldMoveRightFileIfMovedOnLeft()
        {
            // Arrange
            var previousLeftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH + ".obj", new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var rightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var compareService = GetMirrorCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(FILE_PATH_RIGHT, array[0].Source.FullName);
            Assert.AreEqual(FILE_PATH_RIGHT + ".obj", array[0].Destination.FullName);
            Assert.AreEqual(FileActions.MOVE, array[0].FileAction);
        }

        [Test]
        public void Compare_ShouldMoveLeftFileIfMovedOnRight()
        {
            // Arrange
            var previousLeftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var previousRightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var leftArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_LEFT, FILE_RELATIVE_PATH, new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var rightArgs = m_CompareTestArgsFactory.GetSingleFile(DIRECTORY_PATH_RIGHT, FILE_RELATIVE_PATH + ".obj", new DateTime(2014, 01, 01, 0, 0, 0, 0));
            var compareService = GetMirrorCompareStrategy(previousLeftArgs, previousRightArgs, leftArgs, rightArgs);

            // Act
            var result = compareService.Compare(leftArgs.DirectoryInfo.Object, rightArgs.DirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(FILE_PATH_LEFT, array[0].Source.FullName);
            Assert.AreEqual(FILE_PATH_LEFT + ".obj", array[0].Destination.FullName);
            Assert.AreEqual(FileActions.MOVE, array[0].FileAction);
        }
    }
}
