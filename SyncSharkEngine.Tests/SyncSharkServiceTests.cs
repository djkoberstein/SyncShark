using Moq;
using NUnit.Framework;
using SyncSharkEngine.DataModel;
using SyncSharkEngine.Factories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.Tests
{
    [TestFixture]
    public class SyncSharkServiceTests
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
            var leftDirfileInfo = new Mock<IFileInfo>();
            leftDirfileInfo.Setup(o => o.FullName).Returns(FILE_PATH_LEFT);
            leftDirfileInfo.Setup(o => o.LastWriteTimeUtc).Returns(new DateTime(2014, 01, 01, 0, 0, 0, 0));
            IFileInfo[] leftFileInfos = new IFileInfo[1];
            leftFileInfos[0] = leftDirfileInfo.Object;

            IFileInfo[] rightFileInfos = new IFileInfo[0];

            var leftDirectoryInfo = new Mock<IDirectoryInfo>();
            leftDirectoryInfo.Setup(o => o.FullName).Returns(DIRECTORY_PATH_LEFT);
            leftDirectoryInfo.Setup(o => o.GetFiles("*.*", It.IsAny<SearchOption>())).Returns(leftFileInfos);

            var rightDirectoryInfo = new Mock<IDirectoryInfo>();
            rightDirectoryInfo.Setup(o => o.FullName).Returns(DIRECTORY_PATH_RIGHT);
            rightDirectoryInfo.Setup(o => o.GetFiles("*.*", It.IsAny<SearchOption>())).Returns(rightFileInfos);

            var previousLeftDictionarySnapshot = new Dictionary<string, IFileInfo>();
            var previousRightDictionarySnapshot = new Dictionary<string, IFileInfo>();

            var leftDictionarySnapshot = new Dictionary<string, IFileInfo>();
            leftDictionarySnapshot.Add(FILE_RELATIVE_PATH, leftDirfileInfo.Object);
            var rightDictionarySnapshot = new Dictionary<string, IFileInfo>();

            var dictionarySnapShotService = new Mock<IDirectorySnapshotService>();
            dictionarySnapShotService.Setup(o => o.Exists(It.IsAny<IDirectoryInfo>())).Returns(true);
            dictionarySnapShotService.Setup(o => o.Read(leftDirectoryInfo.Object)).Returns(previousLeftDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Read(rightDirectoryInfo.Object)).Returns(previousRightDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Update(leftDirectoryInfo.Object)).Returns(leftDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Update(rightDirectoryInfo.Object)).Returns(rightDictionarySnapshot);

            var directorySnapshotServiceFactory = new Mock<IDirectorySnapshotServiceFactory>();
            directorySnapshotServiceFactory.Setup(o => o.GetDirectorySnapshotService(leftDirectoryInfo.Object, rightDirectoryInfo.Object)).Returns(dictionarySnapShotService.Object);
            
            var syncSharkService = new SyncSharkService(directorySnapshotServiceFactory.Object);

            // Act
            var result = syncSharkService.Compare(leftDirectoryInfo.Object, rightDirectoryInfo.Object);

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
            IFileInfo[] leftFileInfos = new IFileInfo[0];
            var fileInfo = new Mock<IFileInfo>();
            fileInfo.Setup(o => o.FullName).Returns(FILE_PATH_RIGHT);
            fileInfo.Setup(o => o.LastWriteTimeUtc).Returns(new DateTime(2014, 01, 01, 0, 0, 0, 0));
            IFileInfo[] rightFileInfos = new IFileInfo[1];
            rightFileInfos[0] = fileInfo.Object;


            var previousLeftDictionarySnapshot = new Dictionary<string, IFileInfo>();
            var previousRightDictionarySnapshot = new Dictionary<string, IFileInfo>();

            var leftDirectoryInfo = new Mock<IDirectoryInfo>();
            leftDirectoryInfo.Setup(o => o.FullName).Returns(DIRECTORY_PATH_LEFT);
            leftDirectoryInfo.Setup(o => o.GetFiles("*.*", It.IsAny<SearchOption>())).Returns(leftFileInfos);

            var rightDirectoryInfo = new Mock<IDirectoryInfo>();
            rightDirectoryInfo.Setup(o => o.FullName).Returns(DIRECTORY_PATH_RIGHT);
            rightDirectoryInfo.Setup(o => o.GetFiles("*.*", It.IsAny<SearchOption>())).Returns(rightFileInfos);

            var leftDictionarySnapshot = new Dictionary<string, IFileInfo>();
            var rightDictionarySnapshot = new Dictionary<string, IFileInfo>();
            rightDictionarySnapshot.Add(FILE_RELATIVE_PATH, fileInfo.Object);

            var dictionarySnapShotService = new Mock<IDirectorySnapshotService>();
            dictionarySnapShotService.Setup(o => o.Exists(It.IsAny<IDirectoryInfo>())).Returns(true);
            dictionarySnapShotService.Setup(o => o.Read(leftDirectoryInfo.Object)).Returns(previousLeftDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Read(rightDirectoryInfo.Object)).Returns(previousRightDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Update(leftDirectoryInfo.Object)).Returns(leftDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Update(rightDirectoryInfo.Object)).Returns(rightDictionarySnapshot);

            var directorySnapshotServiceFactory = new Mock<IDirectorySnapshotServiceFactory>();
            directorySnapshotServiceFactory.Setup(o => o.GetDirectorySnapshotService(leftDirectoryInfo.Object, rightDirectoryInfo.Object)).Returns(dictionarySnapShotService.Object);

            var syncSharkService = new SyncSharkService(directorySnapshotServiceFactory.Object);

            // Act
            var result = syncSharkService.Compare(leftDirectoryInfo.Object, rightDirectoryInfo.Object);

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
            var leftDirfileInfo = new Mock<IFileInfo>();
            leftDirfileInfo.Setup(o => o.FullName).Returns(FILE_PATH_LEFT);
            leftDirfileInfo.Setup(o => o.LastWriteTimeUtc).Returns(new DateTime(2014, 01, 01, 0, 0, 0, 0));
            IFileInfo[] leftFileInfos = new IFileInfo[1];
            leftFileInfos[0] = leftDirfileInfo.Object;

            var rightDirFileInfo = new Mock<IFileInfo>();
            rightDirFileInfo.Setup(o => o.FullName).Returns(FILE_PATH_RIGHT);
            rightDirFileInfo.Setup(o => o.LastWriteTimeUtc).Returns(new DateTime(2014, 01, 01, 0, 0, 0, 1));
            IFileInfo[] rightFileInfos = new IFileInfo[1];
            rightFileInfos[0] = rightDirFileInfo.Object;

            var leftDirectoryInfo = new Mock<IDirectoryInfo>();
            leftDirectoryInfo.Setup(o => o.FullName).Returns(DIRECTORY_PATH_LEFT);
            leftDirectoryInfo.Setup(o => o.GetFiles("*.*", It.IsAny<SearchOption>())).Returns(leftFileInfos);

            var rightDirectoryInfo = new Mock<IDirectoryInfo>();
            rightDirectoryInfo.Setup(o => o.FullName).Returns(DIRECTORY_PATH_RIGHT);
            rightDirectoryInfo.Setup(o => o.GetFiles("*.*", It.IsAny<SearchOption>())).Returns(rightFileInfos);

            var leftDictionarySnapshot = new Dictionary<string, IFileInfo>();
            leftDictionarySnapshot.Add(FILE_RELATIVE_PATH, leftDirfileInfo.Object);
            var rightDictionarySnapshot = new Dictionary<string, IFileInfo>();
            rightDictionarySnapshot.Add(FILE_RELATIVE_PATH, rightDirFileInfo.Object);

            var dictionarySnapShotService = new Mock<IDirectorySnapshotService>();
            dictionarySnapShotService.Setup(o => o.Exists(It.IsAny<IDirectoryInfo>())).Returns(true);
            dictionarySnapShotService.Setup(o => o.Read(leftDirectoryInfo.Object)).Returns(leftDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Read(rightDirectoryInfo.Object)).Returns(rightDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Update(leftDirectoryInfo.Object)).Returns(leftDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Update(rightDirectoryInfo.Object)).Returns(rightDictionarySnapshot);

            var directorySnapshotServiceFactory = new Mock<IDirectorySnapshotServiceFactory>();
            directorySnapshotServiceFactory.Setup(o => o.GetDirectorySnapshotService(leftDirectoryInfo.Object, rightDirectoryInfo.Object)).Returns(dictionarySnapShotService.Object);

            var syncSharkService = new SyncSharkService(directorySnapshotServiceFactory.Object);

            // Act
            var result = syncSharkService.Compare(leftDirectoryInfo.Object, rightDirectoryInfo.Object);

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
            var leftDirfileInfo = new Mock<IFileInfo>();
            leftDirfileInfo.Setup(o => o.FullName).Returns(FILE_PATH_LEFT);
            leftDirfileInfo.Setup(o => o.LastWriteTimeUtc).Returns(new DateTime(2014, 01, 01, 0, 0, 0, 1));
            IFileInfo[] leftFileInfos = new IFileInfo[1];
            leftFileInfos[0] = leftDirfileInfo.Object;

            var rightDirFileInfo = new Mock<IFileInfo>();
            rightDirFileInfo.Setup(o => o.FullName).Returns(FILE_PATH_RIGHT);
            rightDirFileInfo.Setup(o => o.LastWriteTimeUtc).Returns(new DateTime(2014, 01, 01, 0, 0, 0, 0));
            IFileInfo[] rightFileInfos = new IFileInfo[1];
            rightFileInfos[0] = rightDirFileInfo.Object;

            var leftDirectoryInfo = new Mock<IDirectoryInfo>();
            leftDirectoryInfo.Setup(o => o.FullName).Returns(DIRECTORY_PATH_LEFT);
            leftDirectoryInfo.Setup(o => o.GetFiles("*.*", It.IsAny<SearchOption>())).Returns(leftFileInfos);

            var rightDirectoryInfo = new Mock<IDirectoryInfo>();
            rightDirectoryInfo.Setup(o => o.FullName).Returns(DIRECTORY_PATH_RIGHT);
            rightDirectoryInfo.Setup(o => o.GetFiles("*.*", It.IsAny<SearchOption>())).Returns(rightFileInfos);

            var leftDictionarySnapshot = new Dictionary<string, IFileInfo>();
            leftDictionarySnapshot.Add(FILE_RELATIVE_PATH, leftDirfileInfo.Object);
            var rightDictionarySnapshot = new Dictionary<string, IFileInfo>();
            rightDictionarySnapshot.Add(FILE_RELATIVE_PATH, rightDirFileInfo.Object);

            var dictionarySnapShotService = new Mock<IDirectorySnapshotService>();
            dictionarySnapShotService.Setup(o => o.Exists(It.IsAny<IDirectoryInfo>())).Returns(true);
            dictionarySnapShotService.Setup(o => o.Read(leftDirectoryInfo.Object)).Returns(leftDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Read(rightDirectoryInfo.Object)).Returns(rightDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Update(leftDirectoryInfo.Object)).Returns(leftDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Update(rightDirectoryInfo.Object)).Returns(rightDictionarySnapshot);

            var directorySnapshotServiceFactory = new Mock<IDirectorySnapshotServiceFactory>();
            directorySnapshotServiceFactory.Setup(o => o.GetDirectorySnapshotService(leftDirectoryInfo.Object, rightDirectoryInfo.Object)).Returns(dictionarySnapShotService.Object);

            var syncSharkService = new SyncSharkService(directorySnapshotServiceFactory.Object);

            // Act
            var result = syncSharkService.Compare(leftDirectoryInfo.Object, rightDirectoryInfo.Object);

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
            var leftDirfileInfo = new Mock<IFileInfo>();
            leftDirfileInfo.Setup(o => o.FullName).Returns(FILE_PATH_LEFT);
            leftDirfileInfo.Setup(o => o.LastWriteTimeUtc).Returns(new DateTime(2014, 01, 01, 0, 0, 0, 0));
            IFileInfo[] leftFileInfos = new IFileInfo[1];
            leftFileInfos[0] = leftDirfileInfo.Object;

            var rightDirFileInfo = new Mock<IFileInfo>();
            rightDirFileInfo.Setup(o => o.FullName).Returns(FILE_PATH_RIGHT);
            rightDirFileInfo.Setup(o => o.LastWriteTimeUtc).Returns(new DateTime(2014, 01, 01, 0, 0, 0, 0));
            IFileInfo[] rightFileInfos = new IFileInfo[1];
            rightFileInfos[0] = rightDirFileInfo.Object;

            var leftDirectoryInfo = new Mock<IDirectoryInfo>();
            leftDirectoryInfo.Setup(o => o.FullName).Returns(DIRECTORY_PATH_LEFT);
            leftDirectoryInfo.Setup(o => o.GetFiles("*.*", It.IsAny<SearchOption>())).Returns(leftFileInfos);

            var rightDirectoryInfo = new Mock<IDirectoryInfo>();
            rightDirectoryInfo.Setup(o => o.FullName).Returns(DIRECTORY_PATH_RIGHT);
            rightDirectoryInfo.Setup(o => o.GetFiles("*.*", It.IsAny<SearchOption>())).Returns(rightFileInfos);

            var leftDictionarySnapshot = new Dictionary<string, IFileInfo>();
            leftDictionarySnapshot.Add(FILE_RELATIVE_PATH, leftDirfileInfo.Object);
            var rightDictionarySnapshot = new Dictionary<string, IFileInfo>();
            rightDictionarySnapshot.Add(FILE_RELATIVE_PATH, rightDirFileInfo.Object);

            var dictionarySnapShotService = new Mock<IDirectorySnapshotService>();
            dictionarySnapShotService.Setup(o => o.Exists(It.IsAny<IDirectoryInfo>())).Returns(true);
            dictionarySnapShotService.Setup(o => o.Read(leftDirectoryInfo.Object)).Returns(leftDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Read(rightDirectoryInfo.Object)).Returns(rightDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Update(leftDirectoryInfo.Object)).Returns(leftDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Update(rightDirectoryInfo.Object)).Returns(rightDictionarySnapshot);

            var directorySnapshotServiceFactory = new Mock<IDirectorySnapshotServiceFactory>();
            directorySnapshotServiceFactory.Setup(o => o.GetDirectorySnapshotService(leftDirectoryInfo.Object, rightDirectoryInfo.Object)).Returns(dictionarySnapShotService.Object);

            var syncSharkService = new SyncSharkService(directorySnapshotServiceFactory.Object);

            // Act
            var result = syncSharkService.Compare(leftDirectoryInfo.Object, rightDirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(FileActions.NONE, array[0].FileAction);
        }

        [Test]
        public void Compare_ShouldDeleteLeftFileIfDeletedFromRight()
        {
            // Arrange
            var leftDirfileInfo = new Mock<IFileInfo>();
            leftDirfileInfo.Setup(o => o.FullName).Returns(FILE_PATH_LEFT);
            leftDirfileInfo.Setup(o => o.LastWriteTimeUtc).Returns(new DateTime(2014, 01, 01, 0, 0, 0, 0));
            IFileInfo[] leftFileInfos = new IFileInfo[1];
            leftFileInfos[0] = leftDirfileInfo.Object;

            var rightDirFileInfo = new Mock<IFileInfo>();
            rightDirFileInfo.Setup(o => o.FullName).Returns(FILE_PATH_RIGHT);
            rightDirFileInfo.Setup(o => o.LastWriteTimeUtc).Returns(new DateTime(2014, 01, 01, 0, 0, 0, 0));
            IFileInfo[] rightFileInfos = new IFileInfo[1];
            rightFileInfos[0] = rightDirFileInfo.Object;

            var leftDirectoryInfo = new Mock<IDirectoryInfo>();
            leftDirectoryInfo.Setup(o => o.FullName).Returns(DIRECTORY_PATH_LEFT);
            leftDirectoryInfo.Setup(o => o.GetFiles("*.*", It.IsAny<SearchOption>())).Returns(leftFileInfos);

            var rightDirectoryInfo = new Mock<IDirectoryInfo>();
            rightDirectoryInfo.Setup(o => o.FullName).Returns(DIRECTORY_PATH_RIGHT);
            rightDirectoryInfo.Setup(o => o.GetFiles("*.*", It.IsAny<SearchOption>())).Returns(rightFileInfos);

            var previousLeftDictionarySnapshot = new Dictionary<string, IFileInfo>();
            previousLeftDictionarySnapshot.Add(FILE_RELATIVE_PATH, leftDirfileInfo.Object);
            var previousRightDictionarySnapshot = new Dictionary<string, IFileInfo>();
            previousRightDictionarySnapshot.Add(FILE_RELATIVE_PATH, rightDirFileInfo.Object);

            var leftDictionarySnapshot = new Dictionary<string, IFileInfo>();
            leftDictionarySnapshot.Add(FILE_RELATIVE_PATH, leftDirfileInfo.Object);
            var rightDictionarySnapshot = new Dictionary<string, IFileInfo>();

            var dictionarySnapShotService = new Mock<IDirectorySnapshotService>();
            dictionarySnapShotService.Setup(o => o.Exists(It.IsAny<IDirectoryInfo>())).Returns(true);
            dictionarySnapShotService.Setup(o => o.Read(leftDirectoryInfo.Object)).Returns(previousLeftDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Read(rightDirectoryInfo.Object)).Returns(previousRightDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Update(leftDirectoryInfo.Object)).Returns(leftDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Update(rightDirectoryInfo.Object)).Returns(rightDictionarySnapshot);

            var directorySnapshotServiceFactory = new Mock<IDirectorySnapshotServiceFactory>();
            directorySnapshotServiceFactory.Setup(o => o.GetDirectorySnapshotService(leftDirectoryInfo.Object, rightDirectoryInfo.Object)).Returns(dictionarySnapShotService.Object);

            var syncSharkService = new SyncSharkService(directorySnapshotServiceFactory.Object);

            // Act
            var result = syncSharkService.Compare(leftDirectoryInfo.Object, rightDirectoryInfo.Object);

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
            var leftDirfileInfo = new Mock<IFileInfo>();
            leftDirfileInfo.Setup(o => o.FullName).Returns(FILE_PATH_LEFT);
            leftDirfileInfo.Setup(o => o.LastWriteTimeUtc).Returns(new DateTime(2014, 01, 01, 0, 0, 0, 0));
            IFileInfo[] leftFileInfos = new IFileInfo[1];
            leftFileInfos[0] = leftDirfileInfo.Object;

            var rightDirFileInfo = new Mock<IFileInfo>();
            rightDirFileInfo.Setup(o => o.FullName).Returns(FILE_PATH_RIGHT);
            rightDirFileInfo.Setup(o => o.LastWriteTimeUtc).Returns(new DateTime(2014, 01, 01, 0, 0, 0, 0));
            IFileInfo[] rightFileInfos = new IFileInfo[1];
            rightFileInfos[0] = rightDirFileInfo.Object;

            var leftDirectoryInfo = new Mock<IDirectoryInfo>();
            leftDirectoryInfo.Setup(o => o.FullName).Returns(DIRECTORY_PATH_LEFT);
            leftDirectoryInfo.Setup(o => o.GetFiles("*.*", It.IsAny<SearchOption>())).Returns(leftFileInfos);

            var rightDirectoryInfo = new Mock<IDirectoryInfo>();
            rightDirectoryInfo.Setup(o => o.FullName).Returns(DIRECTORY_PATH_RIGHT);
            rightDirectoryInfo.Setup(o => o.GetFiles("*.*", It.IsAny<SearchOption>())).Returns(rightFileInfos);

            var previousLeftDictionarySnapshot = new Dictionary<string, IFileInfo>();
            previousLeftDictionarySnapshot.Add(FILE_RELATIVE_PATH, leftDirfileInfo.Object);
            var previousRightDictionarySnapshot = new Dictionary<string, IFileInfo>();
            previousRightDictionarySnapshot.Add(FILE_RELATIVE_PATH, rightDirFileInfo.Object);

            var leftDictionarySnapshot = new Dictionary<string, IFileInfo>();
            var rightDictionarySnapshot = new Dictionary<string, IFileInfo>();
            rightDictionarySnapshot.Add(FILE_RELATIVE_PATH, rightDirFileInfo.Object);

            var dictionarySnapShotService = new Mock<IDirectorySnapshotService>();
            dictionarySnapShotService.Setup(o => o.Exists(It.IsAny<IDirectoryInfo>())).Returns(true);
            dictionarySnapShotService.Setup(o => o.Read(leftDirectoryInfo.Object)).Returns(previousLeftDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Read(rightDirectoryInfo.Object)).Returns(previousRightDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Update(leftDirectoryInfo.Object)).Returns(leftDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Update(rightDirectoryInfo.Object)).Returns(rightDictionarySnapshot);

            var directorySnapshotServiceFactory = new Mock<IDirectorySnapshotServiceFactory>();
            directorySnapshotServiceFactory.Setup(o => o.GetDirectorySnapshotService(leftDirectoryInfo.Object, rightDirectoryInfo.Object)).Returns(dictionarySnapShotService.Object);

            var syncSharkService = new SyncSharkService(directorySnapshotServiceFactory.Object);

            // Act
            var result = syncSharkService.Compare(leftDirectoryInfo.Object, rightDirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(FILE_PATH_LEFT, array[0].Source.FullName);
            Assert.AreEqual(FILE_PATH_RIGHT, array[0].Destination.FullName);
            Assert.AreEqual(FileActions.DELETE, array[0].FileAction);
        }

        [Test]
        public void Compare_ShouldDeleteRightFileIfDeletedFromLeft()
        {
            // Arrange
            var previousLeftDirfileInfo = new Mock<IFileInfo>();
            previousLeftDirfileInfo.Setup(o => o.FullName).Returns(FILE_PATH_LEFT);
            previousLeftDirfileInfo.Setup(o => o.LastWriteTimeUtc).Returns(new DateTime(2014, 01, 01, 0, 0, 0, 0));
            IFileInfo[] previousLeftFileInfos = new IFileInfo[1];
            previousLeftFileInfos[0] = previousLeftDirfileInfo.Object;

            var previousRightDirfileInfo = new Mock<IFileInfo>();
            previousRightDirfileInfo.Setup(o => o.FullName).Returns(FILE_PATH_RIGHT);
            previousRightDirfileInfo.Setup(o => o.LastWriteTimeUtc).Returns(new DateTime(2014, 01, 01, 0, 0, 0, 0));
            IFileInfo[] previousRightFileInfos = new IFileInfo[1];
            previousRightFileInfos[0] = previousRightDirfileInfo.Object;

            var leftDirfileInfo = new Mock<IFileInfo>();
            leftDirfileInfo.Setup(o => o.FullName).Returns(FILE_PATH_LEFT);
            leftDirfileInfo.Setup(o => o.LastWriteTimeUtc).Returns(new DateTime(2014, 01, 01, 0, 0, 0, 1));
            IFileInfo[] leftFileInfos = new IFileInfo[1];
            leftFileInfos[0] = leftDirfileInfo.Object;

            var rightDirFileInfo = new Mock<IFileInfo>();
            rightDirFileInfo.Setup(o => o.FullName).Returns(FILE_PATH_RIGHT);
            rightDirFileInfo.Setup(o => o.LastWriteTimeUtc).Returns(new DateTime(2014, 01, 01, 0, 0, 0, 2));
            IFileInfo[] rightFileInfos = new IFileInfo[1];
            rightFileInfos[0] = rightDirFileInfo.Object;

            var leftDirectoryInfo = new Mock<IDirectoryInfo>();
            leftDirectoryInfo.Setup(o => o.FullName).Returns(DIRECTORY_PATH_LEFT);
            leftDirectoryInfo.Setup(o => o.GetFiles("*.*", It.IsAny<SearchOption>())).Returns(leftFileInfos);

            var rightDirectoryInfo = new Mock<IDirectoryInfo>();
            rightDirectoryInfo.Setup(o => o.FullName).Returns(DIRECTORY_PATH_RIGHT);
            rightDirectoryInfo.Setup(o => o.GetFiles("*.*", It.IsAny<SearchOption>())).Returns(rightFileInfos);

            var previousLeftDictionarySnapshot = new Dictionary<string, IFileInfo>();
            previousLeftDictionarySnapshot.Add(FILE_RELATIVE_PATH, previousLeftDirfileInfo.Object);
            var previousRightDictionarySnapshot = new Dictionary<string, IFileInfo>();
            previousRightDictionarySnapshot.Add(FILE_RELATIVE_PATH, previousRightDirfileInfo.Object);

            var leftDictionarySnapshot = new Dictionary<string, IFileInfo>();
            leftDictionarySnapshot.Add(FILE_RELATIVE_PATH, leftDirfileInfo.Object);
            var rightDictionarySnapshot = new Dictionary<string, IFileInfo>();
            rightDictionarySnapshot.Add(FILE_RELATIVE_PATH, rightDirFileInfo.Object);

            var dictionarySnapShotService = new Mock<IDirectorySnapshotService>();
            dictionarySnapShotService.Setup(o => o.Exists(It.IsAny<IDirectoryInfo>())).Returns(true);
            dictionarySnapShotService.Setup(o => o.Read(leftDirectoryInfo.Object)).Returns(previousLeftDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Read(rightDirectoryInfo.Object)).Returns(previousRightDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Update(leftDirectoryInfo.Object)).Returns(leftDictionarySnapshot);
            dictionarySnapShotService.Setup(o => o.Update(rightDirectoryInfo.Object)).Returns(rightDictionarySnapshot);

            var directorySnapshotServiceFactory = new Mock<IDirectorySnapshotServiceFactory>();
            directorySnapshotServiceFactory.Setup(o => o.GetDirectorySnapshotService(leftDirectoryInfo.Object, rightDirectoryInfo.Object)).Returns(dictionarySnapShotService.Object);

            var syncSharkService = new SyncSharkService(directorySnapshotServiceFactory.Object);

            // Act
            var result = syncSharkService.Compare(leftDirectoryInfo.Object, rightDirectoryInfo.Object);

            // Assert
            var array = result.ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(FILE_PATH_LEFT, array[0].Source.FullName);
            Assert.AreEqual(FILE_PATH_RIGHT, array[0].Destination.FullName);
            Assert.AreEqual(FileActions.DELETE, array[0].FileAction);
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

            var DirectorySnapshotServiceFactory = new DirectorySnapshotServiceFactory();
            var syncSharkService = new SyncSharkService(DirectorySnapshotServiceFactory);

            // Act
            syncSharkService.Sync(syncWorkItemList);

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

            var DirectorySnapshotServiceFactory = new DirectorySnapshotServiceFactory();
            var syncSharkService = new SyncSharkService(DirectorySnapshotServiceFactory);

            // Act
            syncSharkService.Sync(syncWorkItemList);

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

            var DirectorySnapshotServiceFactory = new DirectorySnapshotServiceFactory();
            var syncSharkService = new SyncSharkService(DirectorySnapshotServiceFactory);

            // Act
            syncSharkService.Sync(syncWorkItemList);

            // Assert
            sourceFileInfo.Verify(o => o.OpenRead(), Times.Exactly(0));
            sourceFileInfo.Verify(o => o.OpenWrite(), Times.Exactly(0));
            sourceFileInfo.Verify(o => o.Delete(), Times.Exactly(0));
        }
    }
}

