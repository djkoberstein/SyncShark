using Moq;
using NUnit.Framework;
using SyncSharkEngine.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncSharkEngine.Strategies.Compare;
using SyncSharkEngine.Strategies;

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

            var syncSharkService = new SyncSharkService(syncCompareStrategy.Object, mirrorCompareStrategy.Object);

            // Act
            syncSharkService.Sync();

            // Assert
            syncCompareStrategy.Verify(o => o.CompareAndExecute(), Times.Exactly(1));
        }

        [Test]
        public void Mirror_ShouldInvokeCompareAndExecute()
        {
            // Arrange
            var syncCompareStrategy = new Mock<IExecuteStrategy>();
            var mirrorCompareStrategy = new Mock<IExecuteStrategy>();

            var syncSharkService = new SyncSharkService(syncCompareStrategy.Object, mirrorCompareStrategy.Object);

            // Act
            syncSharkService.Mirror();

            // Assert
            mirrorCompareStrategy.Verify(o => o.CompareAndExecute(), Times.Exactly(1));
        }

    }
}

