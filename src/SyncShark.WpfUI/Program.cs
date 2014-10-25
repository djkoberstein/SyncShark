using SyncShark.Engine;
using SyncShark.Engine.FileSystem;
using SyncShark.Engine.Strategies;
using SyncShark.Engine.Strategies.Compare;
using SyncShark.Engine.Strategies.DirectorySnapshot;
using SyncShark.Interfaces;
using SyncShark.WpfUI.ViewModels;
using SyncShark.WpfUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SyncShark.WpfUI
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var app = Componse();
            app.Start();
        }

        static App Componse()
        {
            var fileSystemInfoFactory = new FileSystemInfoFactory();

            // Mirror components
            var memorySnapshotStrategy = new MemorySnapshotStrategy();
            var mirrorCompareStrategy = new MirrorCompareStrategy(memorySnapshotStrategy, fileSystemInfoFactory);
            var mirrorExecuteStrategy = new ExecuteStrategy(mirrorCompareStrategy);

            // Sync components
            var fileSystemSnapshotStrategy = new FileSystemSnapshotStrategy(memorySnapshotStrategy);
            var snapshotFilter = new DirectorySnapshotBlacklistFilter(fileSystemSnapshotStrategy);
            var syncCompareStrategy = new SyncCompareStrategy(snapshotFilter, fileSystemInfoFactory);
            var syncExecuteStrategy = new ExecuteStrategy(syncCompareStrategy);

            // Service
            var syncSharkService = new SyncSharkService(syncExecuteStrategy, mirrorExecuteStrategy);

            var workItemViewModel = new WorkItemViewModel();
            var configurationViewModel = new ConfigurationViewModel();
            var mainWindowViewModel = new MainWindowViewModel(workItemViewModel, configurationViewModel);
            var mainWindowView = new MainWindowView() { DataContext = mainWindowViewModel };
            var app = new App(mainWindowView);

            return app;
        }
    }
}
