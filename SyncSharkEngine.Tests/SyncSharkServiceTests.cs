using Moq;
using NUnit.Framework;
using SyncShark.Engine.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncShark.Engine.Strategies.Compare;
using SyncShark.Engine.Strategies;
using SyncShark.Interfaces;
using SyncShark.Engine;

namespace SyncSharkEngine.Tests
{
    [TestFixture]
    public class SyncSharkServiceTests
    {
        [Test]
        public void Sync_ShouldInvokeCompareAndExecute()
        {
            // Arrange
            var syncCompareStrategy = new Mock<IExecuteStrategy>();
            var mirrorCompareStrategy = new Mock<IExecuteStrategy>();
            var directoryInfo = new Mock<IDirectoryInfo>();

            var syncSharkService = new SyncSharkService(syncCompareStrategy.Object, mirrorCompareStrategy.Object);

            // Act
            syncSharkService.Sync(directoryInfo.Object, directoryInfo.Object);

            // Assert
            syncCompareStrategy.Verify(o => o.CompareAndExecute(It.IsAny<IDirectoryInfo>(), It.IsAny<IDirectoryInfo>()), Times.Exactly(1));
        }

        [Test]
        public void Mirror_ShouldInvokeCompareAndExecute()
        {
            // Arrange
            var syncCompareStrategy = new Mock<IExecuteStrategy>();
            var mirrorCompareStrategy = new Mock<IExecuteStrategy>();
            var directoryInfo = new Mock<IDirectoryInfo>();

            var syncSharkService = new SyncSharkService(syncCompareStrategy.Object, mirrorCompareStrategy.Object);

            // Act
            syncSharkService.Mirror(directoryInfo.Object, directoryInfo.Object);

            // Assert
            mirrorCompareStrategy.Verify(o => o.CompareAndExecute(It.IsAny<IDirectoryInfo>(), It.IsAny<IDirectoryInfo>()), Times.Exactly(1));
        }
    }
}

