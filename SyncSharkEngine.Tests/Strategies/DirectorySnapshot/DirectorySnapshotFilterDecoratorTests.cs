using Moq;
using NUnit.Framework;
using SyncSharkEngine.FileSystem;
using SyncSharkEngine.Strategies.DirectorySnapshot;
using SyncSharkEngine.Tests.Strategies.Compare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSharkEngine.Tests
{
    [TestFixture]
    public class DirectorySnapshotFilterDecoratorTests
    {
        private const string FOLDER = @"C:\Dir1\";
        private const string STORE_FILE = FileSystemSnapshotStrategy.STORE_FILE_NAME;
        private const string NON_STORE_FILE = "Test.txt";
        private CompareTestArgsFactory m_CompareTestArgsFactory;


        [SetUp]
        public void Setup()
        {
            m_CompareTestArgsFactory = new CompareTestArgsFactory();
        }

        [Test]
        public void Read_ShouldExcludeStoreDatabaseFile()
        {
            // Arrage
            CompareTestArgs compareTestArgs = m_CompareTestArgsFactory.GetSingleFile(FOLDER, STORE_FILE, new DateTime(2014, 1, 1, 0, 0, 0, 0));
            var snapshotStrategy = new Mock<IDirectorySnapshotStrategy>();
            snapshotStrategy.Setup(o => o.Read(compareTestArgs.DirectoryInfo.Object)).Returns(compareTestArgs.Snapshot);
            var filter = new DirectorySnapshotFilterDecorator(snapshotStrategy.Object);

            // Act
            var snapshot = filter.Read(compareTestArgs.DirectoryInfo.Object);

            // Assert
            Assert.AreEqual(0, snapshot.Count);
        }
        
        [Test]
        public void Read_ShouldIncludeNonStoreDatabaseFile()
        {
            // Arrage
            CompareTestArgs compareTestArgs = m_CompareTestArgsFactory.GetSingleFile(FOLDER, NON_STORE_FILE, new DateTime(2014, 1, 1, 0, 0, 0, 0));
            var snapshotStrategy = new Mock<IDirectorySnapshotStrategy>();
            snapshotStrategy.Setup(o => o.Read(compareTestArgs.DirectoryInfo.Object)).Returns(compareTestArgs.Snapshot);
            var filter = new DirectorySnapshotFilterDecorator(snapshotStrategy.Object);

            // Act
            var snapshot = filter.Read(compareTestArgs.DirectoryInfo.Object);

            // Assert
            Assert.AreEqual(1, snapshot.Count);
        }

        [Test]
        public void Create_ShouldExcludeStoreDatabaseFile()
        {
            // Arrage
            CompareTestArgs compareTestArgs = m_CompareTestArgsFactory.GetSingleFile(FOLDER, STORE_FILE, new DateTime(2014, 1, 1, 0, 0, 0, 0));
            var snapshotStrategy = new Mock<IDirectorySnapshotStrategy>();
            snapshotStrategy.Setup(o => o.Create(compareTestArgs.DirectoryInfo.Object)).Returns(compareTestArgs.Snapshot);
            var filter = new DirectorySnapshotFilterDecorator(snapshotStrategy.Object);

            // Act
            var snapshot = filter.Create(compareTestArgs.DirectoryInfo.Object);

            // Assert
            Assert.AreEqual(0, snapshot.Count);
        }

        [Test]
        public void Create_ShouldIncludeNonStoreDatabaseFile()
        {
            // Arrage
            CompareTestArgs compareTestArgs = m_CompareTestArgsFactory.GetSingleFile(FOLDER, NON_STORE_FILE, new DateTime(2014, 1, 1, 0, 0, 0, 0));
            var snapshotStrategy = new Mock<IDirectorySnapshotStrategy>();
            snapshotStrategy.Setup(o => o.Create(compareTestArgs.DirectoryInfo.Object)).Returns(compareTestArgs.Snapshot);
            var filter = new DirectorySnapshotFilterDecorator(snapshotStrategy.Object);

            // Act
            var snapshot = filter.Create(compareTestArgs.DirectoryInfo.Object);

            // Assert
            Assert.AreEqual(1, snapshot.Count);
        }

        [Test]
        public void Update_ShouldExcludeStoreDatabaseFile()
        {
            // Arrage
            CompareTestArgs compareTestArgs = m_CompareTestArgsFactory.GetSingleFile(FOLDER, STORE_FILE, new DateTime(2014, 1, 1, 0, 0, 0, 0));
            var snapshotStrategy = new Mock<IDirectorySnapshotStrategy>();
            snapshotStrategy.Setup(o => o.Update(compareTestArgs.DirectoryInfo.Object)).Returns(compareTestArgs.Snapshot);
            var filter = new DirectorySnapshotFilterDecorator(snapshotStrategy.Object);

            // Act
            var snapshot = filter.Update(compareTestArgs.DirectoryInfo.Object);

            // Assert
            Assert.AreEqual(0, snapshot.Count);
        }

        [Test]
        public void Update_ShouldIncludeNonStoreDatabaseFile()
        {
            // Arrage
            CompareTestArgs compareTestArgs = m_CompareTestArgsFactory.GetSingleFile(FOLDER, NON_STORE_FILE, new DateTime(2014, 1, 1, 0, 0, 0, 0));
            var snapshotStrategy = new Mock<IDirectorySnapshotStrategy>();
            snapshotStrategy.Setup(o => o.Update(compareTestArgs.DirectoryInfo.Object)).Returns(compareTestArgs.Snapshot);
            var filter = new DirectorySnapshotFilterDecorator(snapshotStrategy.Object);

            // Act
            var snapshot = filter.Update(compareTestArgs.DirectoryInfo.Object);

            // Assert
            Assert.AreEqual(1, snapshot.Count);
        }
    }
}
